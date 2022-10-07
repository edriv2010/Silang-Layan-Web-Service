// Silang_Layan_Web_Service.Restful
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Restful : Page
{


	protected void Page_Load(object sender, EventArgs e)
	{
		if (Page.Request["op"] == null)
		{
			base.Response.Clear();
			base.Response.Write("Error : Operation required. Check corresponding manual");
			base.Response.End();
			return;
		}
		switch (Page.Request["op"].ToString().ToLower())
		{
		case "setservicestatuson":
		{
			if (Page.Request["branchcode"] == null)
			{
				base.Response.Clear();
				base.Response.Write("Error : BranchCode required. Check corresponding manual");
				base.Response.End();
				break;
			}
			string BranchCode = Page.Request["branchcode"].ToString();
			TwoArrayList tar = new TwoArrayList();
			tar.Clear();
			tar.Add("CODE", BranchCode.ToUpper());
			string BranchId = Command.ExecScalar(tar, "SELECT ID FROM BRANCHS WHERE UPPER(CODE) = " + Connection.ParameterSymbol + "CODE");
			if (string.IsNullOrEmpty(BranchId))
			{
				base.Response.Clear();
				base.Response.Write("Error : Kode Perpustakaan tidak dapat ditemukan");
				base.Response.End();
			}
			else
			{
				base.Response.Clear();
				base.Response.Write(RestfulService.SetServiceStatusON(this, BranchId));
				base.Response.End();
			}
			break;
		}
		case "registermember":
		{
			string CreateBy = "";
			string NamaAnggota = "";
			string EmailAddress = "";
			string JenisIdentitas = "";
			string NomorIdentitas = "";
			string TempatLahir = "";
			string TanggalLahir = "";
			string AlamatKTP = "";
			string AlamatSekarang = "";
			string HomePhoneNumber = "";
			string InstitutionName = "";
			string InstitutionAddress = "";
			string InstitutionPhoneNumber = "";
			string HandPhone = "";
			string IbuKandung = "";
			string JenisKelamin = "";
			string ProvinsiKTP = "";
			string KabupatenKTP = "";
			string KecamatanKTP = "";
			string KelurahanKTP = "";
			string RWKTP = "";
			string RTKTP = "";
			string ProvinsiSekarang = "";
			string KabupatenSekarang = "";
			string KecamatanSekarang = "";
			string KelurahanSekarang = "";
			string RWSekarang = "";
			string RTSekarang = "";
			string Pendidikan = "";
			string StatusPerkawinan = "";
			string Pekerjaan = "";
			string PekerjaanLainnya = "";
			string JenisAnggota = "";
			if (Page.Request["xml_doc"] != null)
			{
				string xml_doc = Page.Request["xml_doc"].ToString();
				NameValueCollection parameters = HttpUtility.ParseQueryString(xml_doc);
				CreateBy = HttpUtility.UrlDecode(parameters.Get("CreateBy"));
				NamaAnggota = HttpUtility.UrlDecode(parameters.Get("NamaAnggota"));
				EmailAddress = HttpUtility.UrlDecode(parameters.Get("EmailAddress"));
				JenisIdentitas = HttpUtility.UrlDecode(parameters.Get("JenisIdentitas"));
				NomorIdentitas = HttpUtility.UrlDecode(parameters.Get("NomorIdentitas"));
				TempatLahir = HttpUtility.UrlDecode(parameters.Get("TempatLahir"));
				TanggalLahir = HttpUtility.UrlDecode(parameters.Get("TanggalLahir"));
				AlamatKTP = HttpUtility.UrlDecode(parameters.Get("AlamatKTP"));
				AlamatSekarang = HttpUtility.UrlDecode(parameters.Get("AlamatSekarang"));
				HomePhoneNumber = HttpUtility.UrlDecode(parameters.Get("HomePhoneNumber"));
				InstitutionName = HttpUtility.UrlDecode(parameters.Get("InstitutionName"));
				InstitutionAddress = HttpUtility.UrlDecode(parameters.Get("InstitutionAddress"));
				InstitutionPhoneNumber = HttpUtility.UrlDecode(parameters.Get("InstitutionPhoneNumber"));
				HandPhone = HttpUtility.UrlDecode(parameters.Get("HandPhone"));
				IbuKandung = HttpUtility.UrlDecode(parameters.Get("IbuKandung"));
				JenisKelamin = HttpUtility.UrlDecode(parameters.Get("JenisKelamin"));
				ProvinsiKTP = HttpUtility.UrlDecode(parameters.Get("ProvinsiKTP"));
				KabupatenKTP = HttpUtility.UrlDecode(parameters.Get("KabupatenKTP"));
				KecamatanKTP = HttpUtility.UrlDecode(parameters.Get("KecamatanKTP"));
				KelurahanKTP = HttpUtility.UrlDecode(parameters.Get("KelurahanKTP"));
				RWKTP = HttpUtility.UrlDecode(parameters.Get("RWKTP"));
				RTKTP = HttpUtility.UrlDecode(parameters.Get("RTKTP"));
				ProvinsiSekarang = HttpUtility.UrlDecode(parameters.Get("ProvinsiSekarang"));
				KabupatenSekarang = HttpUtility.UrlDecode(parameters.Get("KabupatenSekarang"));
				KecamatanSekarang = HttpUtility.UrlDecode(parameters.Get("KecamatanSekarang"));
				KelurahanSekarang = HttpUtility.UrlDecode(parameters.Get("KelurahanSekarang"));
				RWSekarang = HttpUtility.UrlDecode(parameters.Get("RWSekarang"));
				RTSekarang = HttpUtility.UrlDecode(parameters.Get("RTSekarang"));
				Pendidikan = HttpUtility.UrlDecode(parameters.Get("Pendidikan"));
				StatusPerkawinan = HttpUtility.UrlDecode(parameters.Get("StatusPerkawinan"));
				Pekerjaan = HttpUtility.UrlDecode(parameters.Get("Pekerjaan"));
				PekerjaanLainnya = HttpUtility.UrlDecode(parameters.Get("PekerjaanLainnya"));
				JenisAnggota = HttpUtility.UrlDecode(parameters.Get("JenisAnggota"));
			}
			else
			{
				if (Page.Request["createby"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : CreateBy required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["namaanggota"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : NamaAnggota required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["emailaddress"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : EmailAddress required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["jenisidentitas"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : JenisIdentitas required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["nomoridentitas"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : NomorIdentitas required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["tempatlahir"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : TempatLahir required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["tanggallahir"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : TanggalLahir required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["alamatktp"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : AlamatKTP required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["alamatsekarang"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : AlamatSekarang required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["homephonenumber"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : HomePhoneNumber required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["institutionname"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : InstitutionName required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["institutionaddress"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : InstitutionAddress required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["institutionphonenumber"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : InstitutionPhoneNumber required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["handphone"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : HandPhone required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["ibukandung"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : IbuKandung required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["jeniskelamin"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : JenisKelamin required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["provinsiktp"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : ProvinsiKTP required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["kabupatenktp"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : KabupatenKTP required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["kecamatanktp"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : KecamatanKTP required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["kelurahanktp"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : KelurahanKTP required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["rwktp"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : RWKTP required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["rtktp"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : RTKTP required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["provinsisekarang"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : ProvinsiSekarang required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["kabupatensekarang"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : KabupatenSekarang required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["kecamatansekarang"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : KecamatanSekarang required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["kelurahansekarang"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : KelurahanSekarang required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["rwsekarang"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : RWSekarang required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["rtsekarang"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : RTSekarang required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["pendidikan"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : Pendidikan required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["statusperkawinan"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : StatusPerkawinan required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["pekerjaan"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : Pekerjaan required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["pekerjaanlainnya"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : PekerjaanLainnya required. Check corresponding manual");
					base.Response.End();
					break;
				}
				if (Page.Request["jenisanggota"] == null)
				{
					base.Response.Clear();
					base.Response.Write("Error : JenisAnggota required. Check corresponding manual");
					base.Response.End();
					break;
				}
				CreateBy = HttpUtility.UrlDecode(Page.Request["CreateBy"].ToString());
				NamaAnggota = HttpUtility.UrlDecode(Page.Request["NamaAnggota"].ToString());
				EmailAddress = HttpUtility.UrlDecode(Page.Request["EmailAddress"].ToString());
				JenisIdentitas = HttpUtility.UrlDecode(Page.Request["JenisIdentitas"].ToString());
				NomorIdentitas = HttpUtility.UrlDecode(Page.Request["NomorIdentitas"].ToString());
				TempatLahir = HttpUtility.UrlDecode(Page.Request["TempatLahir"].ToString());
				TanggalLahir = HttpUtility.UrlDecode(Page.Request["TanggalLahir"].ToString());
				AlamatKTP = HttpUtility.UrlDecode(Page.Request["AlamatKTP"].ToString());
				AlamatSekarang = HttpUtility.UrlDecode(Page.Request["AlamatSekarang"].ToString());
				HomePhoneNumber = HttpUtility.UrlDecode(Page.Request["HomePhoneNumber"].ToString());
				InstitutionName = HttpUtility.UrlDecode(Page.Request["InstitutionName"].ToString());
				InstitutionAddress = HttpUtility.UrlDecode(Page.Request["InstitutionAddress"].ToString());
				InstitutionPhoneNumber = HttpUtility.UrlDecode(Page.Request["InstitutionPhoneNumber"].ToString());
				HandPhone = HttpUtility.UrlDecode(Page.Request["HandPhone"].ToString());
				IbuKandung = HttpUtility.UrlDecode(Page.Request["IbuKandung"].ToString());
				JenisKelamin = HttpUtility.UrlDecode(Page.Request["JenisKelamin"].ToString());
				ProvinsiKTP = HttpUtility.UrlDecode(Page.Request["ProvinsiKTP"].ToString());
				KabupatenKTP = HttpUtility.UrlDecode(Page.Request["KabupatenKTP"].ToString());
				KecamatanKTP = HttpUtility.UrlDecode(Page.Request["KecamatanKTP"].ToString());
				KelurahanKTP = HttpUtility.UrlDecode(Page.Request["KelurahanKTP"].ToString());
				RWKTP = HttpUtility.UrlDecode(Page.Request["RWKTP"].ToString());
				RTKTP = HttpUtility.UrlDecode(Page.Request["RTKTP"].ToString());
				ProvinsiSekarang = HttpUtility.UrlDecode(Page.Request["ProvinsiSekarang"].ToString());
				KabupatenSekarang = HttpUtility.UrlDecode(Page.Request["KabupatenSekarang"].ToString());
				KecamatanSekarang = HttpUtility.UrlDecode(Page.Request["KecamatanSekarang"].ToString());
				KelurahanSekarang = HttpUtility.UrlDecode(Page.Request["KelurahanSekarang"].ToString());
				RWSekarang = HttpUtility.UrlDecode(Page.Request["RWSekarang"].ToString());
				RTSekarang = HttpUtility.UrlDecode(Page.Request["RTSekarang"].ToString());
				Pendidikan = HttpUtility.UrlDecode(Page.Request["Pendidikan"].ToString());
				StatusPerkawinan = HttpUtility.UrlDecode(Page.Request["StatusPerkawinan"].ToString());
				Pekerjaan = HttpUtility.UrlDecode(Page.Request["Pekerjaan"].ToString());
				PekerjaanLainnya = HttpUtility.UrlDecode(Page.Request["PekerjaanLainnya"].ToString());
				JenisAnggota = HttpUtility.UrlDecode(Page.Request["JenisAnggota"].ToString());
			}
			string BranchCode = CreateBy;
			TwoArrayList tar = new TwoArrayList();
			tar.Clear();
			tar.Add("CODE", BranchCode.ToUpper());
			string BranchId = Command.ExecScalar(tar, "SELECT ID FROM BRANCHS WHERE UPPER(CODE) = " + Connection.ParameterSymbol + "CODE");
			if (string.IsNullOrEmpty(BranchId))
			{
				base.Response.Clear();
				base.Response.Write("Error : Kode Perpustakaan tidak dapat ditemukan");
				base.Response.End();
			}
			else
			{
				base.Response.Clear();
				base.Response.Write(RestfulService.RegisterMember(this, BranchId, CreateBy, NamaAnggota, EmailAddress, JenisIdentitas, NomorIdentitas, TempatLahir, TanggalLahir, AlamatKTP, AlamatSekarang, HomePhoneNumber, InstitutionName, InstitutionAddress, InstitutionPhoneNumber, HandPhone, IbuKandung, JenisKelamin, ProvinsiKTP, KabupatenKTP, KecamatanKTP, KelurahanKTP, RWKTP, RTKTP, ProvinsiSekarang, KabupatenSekarang, KecamatanSekarang, KelurahanSekarang, RWSekarang, RTSekarang, Pendidikan, StatusPerkawinan, Pekerjaan, PekerjaanLainnya, JenisAnggota));
				base.Response.End();
			}
			break;
		}
		default:
			base.Response.Clear();
			base.Response.Write("Error : Operation doesn't exist. Check corresponding manual");
			base.Response.End();
			break;
		}
	}
}
