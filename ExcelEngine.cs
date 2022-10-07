// ExcelEngine
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

public class ExcelEngine
{
	public static string CurrentErrorMessage = "";

	public static DataTable ExcelToDataTable(string FilePath, string FolderImageImport = "")
	{
		DataTable dt = new DataTable();
		SpreadsheetDocument spreadSheetDocument = null;
		try
		{
			string FileName = System.IO.Path.GetFileNameWithoutExtension(FilePath);
			spreadSheetDocument = SpreadsheetDocument.Open(FilePath, isEditable: false);
			WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
			IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
			string relationshipId = sheets.First().Id.Value;
			WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
			Worksheet workSheet = worksheetPart.Worksheet;
			SheetData sheetData = workSheet.GetFirstChild<SheetData>();
			IEnumerable<Row> rows = sheetData.Descendants<Row>();
			List<string> columnRef = new List<string>();
			foreach (Cell cell in rows.ElementAt(0))
			{
				dt.Columns.Add(GetCellValue(spreadSheetDocument, cell));
				columnRef.Add(cell.CellReference.ToString().Substring(0, cell.CellReference.ToString().Length - 1));
			}
			int ColumnCount = dt.Columns.Count;
			int rowIndex = 0;
			foreach (Row row in rows)
			{
				DataRow tempRow = dt.NewRow();
				int columnIndex = 0;
				foreach (Cell cell in row.Descendants<Cell>())
				{
					int cellColumnIndex = GetColumnIndexFromName(GetColumnName(cell.CellReference)).Value;
					cellColumnIndex--;
					if (columnIndex < cellColumnIndex)
					{
						do
						{
							tempRow[columnIndex] = "";
							columnIndex++;
						}
						while (columnIndex < cellColumnIndex);
					}
					if (dt.Columns[columnIndex].ColumnName != "Foto" || string.IsNullOrEmpty(FolderImageImport))
					{
						tempRow[columnIndex] = GetCellValue(spreadSheetDocument, cell);
					}
					else if (worksheetPart.DrawingsPart != null)
					{
						TwoCellAnchor cellHoldingPicture = (from x in worksheetPart.DrawingsPart.WorksheetDrawing.OfType<TwoCellAnchor>()
							where x.FromMarker.RowId.Text == row.RowIndex && x.FromMarker.ColumnId.Text == columnIndex.ToString()
							select x).FirstOrDefault();
						if (cellHoldingPicture != null)
						{
							DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picture = cellHoldingPicture.OfType<DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture>().FirstOrDefault();
							if (picture != null)
							{
								string rIdofPicture = picture.BlipFill.Blip.Embed;
								ImagePart imagePart = (ImagePart)worksheetPart.DrawingsPart.GetPartById(rIdofPicture);
								Stream stream = imagePart.GetStream();
								long length = stream.Length;
								byte[] byteStream = new byte[length];
								stream.Read(byteStream, 0, (int)length);
								FileStream fstream = new FileStream(System.IO.Path.Combine(FolderImageImport, FileName + "_" + rowIndex + ".jpg"), FileMode.OpenOrCreate);
								fstream.Write(byteStream, 0, (int)length);
								fstream.Close();
							}
						}
					}
					columnIndex++;
				}
				dt.Rows.Add(tempRow);
			}
			dt.Rows.RemoveAt(0);
			spreadSheetDocument.Close();
			spreadSheetDocument = null;
			workbookPart = null;
			sheets = null;
			worksheetPart = null;
			sheetData = null;
		}
		catch (Exception ex)
		{
			CurrentErrorMessage = ex.Message;
			try
			{
				spreadSheetDocument.Close();
			}
			catch
			{
			}
			spreadSheetDocument = null;
			WorkbookPart workbookPart = null;
			IEnumerable<Sheet> sheets = null;
			WorksheetPart worksheetPart = null;
			SheetData sheetData = null;
			return null;
		}
		return dt;
	}

	public static string GetCellValue(SpreadsheetDocument document, Cell cell)
	{
		SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
		if (cell.CellValue == null)
		{
			return null;
		}
		string value = cell.CellValue.InnerXml;
		if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
		{
			return stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
		}
		return value;
	}

	public static string GetColumnName(string cellReference)
	{
		Regex regex = new Regex("[A-Za-z]+");
		Match match = regex.Match(cellReference);
		return match.Value;
	}

	public static int? GetColumnIndexFromName(string columnName)
	{
		int number = 0;
		int pow = 1;
		for (int i = columnName.Length - 1; i >= 0; i--)
		{
			number += (columnName[i] - 65 + 1) * pow;
			pow *= 26;
		}
		return number;
	}

	private static int CellReferenceToIndex(Cell cell)
	{
		int index = 0;
		string reference = cell.CellReference.ToString().ToUpper();
		string text = reference;
		foreach (char ch in text)
		{
			if (char.IsLetter(ch))
			{
				int value = ch - 65;
				index = ((index == 0) ? value : ((index + 1) * 26 + value));
				continue;
			}
			return index;
		}
		return index;
	}

	public static void ExportDataTableToExcel(DataTable dt, string destination, string FotoFolderPath)
	{
		DataSet ds = new DataSet();
		ds.Tables.Add(dt);
		ExportDataSetToExcel(ds, destination, FotoFolderPath);
	}

	public static void ExportDataSetToExcel(DataSet ds, string destination, string FotoFolderPath)
	{
		if (File.Exists(destination))
		{
			File.Delete(destination);
		}
		SpreadsheetDocument workbook = SpreadsheetDocument.Create(destination, SpreadsheetDocumentType.Workbook);
		WorkbookPart workbookPart = workbook.AddWorkbookPart();
		workbook.WorkbookPart.Workbook = new Workbook();
		workbook.WorkbookPart.Workbook.Sheets = new Sheets();
		uint sheetId = 1u;
		WorkbookStylesPart stylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
		stylesPart.Stylesheet = GenerateStyleSheet();
		stylesPart.Stylesheet.Save();
		foreach (DataTable table in ds.Tables)
		{
			WorksheetPart sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
			SheetData sheetData = new SheetData();
			sheetPart.Worksheet = new Worksheet(sheetData);
			Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
			string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);
			if (sheets.Elements<Sheet>().Count() > 0)
			{
				sheetId = (from s in sheets.Elements<Sheet>()
					select s.SheetId.Value).Max() + 1;
			}
			Sheet sheet2 = new Sheet();
			sheet2.Id = relationshipId;
			sheet2.SheetId = sheetId;
			sheet2.Name = table.TableName;
			Sheet sheet = sheet2;
			sheets.Append(sheet);
			DrawingsPart drawingsPart = sheetPart.AddNewPart<DrawingsPart>();
			if (!sheetPart.Worksheet.ChildElements.OfType<Drawing>().Any())
			{
				sheetPart.Worksheet.Append(new Drawing
				{
					Id = sheetPart.GetIdOfPart(drawingsPart)
				});
			}
			if (drawingsPart.WorksheetDrawing == null)
			{
				drawingsPart.WorksheetDrawing = new WorksheetDrawing();
			}
			Row headerRow = new Row();
			ArrayList ListColumLength = new ArrayList();
			int k = 1;
			List<string> columns = new List<string>();
			foreach (DataColumn column in table.Columns)
			{
				columns.Add(column.ColumnName);
				Cell cell = new Cell();
				cell.DataType = CellValues.String;
				cell.CellValue = new CellValue(column.ColumnName);
				cell.StyleIndex = 1u;
				headerRow.AppendChild(cell);
				ListColumLength.Add(column.ColumnName.Length);
				SetColumnWidth(sheetPart.Worksheet, Convert.ToUInt32(k), 20.0);
				k++;
			}
			sheetData.AppendChild(headerRow);
			int i = 0;
			foreach (DataRow dsrow in table.Rows)
			{
				Row newRow = new Row();
				int j = 0;
				foreach (string col in columns)
				{
					Cell cell = new Cell();
					if (col != "Foto")
					{
						cell.DataType = CellValues.String;
						cell.CellValue = new CellValue(dsrow[col].ToString());
						cell.StyleIndex = 2u;
						newRow.AppendChild(cell);
						if (dsrow[col].ToString().Length > int.Parse(ListColumLength[j].ToString()))
						{
							ListColumLength[j] = dsrow[col].ToString().Length;
						}
					}
					else
					{
						if (!string.IsNullOrEmpty(FotoFolderPath) && !string.IsNullOrEmpty(dsrow["Foto"].ToString()))
						{
							string FilePath = System.IO.Path.Combine(FotoFolderPath, dsrow["Foto"].ToString());
							if (File.Exists(FilePath))
							{
								cell.DataType = CellValues.String;
								cell.CellValue = new CellValue("");
								cell.StyleIndex = 2u;
								newRow.AppendChild(cell);
								InsertImage(sheetPart, drawingsPart, FilePath, i + 2, j + 1);
							}
							else
							{
								cell.DataType = CellValues.String;
								cell.CellValue = new CellValue("");
								cell.StyleIndex = 2u;
								newRow.AppendChild(cell);
							}
						}
						else
						{
							cell.DataType = CellValues.String;
							cell.CellValue = new CellValue("");
							cell.StyleIndex = 2u;
							newRow.AppendChild(cell);
						}
						if (dsrow[col].ToString().Length > int.Parse(ListColumLength[j].ToString()))
						{
							ListColumLength[j] = dsrow[col].ToString().Length;
						}
					}
					j++;
				}
				sheetData.AppendChild(newRow);
				i++;
			}
		}
	}

	private static void InsertImage(WorksheetPart worksheetPart, DrawingsPart drawingsPart, string imageFilePath, int rowNumber, int colNumber)
	{
		WorksheetDrawing worksheetDrawing = drawingsPart.WorksheetDrawing;
		ImagePart imagePart = drawingsPart.AddImagePart(ImagePartType.Jpeg);
		using (FileStream stream = new FileStream(imageFilePath, FileMode.Open))
		{
			imagePart.FeedData(stream);
		}
		Bitmap bm = new Bitmap(imageFilePath);
		float ratio = (float)bm.Height / (float)bm.Width;
		long w = 1280160L;
		Extents extents = new Extents();
		long extentsCx = w;
		long extentsCy = (long)(ratio * (float)w);
		bm.Dispose();
		int colOffset = 0;
		int rowOffset = 0;
		IEnumerable<DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties> nvps = worksheetDrawing.Descendants<DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties>();
		uint nvpId = ((nvps.Count() <= 0) ? 1u : ((uint)(UInt32Value)worksheetDrawing.Descendants<DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties>().Max((DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties p) => p.Id.Value) + 1));
		OneCellAnchor oneCellAnchor = new OneCellAnchor(new DocumentFormat.OpenXml.Drawing.Spreadsheet.FromMarker
		{
			ColumnId = new ColumnId((colNumber - 1).ToString()),
			RowId = new RowId((rowNumber - 1).ToString()),
			ColumnOffset = new ColumnOffset(colOffset.ToString()),
			RowOffset = new RowOffset(rowOffset.ToString())
		}, new Extent
		{
			Cx = extentsCx,
			Cy = extentsCy
		}, new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture(new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureProperties(new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualDrawingProperties
		{
			Id = nvpId,
			Name = "Picture " + nvpId,
			Description = imageFilePath
		}, new DocumentFormat.OpenXml.Drawing.Spreadsheet.NonVisualPictureDrawingProperties(new PictureLocks
		{
			NoChangeAspect = true
		})), new DocumentFormat.OpenXml.Drawing.Spreadsheet.BlipFill(new Blip
		{
			Embed = drawingsPart.GetIdOfPart(imagePart),
			CompressionState = BlipCompressionValues.Print
		}, new Stretch(new FillRectangle())), new DocumentFormat.OpenXml.Drawing.Spreadsheet.ShapeProperties(new Transform2D(new Offset
		{
			X = 0L,
			Y = 0L
		}, new Extents
		{
			Cx = extentsCx,
			Cy = extentsCy
		}), new PresetGeometry
		{
			Preset = ShapeTypeValues.Rectangle
		})), new ClientData());
		worksheetDrawing.Append(oneCellAnchor);
	}

	private static Stylesheet GenerateStyleSheet()
	{
		return new Stylesheet(new DocumentFormat.OpenXml.Spreadsheet.Fonts(new DocumentFormat.OpenXml.Spreadsheet.Font(new FontSize
		{
			Val = 11.0
		}, new DocumentFormat.OpenXml.Spreadsheet.Color
		{
			Rgb = new HexBinaryValue
			{
				Value = "000000"
			}
		}, new FontName
		{
			Val = "Calibri"
		}), new DocumentFormat.OpenXml.Spreadsheet.Font(new Bold(), new FontSize
		{
			Val = 11.0
		}, new DocumentFormat.OpenXml.Spreadsheet.Color
		{
			Rgb = new HexBinaryValue
			{
				Value = "000000"
			}
		}, new FontName
		{
			Val = "Calibri"
		}), new DocumentFormat.OpenXml.Spreadsheet.Font(new Italic(), new FontSize
		{
			Val = 11.0
		}, new DocumentFormat.OpenXml.Spreadsheet.Color
		{
			Rgb = new HexBinaryValue
			{
				Value = "000000"
			}
		}, new FontName
		{
			Val = "Calibri"
		}), new DocumentFormat.OpenXml.Spreadsheet.Font(new FontSize
		{
			Val = 16.0
		}, new DocumentFormat.OpenXml.Spreadsheet.Color
		{
			Rgb = new HexBinaryValue
			{
				Value = "000000"
			}
		}, new FontName
		{
			Val = "Times New Roman"
		})), new Fills(new DocumentFormat.OpenXml.Spreadsheet.Fill(new DocumentFormat.OpenXml.Spreadsheet.PatternFill
		{
			PatternType = PatternValues.None
		}), new DocumentFormat.OpenXml.Spreadsheet.Fill(new DocumentFormat.OpenXml.Spreadsheet.PatternFill
		{
			PatternType = PatternValues.Gray125
		}), new DocumentFormat.OpenXml.Spreadsheet.Fill(new DocumentFormat.OpenXml.Spreadsheet.PatternFill(new DocumentFormat.OpenXml.Spreadsheet.ForegroundColor
		{
			Rgb = new HexBinaryValue
			{
				Value = "FFFFFF00"
			}
		})
		{
			PatternType = PatternValues.Solid
		}), new DocumentFormat.OpenXml.Spreadsheet.Fill(new DocumentFormat.OpenXml.Spreadsheet.PatternFill(new DocumentFormat.OpenXml.Spreadsheet.ForegroundColor
		{
			Rgb = new HexBinaryValue
			{
				Value = "FFDBEEF3"
			}
		})
		{
			PatternType = PatternValues.Solid
		})), new Borders(new Border(new DocumentFormat.OpenXml.Spreadsheet.LeftBorder(), new DocumentFormat.OpenXml.Spreadsheet.RightBorder(), new DocumentFormat.OpenXml.Spreadsheet.TopBorder(), new DocumentFormat.OpenXml.Spreadsheet.BottomBorder(), new DiagonalBorder()), new Border(new DocumentFormat.OpenXml.Spreadsheet.LeftBorder(new DocumentFormat.OpenXml.Spreadsheet.Color
		{
			Auto = true
		})
		{
			Style = BorderStyleValues.Thin
		}, new DocumentFormat.OpenXml.Spreadsheet.RightBorder(new DocumentFormat.OpenXml.Spreadsheet.Color
		{
			Auto = true
		})
		{
			Style = BorderStyleValues.Thin
		}, new DocumentFormat.OpenXml.Spreadsheet.TopBorder(new DocumentFormat.OpenXml.Spreadsheet.Color
		{
			Auto = true
		})
		{
			Style = BorderStyleValues.Thin
		}, new DocumentFormat.OpenXml.Spreadsheet.BottomBorder(new DocumentFormat.OpenXml.Spreadsheet.Color
		{
			Auto = true
		})
		{
			Style = BorderStyleValues.Thin
		}, new DiagonalBorder())), new CellFormats(new CellFormat
		{
			FontId = 0u,
			FillId = 0u,
			BorderId = 0u
		}, new CellFormat
		{
			FontId = 1u,
			FillId = 3u,
			BorderId = 1u,
			ApplyFont = true
		}, new CellFormat
		{
			FontId = 0u,
			FillId = 0u,
			BorderId = 1u,
			ApplyFont = true,
			Alignment = new Alignment
			{
				Vertical = VerticalAlignmentValues.Top,
				WrapText = true
			}
		}, new CellFormat
		{
			FontId = 3u,
			FillId = 0u,
			BorderId = 0u,
			ApplyFont = true
		}, new CellFormat
		{
			FontId = 0u,
			FillId = 2u,
			BorderId = 0u,
			ApplyFill = true
		}, new CellFormat(new Alignment
		{
			Horizontal = HorizontalAlignmentValues.Center,
			Vertical = VerticalAlignmentValues.Center
		})
		{
			FontId = 0u,
			FillId = 0u,
			BorderId = 0u,
			ApplyAlignment = true
		}, new CellFormat
		{
			FontId = 0u,
			FillId = 0u,
			BorderId = 1u,
			ApplyBorder = true
		}));
	}

	private static void SetColumnWidth(Worksheet worksheet, uint Index, DoubleValue dwidth, bool hidden = false)
	{
		Columns cs = worksheet.GetFirstChild<Columns>();
		if (cs != null)
		{
			IEnumerable<Column> ic = from r in cs.Elements<Column>()
				where (uint)r.Min == Index
				where (uint)r.Max == Index
				select r;
			Column c;
			if (ic.Count() > 0)
			{
				c = ic.First();
				c.Width = dwidth;
				return;
			}
			cs = new Columns();
			Column column = new Column();
			column.Min = Index;
			column.Max = Index;
			column.Hidden = hidden;
			column.Width = dwidth;
			column.CustomWidth = true;
			c = column;
			cs.Append(c);
			worksheet.InsertAfter(cs, worksheet.GetFirstChild<SheetProperties>());
		}
		else
		{
			cs = new Columns();
			Column column2 = new Column();
			column2.Min = Index;
			column2.Max = Index;
			column2.Hidden = hidden;
			column2.Width = dwidth;
			column2.CustomWidth = true;
			Column c = column2;
			cs.Append(c);
			worksheet.InsertAfter(cs, worksheet.GetFirstChild<SheetProperties>());
		}
	}

	private static double GetWidth(string text)
	{
		double width = ((double)new Size(2, 3).Width / 7.0 * 256.0 - 18.0) / 256.0;
		return (double)decimal.Round((decimal)width + 0.2m, 2);
	}
}
