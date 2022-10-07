// RestfulService
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web.UI;

public class RestfulService
{
	private class MemberItemResult
	{
		public float MemberID { get; set; }

		public string MemberNo { get; set; }

		public string Fullname { get; set; }

		public string PlaceOfBirth { get; set; }

		public DateTime DateOfBirth { get; set; }

		public string Address { get; set; }

		public string City { get; set; }

		public string Province { get; set; }

		public string AddressNow { get; set; }

		public string CityNow { get; set; }

		public string ProvinceNow { get; set; }

		public string Phone { get; set; }

		public string InstitutionName { get; set; }

		public string InstitutionAddress { get; set; }

		public string InstitutionPhone { get; set; }

		public string IdentityType { get; set; }

		public string IdentityNo { get; set; }

		public string EducationLevel { get; set; }

		public string Religion { get; set; }

		public string Sex { get; set; }

		public string MaritalStatus { get; set; }

		public string JobName { get; set; }

		public DateTime RegisterDate { get; set; }

		public DateTime EndDate { get; set; }

		public string MotherMaidenName { get; set; }

		public string Email { get; set; }

		public string JenisPermohonan { get; set; }

		public string AlamatDomisili { get; set; }

		public string NoHp { get; set; }

		public string JobNameDetail { get; set; }

		public string JenisPermohonanName { get; set; }

		public string JenisAnggota { get; set; }

		public string JenisAnggotaName { get; set; }

		public string StatusAnggota { get; set; }

		public string StatusAnggotaName { get; set; }
	}

	private class MemberOnlineItemResult
	{
		public float MemberOnlineID { get; set; }

		public string NoAnggota { get; set; }

		public string NickName { get; set; }

		public string Password { get; set; }

		public string Email { get; set; }

		public string Status { get; set; }

		public string Activation_Code { get; set; }
	}

	private class KabupatenItemResult
	{
		public string NamaPropinsi { get; set; }

		public string NamaKabupaten { get; set; }
	}

	private class JenisIdentitasItemResult
	{
		public string JenisIdentitas { get; set; }
	}

	private class JenisAnggotaItemResult
	{
		public string JenisAnggota { get; set; }
	}

	private class PendidikanItemResult
	{
		public string Pendidikan { get; set; }
	}

	private class GenderItemResult
	{
		public string Gender { get; set; }
	}

	private class StatusPerkawinanItemResult
	{
		public string StatusPerkawinan { get; set; }
	}

	private class PekerjaanItemResult
	{
		public string Pekerjaan { get; set; }
	}

	private class CollectionReadItemResult
	{
		public string LoanDate { get; set; }

		public string LoanDateTime { get; set; }

		public string NomorBarcode { get; set; }

		public string Title { get; set; }

		public string Author { get; set; }
	}

	private static string KategoriKoleksi {
        get
        {
            return ConfigurationManager.AppSettings["KategoriKoleksi"].ToString();

        }

    }

	public static string GetKabupatenList()
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<KabupatenItemResult> r = new List<KabupatenItemResult>();
		Connection.SetConnection();
		DataTable dt = Command.ExecDataAdapter("SELECT NamaPropinsi,NamaKab FROM kabupaten,propinsi WHERE kabupaten.PropinsiID=propinsi.ID");
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			KabupatenItemResult ResultItem = new KabupatenItemResult();
			ResultItem.NamaPropinsi = dt.Rows[i]["NamaPropinsi"].ToString();
			ResultItem.NamaKabupaten = dt.Rows[i]["NamaKab"].ToString();
			r.Add(ResultItem);
		}
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static string GetJenisIdentitasList()
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<JenisIdentitasItemResult> r = new List<JenisIdentitasItemResult>();
		Connection.SetConnection();
		JenisIdentitasItemResult ResultItem = new JenisIdentitasItemResult();
		ResultItem.JenisIdentitas = "KTP";
		r.Add(ResultItem);
		ResultItem = new JenisIdentitasItemResult();
		ResultItem.JenisIdentitas = "SIM";
		r.Add(ResultItem);
		ResultItem = new JenisIdentitasItemResult();
		ResultItem.JenisIdentitas = "KARTU PELAJAR";
		r.Add(ResultItem);
		ResultItem = new JenisIdentitasItemResult();
		ResultItem.JenisIdentitas = "KARTU MAHASISWA";
		r.Add(ResultItem);
		ResultItem = new JenisIdentitasItemResult();
		ResultItem.JenisIdentitas = "PASPOR";
		r.Add(ResultItem);
		ResultItem = new JenisIdentitasItemResult();
		ResultItem.JenisIdentitas = "KK (Kartu Keluarga)";
		r.Add(ResultItem);
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static string GetJenisAnggotaList()
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<JenisAnggotaItemResult> r = new List<JenisAnggotaItemResult>();
		Connection.SetConnection();
		JenisAnggotaItemResult ResultItem = new JenisAnggotaItemResult();
		ResultItem.JenisAnggota = "PELAJAR";
		r.Add(ResultItem);
		ResultItem = new JenisAnggotaItemResult();
		ResultItem.JenisAnggota = "MAHASISWA";
		r.Add(ResultItem);
		ResultItem = new JenisAnggotaItemResult();
		ResultItem.JenisAnggota = "UMUM";
		r.Add(ResultItem);
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static string GetPendidikanList()
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<PendidikanItemResult> r = new List<PendidikanItemResult>();
		Connection.SetConnection();
		PendidikanItemResult ResultItem = new PendidikanItemResult();
		ResultItem.Pendidikan = "SD";
		r.Add(ResultItem);
		ResultItem = new PendidikanItemResult();
		ResultItem.Pendidikan = "SMP";
		r.Add(ResultItem);
		ResultItem = new PendidikanItemResult();
		ResultItem.Pendidikan = "SMA";
		r.Add(ResultItem);
		ResultItem = new PendidikanItemResult();
		ResultItem.Pendidikan = "D1";
		r.Add(ResultItem);
		ResultItem = new PendidikanItemResult();
		ResultItem.Pendidikan = "D2";
		r.Add(ResultItem);
		ResultItem = new PendidikanItemResult();
		ResultItem.Pendidikan = "D3";
		r.Add(ResultItem);
		ResultItem = new PendidikanItemResult();
		ResultItem.Pendidikan = "S1";
		r.Add(ResultItem);
		ResultItem = new PendidikanItemResult();
		ResultItem.Pendidikan = "S2";
		r.Add(ResultItem);
		ResultItem = new PendidikanItemResult();
		ResultItem.Pendidikan = "S3";
		r.Add(ResultItem);
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static string GetGenderList()
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<GenderItemResult> r = new List<GenderItemResult>();
		Connection.SetConnection();
		GenderItemResult ResultItem = new GenderItemResult();
		ResultItem.Gender = "Laki-Laki";
		r.Add(ResultItem);
		ResultItem = new GenderItemResult();
		ResultItem.Gender = "Perempuan";
		r.Add(ResultItem);
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static string GetStatusPerkawinanList()
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<StatusPerkawinanItemResult> r = new List<StatusPerkawinanItemResult>();
		Connection.SetConnection();
		StatusPerkawinanItemResult ResultItem = new StatusPerkawinanItemResult();
		ResultItem.StatusPerkawinan = "Belum Menikah";
		r.Add(ResultItem);
		ResultItem = new StatusPerkawinanItemResult();
		ResultItem.StatusPerkawinan = "Menikah";
		r.Add(ResultItem);
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static string GetPekerjaanList()
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<PekerjaanItemResult> r = new List<PekerjaanItemResult>();
		Connection.SetConnection();
		PekerjaanItemResult ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "Pegawai Negeri";
		r.Add(ResultItem);
		ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "Peneliti";
		r.Add(ResultItem);
		ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "TNI/POLRI";
		r.Add(ResultItem);
		ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "Pegawai Swasta";
		r.Add(ResultItem);
		ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "Dosen";
		r.Add(ResultItem);
		ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "Pensiunan";
		r.Add(ResultItem);
		ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "Wiraswasta";
		r.Add(ResultItem);
		ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "Guru";
		r.Add(ResultItem);
		ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "Pelajar";
		r.Add(ResultItem);
		ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "Mahasiswa";
		r.Add(ResultItem);
		ResultItem = new PekerjaanItemResult();
		ResultItem.Pekerjaan = "Lainnya";
		r.Add(ResultItem);
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static string GetMemberList(float InitialID, int Limit = 1000)
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<MemberItemResult> r = new List<MemberItemResult>();
		Connection.SetConnection();
		DataTable dt = Command.ExecDataAdapter("SELECT * FROM MEMBERS WHERE ID > " + InitialID + " AND ROWNUM <= " + Limit + " ORDER BY ID");
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			MemberItemResult ResultItem = new MemberItemResult();
			ResultItem.MemberID = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
			ResultItem.MemberNo = dt.Rows[i]["MemberNo"].ToString();
			ResultItem.Fullname = dt.Rows[i]["Fullname"].ToString();
			ResultItem.PlaceOfBirth = dt.Rows[i]["PlaceOfBirth"].ToString();
			if (!string.IsNullOrEmpty(dt.Rows[i]["DateOfBirth"].ToString()))
			{
				ResultItem.DateOfBirth = Convert.ToDateTime(dt.Rows[i]["DateOfBirth"].ToString());
			}
			ResultItem.Address = dt.Rows[i]["Address"].ToString();
			ResultItem.City = dt.Rows[i]["City"].ToString();
			ResultItem.Province = dt.Rows[i]["Province"].ToString();
			ResultItem.AddressNow = dt.Rows[i]["AddressNow"].ToString();
			ResultItem.CityNow = dt.Rows[i]["CityNow"].ToString();
			ResultItem.ProvinceNow = dt.Rows[i]["ProvinceNow"].ToString();
			ResultItem.Phone = dt.Rows[i]["Phone"].ToString();
			ResultItem.InstitutionName = dt.Rows[i]["InstitutionName"].ToString();
			ResultItem.InstitutionAddress = dt.Rows[i]["InstitutionAddress"].ToString();
			ResultItem.InstitutionPhone = dt.Rows[i]["InstitutionPhone"].ToString();
			ResultItem.IdentityType = dt.Rows[i]["IdentityType"].ToString();
			ResultItem.IdentityNo = dt.Rows[i]["IdentityNo"].ToString();
			ResultItem.EducationLevel = dt.Rows[i]["EducationLevel"].ToString();
			ResultItem.Religion = dt.Rows[i]["Religion"].ToString();
			ResultItem.Sex = dt.Rows[i]["Sex"].ToString();
			ResultItem.MaritalStatus = dt.Rows[i]["MaritalStatus"].ToString();
			ResultItem.JobName = dt.Rows[i]["JobName"].ToString();
			if (!string.IsNullOrEmpty(dt.Rows[i]["RegisterDate"].ToString()))
			{
				ResultItem.RegisterDate = Convert.ToDateTime(dt.Rows[i]["RegisterDate"].ToString());
			}
			if (!string.IsNullOrEmpty(dt.Rows[i]["EndDate"].ToString()))
			{
				ResultItem.EndDate = Convert.ToDateTime(dt.Rows[i]["EndDate"].ToString());
			}
			ResultItem.MotherMaidenName = dt.Rows[i]["MotherMaidenName"].ToString();
			ResultItem.Email = dt.Rows[i]["Email"].ToString();
			ResultItem.JenisPermohonan = dt.Rows[i]["JenisPermohonan"].ToString();
			ResultItem.AlamatDomisili = dt.Rows[i]["AlamatDomisili"].ToString();
			ResultItem.NoHp = dt.Rows[i]["NoHp"].ToString();
			ResultItem.JobNameDetail = dt.Rows[i]["JobNameDetail"].ToString();
			ResultItem.JenisPermohonanName = dt.Rows[i]["JenisPermohonanName"].ToString();
			ResultItem.JenisAnggota = dt.Rows[i]["JenisAnggota"].ToString();
			ResultItem.JenisAnggotaName = dt.Rows[i]["JenisAnggotaName"].ToString();
			ResultItem.StatusAnggota = dt.Rows[i]["StatusAnggota"].ToString();
			ResultItem.StatusAnggotaName = dt.Rows[i]["StatusAnggotaName"].ToString();
			r.Add(ResultItem);
		}
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static string GetMemberData(string MemberNo)
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<MemberItemResult> r = new List<MemberItemResult>();
		Connection.SetConnection();
		TwoArrayList tar = new TwoArrayList();
		tar.Add("MemberNo", MemberNo);
		DataTable dt = Command.ExecDataAdapter("SELECT * FROM MEMBERS WHERE MemberNo = " + Connection.ParameterSymbol + "MemberNo", tar);
		if (dt.Rows.Count > 0)
		{
			MemberItemResult ResultItem = new MemberItemResult();
			ResultItem.MemberID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
			ResultItem.MemberNo = dt.Rows[0]["MemberNo"].ToString();
			ResultItem.Fullname = dt.Rows[0]["Fullname"].ToString();
			ResultItem.PlaceOfBirth = dt.Rows[0]["PlaceOfBirth"].ToString();
			if (!string.IsNullOrEmpty(dt.Rows[0]["DateOfBirth"].ToString()))
			{
				ResultItem.DateOfBirth = Convert.ToDateTime(dt.Rows[0]["DateOfBirth"].ToString());
			}
			ResultItem.Address = dt.Rows[0]["Address"].ToString();
			ResultItem.City = dt.Rows[0]["City"].ToString();
			ResultItem.Province = dt.Rows[0]["Province"].ToString();
			ResultItem.AddressNow = dt.Rows[0]["AddressNow"].ToString();
			ResultItem.CityNow = dt.Rows[0]["CityNow"].ToString();
			ResultItem.ProvinceNow = dt.Rows[0]["ProvinceNow"].ToString();
			ResultItem.Phone = dt.Rows[0]["Phone"].ToString();
			ResultItem.InstitutionName = dt.Rows[0]["InstitutionName"].ToString();
			ResultItem.InstitutionAddress = dt.Rows[0]["InstitutionAddress"].ToString();
			ResultItem.InstitutionPhone = dt.Rows[0]["InstitutionPhone"].ToString();
			ResultItem.IdentityType = dt.Rows[0]["IdentityType"].ToString();
			ResultItem.IdentityNo = dt.Rows[0]["IdentityNo"].ToString();
			ResultItem.EducationLevel = dt.Rows[0]["EducationLevel"].ToString();
			ResultItem.Religion = dt.Rows[0]["Religion"].ToString();
			ResultItem.Sex = dt.Rows[0]["Sex"].ToString();
			ResultItem.MaritalStatus = dt.Rows[0]["MaritalStatus"].ToString();
			ResultItem.JobName = dt.Rows[0]["JobName"].ToString();
			if (!string.IsNullOrEmpty(dt.Rows[0]["RegisterDate"].ToString()))
			{
				ResultItem.RegisterDate = Convert.ToDateTime(dt.Rows[0]["RegisterDate"].ToString());
			}
			if (!string.IsNullOrEmpty(dt.Rows[0]["EndDate"].ToString()))
			{
				ResultItem.EndDate = Convert.ToDateTime(dt.Rows[0]["EndDate"].ToString());
			}
			ResultItem.MotherMaidenName = dt.Rows[0]["MotherMaidenName"].ToString();
			ResultItem.Email = dt.Rows[0]["Email"].ToString();
			ResultItem.JenisPermohonan = dt.Rows[0]["JenisPermohonan"].ToString();
			ResultItem.AlamatDomisili = dt.Rows[0]["AlamatDomisili"].ToString();
			ResultItem.NoHp = dt.Rows[0]["NoHp"].ToString();
			ResultItem.JobNameDetail = dt.Rows[0]["JobNameDetail"].ToString();
			ResultItem.JenisPermohonanName = dt.Rows[0]["JenisPermohonanName"].ToString();
			ResultItem.JenisAnggota = dt.Rows[0]["JenisAnggota"].ToString();
			ResultItem.JenisAnggotaName = dt.Rows[0]["JenisAnggotaName"].ToString();
			ResultItem.StatusAnggota = dt.Rows[0]["StatusAnggota"].ToString();
			ResultItem.StatusAnggotaName = dt.Rows[0]["StatusAnggotaName"].ToString();
			r.Add(ResultItem);
		}
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static bool IsMemberExist(string MemberNo)
	{
		Connection.SetConnection();
		TwoArrayList tar = new TwoArrayList();
		tar.Add("MemberNo", MemberNo);
		return Util.ConvertToBoolean(Command.ExecScalar(tar, "SELECT COUNT(*) FROM MEMBERS WHERE MemberNo = " + Connection.ParameterSymbol + "MemberNo", "0"));
	}

	public static string GetMemberOnlineList(float InitialID, int Limit = 1000)
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<MemberOnlineItemResult> r = new List<MemberOnlineItemResult>();
		Connection.SetConnection();
		DataTable dt = Command.ExecDataAdapter("SELECT * FROM MEMBERSONLINE WHERE ID > " + InitialID + " AND ROWNUM <= " + Limit + " ORDER BY ID");
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			MemberOnlineItemResult ResultItem = new MemberOnlineItemResult();
			ResultItem.MemberOnlineID = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
			ResultItem.NoAnggota = dt.Rows[i]["NoAnggota"].ToString();
			ResultItem.NickName = dt.Rows[i]["NickName"].ToString();
			ResultItem.Password = dt.Rows[i]["Password"].ToString();
			ResultItem.Email = dt.Rows[i]["Email"].ToString();
			ResultItem.Status = dt.Rows[i]["Status"].ToString();
			ResultItem.Activation_Code = dt.Rows[i]["Activation_Code"].ToString();
			r.Add(ResultItem);
		}
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static string GetMemberOnlineData(string NoAnggota)
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<MemberOnlineItemResult> r = new List<MemberOnlineItemResult>();
		Connection.SetConnection();
		TwoArrayList tar = new TwoArrayList();
		tar.Add("NoAnggota", NoAnggota);
		DataTable dt = Command.ExecDataAdapter("SELECT * FROM MEMBERSONLINE WHERE NoAnggota = " + Connection.ParameterSymbol + "NoAnggota", tar);
		if (dt.Rows.Count > 0)
		{
			MemberOnlineItemResult ResultItem = new MemberOnlineItemResult();
			ResultItem.MemberOnlineID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
			ResultItem.NoAnggota = dt.Rows[0]["NoAnggota"].ToString();
			ResultItem.NickName = dt.Rows[0]["NickName"].ToString();
			ResultItem.Password = dt.Rows[0]["Password"].ToString();
			ResultItem.Email = dt.Rows[0]["Email"].ToString();
			ResultItem.Status = dt.Rows[0]["Status"].ToString();
			ResultItem.Activation_Code = dt.Rows[0]["Activation_Code"].ToString();
			r.Add(ResultItem);
		}
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static bool IsMemberOnlineExist(string NoAnggota)
	{
		Connection.SetConnection();
		TwoArrayList tar = new TwoArrayList();
		tar.Add("NoAnggota", NoAnggota);
		return Util.ConvertToBoolean(Command.ExecScalar(tar, "SELECT COUNT(*) FROM MEMBERSONLINE WHERE NoAnggota = " + Connection.ParameterSymbol + "NoAnggota", "0"));
	}

	public static bool IsDataExistByNomorIdentitas(string identityno)
	{
		TwoArrayList tar = new TwoArrayList();
		tar.Add("IdentityNo", identityno);
		string SQL = "SELECT 1 FROM members WHERE IdentityNo=" + Connection.ParameterSymbol + "IdentityNo";
		string Result = Command.ExecScalar(tar, SQL, "");
		return !string.IsNullOrEmpty(Result);
	}

	public static bool IsDataExistByMemberNo(string MemberNo)
	{
		TwoArrayList tar = new TwoArrayList();
		tar.Add("MemberNo", MemberNo);
		string SQL = "SELECT 1 FROM members WHERE MemberNo=" + Connection.ParameterSymbol + "MemberNo";
		string Result = Command.ExecScalar(tar, SQL, "");
		return !string.IsNullOrEmpty(Result);
	}

	public static string GetMemberDataByIdentity(string fullname, string placebirth, string datebirth, string sex, string identitytype, string identityno)
	{
		Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		Connection.SetConnection();
		string NoAnggota = "";
		DateTime dtbirth = Convert.ToDateTime(datebirth);
		string StrDtBirth = "";
		if (Connection.ServerType == Connection.EServerType.Oracle)
		{
			StrDtBirth = String.Format("{0:dd-MMM-yy}",dtbirth);
		}
		else if (Connection.ServerType == Connection.EServerType.MySQL)
		{
			StrDtBirth = String.Format("{0:yyyy-MM-dd}",dtbirth);
		}
		TwoArrayList tar = new TwoArrayList();
		tar.Add("Fullname1", fullname.ToUpper());
		tar.Add("PlaceOfBirth1", placebirth.ToUpper());
		tar.Add("Fullname2", fullname.ToUpper());
		tar.Add("PlaceOfBirth2", placebirth.ToUpper());
		tar.Add("IdentityNo", identityno);
		string SQL = "SELECT MemberNo,FullName,PlaceOfBirth,DateOfBirth,Sex,IdentityType,IdentityNo FROM members WHERE (UPPER(Fullname)=" + Connection.ParameterSymbol + "Fullname1 AND UPPER(PlaceOfBirth)=" + Connection.ParameterSymbol + "PlaceOfBirth1 AND DateOfBirth='" + StrDtBirth + "' AND UPPER(Sex)='" + sex.ToUpper() + "') OR (UPPER(Fullname)=" + Connection.ParameterSymbol + "Fullname2 AND UPPER(PlaceOfBirth)=" + Connection.ParameterSymbol + "PlaceOfBirth2 AND DateOfBirth='" + StrDtBirth + "' AND UPPER(Sex)='" + sex.ToUpper() + "' AND UPPER(IdentityType)='" + identitytype.ToUpper() + "' AND IdentityNo=" + Connection.ParameterSymbol + "IdentityNo)";
		DataTable dt = Command.ExecDataAdapter(SQL, tar);
		int Count = dt.Rows.Count;
		if (Count > 0)
		{
			NoAnggota = dt.Rows[0]["MemberNo"].ToString();
		}
		return NoAnggota;
	}

	public static string RegisterMember(Page Page, string BranchId, string CreateBy, string NamaAnggota, string EmailAddress, string JenisIdentitas, string NomorIdentitas, string TempatLahir, string TanggalLahir, string AlamatKTP, string AlamatSekarang, string HomePhoneNumber, string InstitutionName, string InstitutionAddress, string InstitutionPhoneNumber, string HandPhone, string IbuKandung, string JenisKelamin, string ProvinsiKTP, string KabupatenKTP, string KecamatanKTP, string KelurahanKTP, string RWKTP, string RTKTP, string ProvinsiSekarang, string KabupatenSekarang, string KecamatanSekarang, string KelurahanSekarang, string RWSekarang, string RTSekarang, string Pendidikan, string StatusPerkawinan, string Pekerjaan, string PekerjaanLainnya, string JenisAnggota)
	{
		Connection.SetConnection();
		if (NamaAnggota.Trim().Length == 0)
		{
			return "Error : Nama harus diisi!";
		}
		if (JenisIdentitas.Trim().Length == 0)
		{
			return "Error : Jenis identitas harus diisi!";
		}
		if (NomorIdentitas.Trim().Length == 0)
		{
			return "Error : Nomor identitas harus diisi!";
		}
		if (NomorIdentitas.Trim().Length != 16)
		{
			return "Error : Nomor identitas harus 16 digit!";
		}
		if (!string.IsNullOrEmpty(TanggalLahir.Trim()))
		{
			try
			{
				DateTime TglLahir = Convert.ToDateTime(TanggalLahir);
			}
			catch
			{
				return "Error : Format Tanggal Lahir salah";
			}
		}
		if (JenisKelamin.Trim().Length == 0)
		{
			return "Error : Jenis Kelamin harus diisi!";
		}
		if (!(JenisKelamin.ToLower() == "laki-laki") && !(JenisKelamin.ToLower() == "perempuan"))
		{
			return "Error : Jenis Kelamin harus Laki-Laki / Perempuan!";
		}
		if (JenisKelamin.ToLower() == "laki-laki")
		{
			JenisKelamin = "Laki-Laki";
		}
		else if (JenisKelamin.ToLower() == "perempuan")
		{
			JenisKelamin = "Perempuan";
		}
		bool IsMemberExistByNomorIdentitas = IsDataExistByNomorIdentitas(NomorIdentitas);
		bool IsMemberExistByMemberNo = IsDataExistByMemberNo(NomorIdentitas);
		if (IsMemberExistByNomorIdentitas || IsMemberExistByMemberNo)
		{
			return UpdateMember(Page, BranchId, CreateBy, NomorIdentitas, NamaAnggota, TempatLahir, TanggalLahir, AlamatKTP, AlamatSekarang, ProvinsiKTP, KabupatenKTP, KecamatanKTP, KelurahanKTP, RWKTP, RTKTP, ProvinsiSekarang, KabupatenSekarang, KecamatanSekarang, KelurahanSekarang, RWSekarang, RTSekarang, HomePhoneNumber, InstitutionName, InstitutionAddress, InstitutionPhoneNumber, HandPhone, JenisIdentitas, NomorIdentitas, Pendidikan, JenisKelamin, Pekerjaan, PekerjaanLainnya, IbuKandung, EmailAddress, StatusPerkawinan);
		}
		return AddMember(Page, BranchId, CreateBy, NamaAnggota, EmailAddress, JenisIdentitas, NomorIdentitas, TempatLahir, TanggalLahir, AlamatKTP, AlamatSekarang, HomePhoneNumber, InstitutionName, InstitutionAddress, InstitutionPhoneNumber, HandPhone, IbuKandung, JenisKelamin, ProvinsiKTP, KabupatenKTP, KecamatanKTP, KelurahanKTP, RWKTP, RTKTP, ProvinsiSekarang, KabupatenSekarang, KecamatanSekarang, KelurahanSekarang, RWSekarang, RTSekarang, Pendidikan, StatusPerkawinan, Pekerjaan, PekerjaanLainnya, JenisAnggota);
	}

	private static string AddMember(Page Page, string BranchId, string CreateBy, string NamaAnggota, string EmailAddress, string JenisIdentitas, string NomorIdentitas, string TempatLahir, string TanggalLahir, string AlamatKTP, string AlamatSekarang, string HomePhoneNumber, string InstitutionName, string InstitutionAddress, string InstitutionPhoneNumber, string HandPhone, string IbuKandung, string JenisKelamin, string ProvinsiKTP, string KabupatenKTP, string KecamatanKTP, string KelurahanKTP, string RWKTP, string RTKTP, string ProvinsiSekarang, string KabupatenSekarang, string KecamatanSekarang, string KelurahanSekarang, string RWSekarang, string RTSekarang, string Pendidikan, string StatusPerkawinan, string Pekerjaan, string PekerjaanLainnya, string JenisAnggota)
	{
		string Password = "member123";
		TwoArrayList tar = new TwoArrayList();
		try
		{
			string activate = Page.Session.SessionID.ToString();
			string nickname = Member.NickGenerator(NamaAnggota, 3);
			string Terminal = Util.GetUserHost(Page);
			DateTime Now = DateTime.Now;
			tar.Clear();
			tar.Add("NoAnggota", NomorIdentitas);
			tar.Add("NickName", nickname);
			tar.Add("Password", HashCode(Password));
			tar.Add("Email", EmailAddress.ToLower());
			tar.Add("BranchId", int.Parse(BranchId));
			tar.Add("Status", "ACTIVE");
			tar.Add("Activation_Code", activate);
			tar.Add("CreateBy", CreateBy);
			tar.Add("CreateDate", Now);
			tar.Add("CreateTerminal", Terminal);
			tar.Add("UpdateBy", CreateBy);
			tar.Add("UpdateDate", Now);
			tar.Add("UpdateTerminal", Terminal);
			if (Command.ExecInsertOrUpdate("MEMBERSONLINE", tar, Command.InsertOrUpdate.Insert, null))
			{
				return SaveMember(Page, BranchId, CreateBy, NomorIdentitas, NamaAnggota, JenisIdentitas, NomorIdentitas, TempatLahir, TanggalLahir, AlamatKTP, AlamatSekarang, HomePhoneNumber, InstitutionName, InstitutionAddress, InstitutionPhoneNumber, HandPhone, EmailAddress, IbuKandung, JenisKelamin, ProvinsiKTP, KabupatenKTP, KecamatanKTP, KelurahanKTP, RWKTP, RTKTP, ProvinsiSekarang, KabupatenSekarang, KecamatanSekarang, KelurahanSekarang, RWSekarang, RTSekarang, Pendidikan, StatusPerkawinan, Pekerjaan, PekerjaanLainnya, JenisAnggota);
			}
			return "Error : Register MemberOnline Error! Data Anggota " + NomorIdentitas + "\r\n" + Page.Session[MySession.CurrentErrorMessage].ToString();
		}
		catch (Exception ex)
		{
			return "Error : " + ex.Message;
		}
	}

	private static string HashCode(string str)
	{
		return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.Default.GetBytes(str))).Replace("-", "");
	}

	private static int GetMemberOnlineID(string NoAnggota)
	{
		Connection.SetConnection();
		TwoArrayList tar = new TwoArrayList();
		tar.Add("NoAnggota", NoAnggota);
		string SQL = "SELECT ID FROM membersonline WHERE NoAnggota=" + Connection.ParameterSymbol + "NoAnggota";
		DataTable dt = Command.ExecDataAdapter(SQL, tar);
		int id;
		return (dt.Rows.Count > 0) ? (id = Convert.ToInt32(dt.Rows[0]["ID"].ToString())) : 0;
	}

	private static int GetUserIDMembers(string NoAnggota)
	{
		Connection.SetConnection();
		TwoArrayList tar = new TwoArrayList();
		tar.Add("NoAnggota", NoAnggota);
		string SQL = "SELECT ID FROM members WHERE memberno=" + Connection.ParameterSymbol + "NoAnggota";
		DataTable dt = Command.ExecDataAdapter(SQL, tar);
		int id;
		return (dt.Rows.Count > 0) ? (id = Convert.ToInt32(dt.Rows[0]["ID"].ToString())) : 0;
	}

	private static string SaveMember(Page Page, string BranchId, string CreateBy, string MemberNo, string NamaAnggota, string JenisIdentitas, string NomorIdentitas, string TempatLahir, string TanggalLahir, string AlamatKTP, string AlamatSekarang, string HomePhoneNumber, string InstitutionName, string InstitutionAddress, string InstitutionPhoneNumber, string HandPhone, string EmailAddress, string IbuKandung, string JenisKelamin, string ProvinsiKTP, string KabupatenKTP, string KecamatanKTP, string KelurahanKTP, string RWKTP, string RTKTP, string ProvinsiSekarang, string KabupatenSekarang, string KecamatanSekarang, string KelurahanSekarang, string RWSekarang, string RTSekarang, string Pendidikan, string StatusPerkawinan, string Pekerjaan, string PekerjaanLainnya, string JenisAnggota)
	{
		DateTime dateOfBirth = default(DateTime);
		if (!string.IsNullOrEmpty(TanggalLahir))
		{
			dateOfBirth = Convert.ToDateTime(TanggalLahir);
		}
		int MemberOnlineID = GetMemberOnlineID(MemberNo);
		string Terminal = Util.GetUserHost(Page);
		DateTime Now = DateTime.Now;
		TwoArrayList tar = new TwoArrayList();
		tar.Add("MemberNo", MemberNo);
		tar.Add("Fullname", NamaAnggota.ToUpper());
		tar.Add("PlaceOfBirth", Util.UpperFirst(TempatLahir));
		if (!string.IsNullOrEmpty(TanggalLahir.Trim()))
		{
			tar.Add("DateOfBirth", dateOfBirth);
		}
		else
		{
			tar.Add("DateOfBirth", DBNull.Value);
		}
		tar.Add("Address", Util.UpperFirst(AlamatKTP));
		tar.Add("AddressNow", Util.UpperFirst(AlamatSekarang));
		tar.Add("Province", Util.UpperFirst(ProvinsiKTP));
		tar.Add("City", Util.UpperFirst(KabupatenKTP));
		tar.Add("Kecamatan", Util.UpperFirst(KecamatanKTP));
		tar.Add("Kelurahan", Util.UpperFirst(KelurahanKTP));
		tar.Add("RW", Util.UpperFirst(RWKTP));
		tar.Add("RT", Util.UpperFirst(RTKTP));
		tar.Add("ProvinceNow", Util.UpperFirst(ProvinsiSekarang));
		tar.Add("CityNow", Util.UpperFirst(KabupatenSekarang));
		tar.Add("KecamatanNow", Util.UpperFirst(KecamatanSekarang));
		tar.Add("KelurahanNow", Util.UpperFirst(KelurahanSekarang));
		tar.Add("RWNow", Util.UpperFirst(RWSekarang));
		tar.Add("RTNow", Util.UpperFirst(RTSekarang));
		tar.Add("Phone", HomePhoneNumber);
		tar.Add("InstitutionName", Util.UpperFirst(InstitutionName));
		tar.Add("InstitutionAddress", Util.UpperFirst(InstitutionAddress));
		tar.Add("InstitutionPhone", InstitutionPhoneNumber);
		tar.Add("IdentityType", "KTP / NIK");
		tar.Add("IdentityNo", NomorIdentitas);
		tar.Add("EducationLevel", Pendidikan);
		tar.Add("Sex", JenisKelamin);
		tar.Add("MaritalStatus", StatusPerkawinan);
		tar.Add("JobName", Pekerjaan);
		tar.Add("RegisterDate", DateTime.Now);
		tar.Add("EndDate", DateTime.Now.AddYears(100));
		tar.Add("MotherMaidenName", Util.UpperFirst(IbuKandung));
		tar.Add("Email", EmailAddress.ToLower());
		tar.Add("JenisPermohonan", "Permohonan");
		tar.Add("JenisPermohonanName", "Permohonan");
		tar.Add("AlamatDomisili", Util.UpperFirst(AlamatSekarang));
		tar.Add("NoHp", HandPhone);
		tar.Add("JobNameDetail", Util.UpperFirst(PekerjaanLainnya));
		tar.Add("JenisAnggota", JenisAnggota.ToUpper());
		tar.Add("JenisAnggotaName", JenisAnggota.ToUpper());
		tar.Add("StatusAnggota", "ACTIVE");
		tar.Add("StatusAnggotaName", "ACTIVE");
		tar.Add("Branch_Id", int.Parse(BranchId));
		tar.Add("User_id", MemberOnlineID);
		tar.Add("CREATEBY", CreateBy);
		tar.Add("CREATEDATE", Now);
		tar.Add("CREATETERMINAL", Terminal);
		tar.Add("UPDATEBY", CreateBy);
		tar.Add("UPDATEDATE", Now);
		tar.Add("UPDATETERMINAL", CreateBy);
		if (Command.ExecInsertOrUpdate("MEMBERS", tar, Command.InsertOrUpdate.Insert, null))
		{
			int Member_Id = GetUserIDMembers(MemberNo);
			TwoArrayList tarLoanCategory = new TwoArrayList();
			DataTable dtLoanCategory = Command.ExecDataAdapter("SELECT Id,Name FROM COLLECTIONCATEGORYS");
			for (int i = 0; i < dtLoanCategory.Rows.Count; i++)
			{
				int CategoryLoan_id = int.Parse(dtLoanCategory.Rows[i]["id"].ToString());
				tarLoanCategory.Clear();
				tarLoanCategory.Add("Member_Id", Member_Id);
				tarLoanCategory.Add("RegisterBy", CreateBy);
				tarLoanCategory.Add("RegisterDate", DateTime.Now);
				tarLoanCategory.Add("RegisterTerminal", Terminal);
				tarLoanCategory.Add("CategoryLoan_id", CategoryLoan_id);
				if (Command.ExecInsertOrUpdate("memberloanauthorizecategory", tarLoanCategory, Command.InsertOrUpdate.Insert, null))
				{
				}
			}
			TwoArrayList tarLoanLocation = new TwoArrayList();
			DataTable dtLoanLocation = Command.ExecDataAdapter("SELECT Id,Name FROM LOCATIONLOAN");
			for (int i = 0; i < dtLoanLocation.Rows.Count; i++)
			{
				int LocationLoan_id = int.Parse(dtLoanLocation.Rows[i]["id"].ToString());
				tarLoanLocation.Clear();
				tarLoanLocation.Add("Member_Id", Member_Id);
				tarLoanLocation.Add("RegisterBy", CreateBy);
				tarLoanLocation.Add("RegisterDate", DateTime.Now);
				tarLoanLocation.Add("RegisterTerminal", Terminal);
				tarLoanLocation.Add("LocationLoan_id", LocationLoan_id);
				if (Command.ExecInsertOrUpdate("memberloanauthorizelocation", tarLoanLocation, Command.InsertOrUpdate.Insert, null))
				{
				}
			}
			tar.Clear();
			tar.Add("MEMBER_ID", Member_Id);
			tar.Add("APPROVALLETTERNO", "-");
			tar.Add("APPROVALLETTERFILE", "Approval_For_UPT.pdf");
			tar.Add("ISACTIVE", 1);
			tar.Add("CREATEBY", "(Silang Layan Web Service)");
			tar.Add("CREATEDATE", DateTime.Now);
			tar.Add("CREATETERMINAL", Terminal);
			Command.ExecInsertOrUpdate("MEMBER_APPROVALS", tar, Command.InsertOrUpdate.Insert);
			return "Pendaftaran Berhasil dengan Nomor Anggota : " + MemberNo;
		}
		return "Error : Register Member Error! Data Anggota " + MemberNo + "\r\n" + Page.Session[MySession.CurrentErrorMessage].ToString();
	}

	public static string IsLoginValid(string NoAnggota, string Password)
	{
		try
		{
			if (NoAnggota.Trim().Length == 0)
			{
				return "Error : Nomor Anggota tidak boleh kosong! ";
			}
			if (Password.Trim().Length == 0)
			{
				return "Error : Password tidak boleh kosong!";
			}
			Connection.SetConnection();
			TwoArrayList tar = new TwoArrayList();
			tar.Add("NoAnggota", NoAnggota);
			string SQL = "SELECT ID,NoAnggota,NickName,Password,Status FROM membersonline WHERE NoAnggota=" + Connection.ParameterSymbol + "NoAnggota";
			DataTable dt = Command.ExecDataAdapter(SQL, tar);
			int Count = dt.Rows.Count;
			if (Count < 1)
			{
				return "Error : Maaf, Nomor Anggota anda belum terdaftar. Silahkan hubungi bagian layanan Perpustakaan Nasional Republik Indonesia jika anda telah mendaftarkan data diri anda sebelumnya.";
			}
			string HashPassword = dt.Rows[0]["Password"].ToString();
			string Status = dt.Rows[0]["Status"].ToString();
			if (HashPassword != HashCode(Password))
			{
				return "Error : Maaf, Password yang dimasukkan tidak sesuai.";
			}
			if (Status != "ACTIVE")
			{
				return "Error : Maaf, Silahkan aktifkan akun anda terlebih dahulu.";
			}
			return "Login berhasil";
		}
		catch (Exception ex)
		{
			return "Error : " + ex.Message;
		}
	}

	public static string ForgotPassword(Page Page, string MemberNo, string Email)
	{
		try
		{
			MemberNo = MemberNo.Trim();
			Email = Email.Trim();
			if (MemberNo.Length == 0)
			{
				return "Error : Nomor Anggota tidak boleh kosong! ";
			}
			if (Email.Length == 0)
			{
				return "Error : Email tidak boleh kosong!";
			}
			string newPassEmail = Page.Session.SessionID.ToString().Substring(0, 6);
			string newPassDB = HashCode(newPassEmail);
			Connection.SetConnection();
			TwoArrayList tarWhere = new TwoArrayList();
			tarWhere.Add("NoAnggota", MemberNo);
			tarWhere.Add("Email", Email);
			string SQL2 = "SELECT membersonline.NoAnggota, members.Email FROM membersonline INNER JOIN members ON (membersonline.NoAnggota = members.MemberNo) WHERE membersonline.NoAnggota = " + Connection.ParameterSymbol + "NoAnggota AND members.Email = " + Connection.ParameterSymbol + "Email";
			DataTable dt = Command.ExecDataAdapter(SQL2, tarWhere);
			if (dt.Rows.Count > 0)
			{
				tarWhere.Clear();
				tarWhere.Add("MemberNo", MemberNo);
				string NamaAnggota = Command.ExecScalar(tarWhere, "SELECT FullName FROM MEMBERS WHERE MemberNo = " + Connection.ParameterSymbol + "MemberNo");
				tarWhere.Clear();
				tarWhere.Add("NoAnggota", MemberNo);
				TwoArrayList tar = new TwoArrayList();
				tar.Add("Password", newPassDB);
				if (Command.ExecInsertOrUpdate("MEMBERSONLINE", tar, Command.InsertOrUpdate.Update, " WHERE NoAnggota=" + Connection.ParameterSymbol + "NoAnggota", tarWhere))
				{
					Mailthis.SendPassword(MemberNo, Email, NamaAnggota, newPassEmail);
					return "Reset Password berhasil. Password baru anda telah dikirimkan ke email Anggota.";
				}
				return "Error : " + Page.Session[MySession.CurrentErrorMessage].ToString();
			}
			return "Error : Maaf, Nomor anggota / alamat email anda belum terdaftar di database kami. Silahkan hubungi bagian layanan " + Util.NmPerpus() + ".";
		}
		catch (Exception ex)
		{
			return "Error : " + ex.Message;
		}
	}

	public static string UpdateMember(Page Page, string BranchId, string CreateBy, string MemberNo, string NamaAnggota, string TempatLahir, string TanggalLahir, string AlamatKTP, string AlamatSekarang, string ProvinsiKTP, string KabupatenKTP, string KecamatanKTP, string KelurahanKTP, string RWKTP, string RTKTP, string KabupatenSekarang, string ProvinsiSekarang, string KecamatanSekarang, string KelurahanSekarang, string RWSekarang, string RTSekarang, string HomePhoneNumber, string InstitutionName, string InstitutionAddress, string InstitutionPhoneNumber, string HandPhone, string JenisIdentitas, string NomorIdentitas, string Pendidikan, string JenisKelamin, string Pekerjaan, string PekerjaanLainnya, string IbuKandung, string EmailAddress, string StatusPerkawinan)
	{
		Connection.SetConnection();
		if (MemberNo.Trim().Length == 0)
		{
			return "Error : Nomor Anggota harus diisi!";
		}
		if (NamaAnggota.Trim().Length == 0)
		{
			return "Error : Nama harus diisi!";
		}
		if (JenisIdentitas.Trim().Length == 0)
		{
			return "Error : Jenis identitas harus diisi!";
		}
		if (NomorIdentitas.Trim().Length == 0)
		{
			return "Error : Nomor identitas harus diisi!";
		}
		if (NomorIdentitas.Trim().Length != 16)
		{
			return "Error : Nomor identitas harus 16 digit!";
		}
		DateTime TglLahir = default(DateTime);
		if (!string.IsNullOrEmpty(TanggalLahir.Trim()))
		{
			try
			{
				TglLahir = Convert.ToDateTime(TanggalLahir);
			}
			catch
			{
				return "Error : Format Tanggal Lahir salah";
			}
		}
		if (JenisKelamin.Trim().Length == 0)
		{
			return "Error : Jenis Kelamin harus diisi!";
		}
		if (!(JenisKelamin.ToLower() == "laki-laki") && !(JenisKelamin.ToLower() == "perempuan"))
		{
			return "Error : Jenis Kelamin harus Laki-Laki / Perempuan!";
		}
		if (JenisKelamin.ToLower() == "laki-laki")
		{
			JenisKelamin = "Laki-Laki";
		}
		else if (JenisKelamin.ToLower() == "perempuan")
		{
			JenisKelamin = "Perempuan";
		}
		string Terminal = Util.GetUserHost(Page);
		string UpdateBy = CreateBy + ". MemberNo : " + MemberNo;
		TwoArrayList tarWhere = new TwoArrayList();
		tarWhere.Add("IdentityNo", NomorIdentitas);
		DataTable dtMember = Command.ExecDataAdapter("SELECT * FROM MEMBERS WHERE IdentityNo = " + Connection.ParameterSymbol + "IdentityNo", tarWhere);
		if (dtMember.Rows.Count == 0)
		{
			return "Error : Tidak dapat menemukan Anggota dengan nomor NIK " + NomorIdentitas;
		}
		TwoArrayList tar = new TwoArrayList();
		string Member_Id = dtMember.Rows[0]["ID"].ToString();
		string MemberApprovalId = Command.ExecScalar("SELECT Id FROM MEMBER_APPROVALS WHERE MEMBER_ID = " + Member_Id);
		if (string.IsNullOrEmpty(MemberApprovalId))
		{
			tar.Clear();
			tar.Add("MEMBER_ID", Member_Id);
			tar.Add("APPROVALLETTERNO", "-");
			tar.Add("APPROVALLETTERFILE", "Approval_For_UPT.pdf");
			tar.Add("ISACTIVE", 1);
			tar.Add("CREATEBY", "(Silang Layan Web Service)");
			tar.Add("CREATEDATE", DateTime.Now);
			tar.Add("CREATETERMINAL", Terminal);
			Command.ExecInsertOrUpdate("MEMBER_APPROVALS", tar, Command.InsertOrUpdate.Insert);
		}
		tar.Clear();
		if (MemberNo != dtMember.Rows[0]["MemberNo"].ToString())
		{
			tar.Add("MemberNo", MemberNo);
		}
		if (BranchId != dtMember.Rows[0]["BRANCH_ID"].ToString())
		{
			tar.Add("Branch_Id", BranchId);
		}
		if (NamaAnggota.ToUpper() != dtMember.Rows[0]["Fullname"].ToString().ToUpper())
		{
			tar.Add("Fullname", NamaAnggota.ToUpper());
		}
		if (TempatLahir.ToUpper() != dtMember.Rows[0]["PlaceOfBirth"].ToString().ToUpper())
		{
			tar.Add("PlaceOfBirth", Util.UpperFirst(TempatLahir));
		}
		string IsUpdateNeeded = "";
		if (!string.IsNullOrEmpty(TanggalLahir))
		{
			IsUpdateNeeded = DateTime.Parse(TanggalLahir).ToShortDateString();
		}
		string tarLoanCategory = "";
		if (!string.IsNullOrEmpty(dtMember.Rows[0]["DateOfBirth"].ToString()))
		{
			tarLoanCategory = DateTime.Parse(dtMember.Rows[0]["DateOfBirth"].ToString()).ToShortDateString();
		}
		if (IsUpdateNeeded != tarLoanCategory)
		{
			if (!string.IsNullOrEmpty(TanggalLahir.Trim()))
			{
				tar.Add("DateOfBirth", TglLahir);
			}
			else
			{
				tar.Add("DateOfBirth", DBNull.Value);
			}
		}
		if (AlamatKTP.ToUpper() != dtMember.Rows[0]["Address"].ToString().ToUpper())
		{
			tar.Add("Address", Util.UpperFirst(AlamatKTP));
		}
		if (AlamatSekarang.ToUpper() != dtMember.Rows[0]["AddressNow"].ToString().ToUpper())
		{
			tar.Add("AddressNow", Util.UpperFirst(AlamatSekarang));
		}
		if (ProvinsiKTP.ToUpper() != dtMember.Rows[0]["Province"].ToString().ToUpper())
		{
			tar.Add("Province", Util.UpperFirst(ProvinsiKTP));
		}
		if (KabupatenKTP.ToUpper() != dtMember.Rows[0]["City"].ToString().ToUpper())
		{
			tar.Add("City", Util.UpperFirst(KabupatenKTP));
		}
		if (KecamatanKTP.ToUpper() != dtMember.Rows[0]["Kecamatan"].ToString().ToUpper())
		{
			tar.Add("Kecamatan", Util.UpperFirst(KecamatanKTP));
		}
		if (KelurahanKTP.ToUpper() != dtMember.Rows[0]["Kelurahan"].ToString().ToUpper())
		{
			tar.Add("Kelurahan", Util.UpperFirst(KelurahanKTP));
		}
		if (RWKTP.ToUpper() != dtMember.Rows[0]["RW"].ToString().ToUpper())
		{
			tar.Add("RW", Util.UpperFirst(RWKTP));
		}
		if (RTKTP.ToUpper() != dtMember.Rows[0]["RT"].ToString().ToUpper())
		{
			tar.Add("RT", Util.UpperFirst(RTKTP));
		}
		if (ProvinsiSekarang.ToUpper() != dtMember.Rows[0]["ProvinceNow"].ToString().ToUpper())
		{
			tar.Add("ProvinceNow", Util.UpperFirst(ProvinsiSekarang));
		}
		if (KabupatenSekarang.ToUpper() != dtMember.Rows[0]["CityNow"].ToString().ToUpper())
		{
			tar.Add("CityNow", Util.UpperFirst(KabupatenSekarang));
		}
		if (KecamatanSekarang.ToUpper() != dtMember.Rows[0]["KecamatanNow"].ToString().ToUpper())
		{
			tar.Add("KecamatanNow", Util.UpperFirst(KecamatanSekarang));
		}
		if (KelurahanSekarang.ToUpper() != dtMember.Rows[0]["KelurahanNow"].ToString().ToUpper())
		{
			tar.Add("KelurahanNow", Util.UpperFirst(KelurahanSekarang));
		}
		if (RWSekarang.ToUpper() != dtMember.Rows[0]["RWNow"].ToString().ToUpper())
		{
			tar.Add("RWNow", Util.UpperFirst(RWSekarang));
		}
		if (RTSekarang.ToUpper() != dtMember.Rows[0]["RTNow"].ToString().ToUpper())
		{
			tar.Add("RTNow", Util.UpperFirst(RTSekarang));
		}
		if (HomePhoneNumber.ToUpper() != dtMember.Rows[0]["Phone"].ToString().ToUpper())
		{
			tar.Add("Phone", HomePhoneNumber);
		}
		if (InstitutionName.ToUpper() != dtMember.Rows[0]["InstitutionName"].ToString().ToUpper())
		{
			tar.Add("InstitutionName", Util.UpperFirst(InstitutionName));
		}
		if (InstitutionAddress.ToUpper() != dtMember.Rows[0]["InstitutionAddress"].ToString().ToUpper())
		{
			tar.Add("InstitutionAddress", Util.UpperFirst(InstitutionAddress));
		}
		if (InstitutionPhoneNumber.ToUpper() != dtMember.Rows[0]["InstitutionPhone"].ToString().ToUpper())
		{
			tar.Add("InstitutionPhone", InstitutionPhoneNumber);
		}
		if ("KTP / NIK" != dtMember.Rows[0]["IdentityType"].ToString().ToUpper())
		{
			tar.Add("IdentityType", "KTP / NIK");
		}
		if (NomorIdentitas.ToUpper() != dtMember.Rows[0]["IdentityNo"].ToString().ToUpper())
		{
			tar.Add("IdentityNo", NomorIdentitas);
		}
		if (Pendidikan.ToUpper() != dtMember.Rows[0]["EducationLevel"].ToString().ToUpper())
		{
			tar.Add("EducationLevel", Pendidikan);
		}
		if (JenisKelamin.ToUpper() != dtMember.Rows[0]["Sex"].ToString().ToUpper())
		{
			tar.Add("Sex", JenisKelamin);
		}
		if (StatusPerkawinan.ToUpper() != dtMember.Rows[0]["MaritalStatus"].ToString().ToUpper())
		{
			tar.Add("MaritalStatus", StatusPerkawinan);
		}
		if (Pekerjaan.ToUpper() != dtMember.Rows[0]["JobName"].ToString().ToUpper())
		{
			tar.Add("JobName", Pekerjaan);
		}
		if (IbuKandung.ToUpper() != dtMember.Rows[0]["MotherMaidenName"].ToString().ToUpper())
		{
			tar.Add("MotherMaidenName", Util.UpperFirst(IbuKandung));
		}
		if (EmailAddress.ToUpper() != dtMember.Rows[0]["Email"].ToString().ToUpper())
		{
			tar.Add("Email", EmailAddress.ToLower());
		}
		if (AlamatSekarang.ToUpper() != dtMember.Rows[0]["AlamatDomisili"].ToString().ToUpper())
		{
			tar.Add("AlamatDomisili", Util.UpperFirst(AlamatSekarang));
		}
		if (HandPhone.ToUpper() != dtMember.Rows[0]["NoHp"].ToString().ToUpper())
		{
			tar.Add("NoHp", HandPhone);
		}
		if (PekerjaanLainnya.ToUpper() != dtMember.Rows[0]["JobNameDetail"].ToString().ToUpper())
		{
			tar.Add("JobNameDetail", Util.UpperFirst(PekerjaanLainnya));
		}
		bool dtLoanCategory = tar.Count() > 0;
		tar.Add("UpdateBy", UpdateBy);
		tar.Add("UpdateDate", DateTime.Now);
		tar.Add("UpdateTerminal", Terminal);
		if (dtLoanCategory)
		{
			if (Command.ExecInsertOrUpdate("MEMBERS", tar, Command.InsertOrUpdate.Update, " WHERE ID = " + Member_Id))
			{
				Command.ExecNonQuery("DELETE FROM memberloanauthorizecategory WHERE Member_Id = " + Member_Id);
				TwoArrayList i = new TwoArrayList();
				DataTable CategoryLoan_id = Command.ExecDataAdapter("SELECT Id,Name FROM COLLECTIONCATEGORYS");
				for (int tarLoanLocation = 0; tarLoanLocation < CategoryLoan_id.Rows.Count; tarLoanLocation++)
				{
					int dtLoanLocation = int.Parse(CategoryLoan_id.Rows[tarLoanLocation]["id"].ToString());
					i.Clear();
					i.Add("Member_Id", Member_Id);
					i.Add("RegisterBy", CreateBy);
					i.Add("RegisterDate", DateTime.Now);
					i.Add("RegisterTerminal", Terminal);
					i.Add("CategoryLoan_id", dtLoanLocation);
					if (Command.ExecInsertOrUpdate("memberloanauthorizecategory", i, Command.InsertOrUpdate.Insert, null))
					{
					}
				}
				Command.ExecNonQuery("DELETE FROM memberloanauthorizelocation WHERE Member_Id = " + Member_Id);
				TwoArrayList LocationLoan_id = new TwoArrayList();
				DataTable nickname = Command.ExecDataAdapter("SELECT Id,Name FROM LOCATIONLOAN");
				for (int tarLoanLocation = 0; tarLoanLocation < nickname.Rows.Count; tarLoanLocation++)
				{
					int num = int.Parse(nickname.Rows[tarLoanLocation]["id"].ToString());
					LocationLoan_id.Clear();
					LocationLoan_id.Add("Member_Id", Member_Id);
					LocationLoan_id.Add("RegisterBy", CreateBy);
					LocationLoan_id.Add("RegisterDate", DateTime.Now);
					LocationLoan_id.Add("RegisterTerminal", Terminal);
					LocationLoan_id.Add("LocationLoan_id", num);
					if (Command.ExecInsertOrUpdate("memberloanauthorizelocation", LocationLoan_id, Command.InsertOrUpdate.Insert, null))
					{
					}
				}
				string secondValue = Member.NickGenerator(NamaAnggota, 3);
				tarWhere.Clear();
				tarWhere.Add("NoAnggota", MemberNo);
				tar.Clear();
				tar.Add("NickName", secondValue);
				tar.Add("Email", EmailAddress.ToLower());
				tar.Add("UpdateBy", UpdateBy);
				tar.Add("UpdateDate", DateTime.Now);
				tar.Add("UpdateTerminal", Terminal);
				if (Command.ExecInsertOrUpdate("MEMBERSONLINE", tar, Command.InsertOrUpdate.Update, " WHERE NoAnggota = " + Connection.ParameterSymbol + "NoAnggota", tarWhere))
				{
					return "Update Data Anggota " + MemberNo + " Sukses";
				}
				return "Error : Update Error! Data Anggota" + MemberNo + "\r\n" + Page.Session[MySession.CurrentErrorMessage].ToString();
			}
			return "Error : Update Error! Data Anggota " + MemberNo + "\r\n" + Page.Session[MySession.CurrentErrorMessage].ToString();
		}
		return "Update Data Anggota " + MemberNo + " Tidak Diperlukan";
	}

	public static string ChangePassword(Page Page, string MemberNo, string OldPassword, string NewPassword)
	{
		TwoArrayList tarUserPassword = new TwoArrayList();
		tarUserPassword.Add("NoAnggota", MemberNo);
		string CurrentUserPassword = Command.ExecScalar(tarUserPassword, "SELECT Password FROM MEMBERSONLINE WHERE NoAnggota = " + Connection.ParameterSymbol + "NoAnggota", "");
		string InputPass = HashCode(OldPassword);
		if (InputPass != CurrentUserPassword)
		{
			return "Error : Maaf, Password lama tidak sesuai dengan database Kami";
		}
		return SavePassword(Page, MemberNo, NewPassword);
	}

	private static string SavePassword(Page Page, string MemberNo, string NewPassword)
	{
		TwoArrayList tarWhere = new TwoArrayList();
		tarWhere.Add("NoAnggota", MemberNo);
		TwoArrayList tar = new TwoArrayList();
		tar.Add("Password", HashCode(NewPassword));
		if (Command.ExecInsertOrUpdate("MEMBERSONLINE", tar, Command.InsertOrUpdate.Update, "WHERE NoAnggota = " + Connection.ParameterSymbol + "NoAnggota", tarWhere))
		{
			return "Password telah diubah";
		}
		return " Error : Saving Error!\n\n" + Page.Session[MySession.CurrentErrorMessage].ToString();
	}

	public static string GetCollectionReadList(string MemberNo, string LoanDate)
	{
		JavaScriptSerializer js = new JavaScriptSerializer();
		List<CollectionReadItemResult> r = new List<CollectionReadItemResult>();
		TwoArrayList tar = new TwoArrayList();
		Connection.SetConnection();
		string SQLAND = " WHERE 1=1";
		if (!string.IsNullOrEmpty(MemberNo))
		{
			SQLAND = SQLAND + " AND MemberNo = " + Connection.ParameterSymbol + "MemberNo";
			tar.Add("MemberNo", MemberNo);
		}
		if (!string.IsNullOrEmpty(LoanDate))
		{
			SQLAND = SQLAND + " AND TO_CHAR(LoanDate, 'YYYY-MM-DD') >= " + Connection.ParameterSymbol + "LoanDate";
			tar.Add("LoanDate", DateTime.Parse(LoanDate).ToString("yyyy-MM-dd"));
		}
		DataTable dt = Command.ExecDataAdapter("SELECT LoanDate,NomorBarcode,COL.Title,COL.Author FROM CollectionReadItems CRI INNER JOIN Collections COL ON CRI.Collection_Id = COL.Id INNER JOIN Members M ON CRI.Member_Id = M.Id" + SQLAND, tar);
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			CollectionReadItemResult ResultItem = new CollectionReadItemResult();
			ResultItem.LoanDate = Convert.ToDateTime(dt.Rows[i]["LoanDate"].ToString()).ToString("yyyy-MM-dd");
			ResultItem.LoanDateTime = Convert.ToDateTime(dt.Rows[i]["LoanDate"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
			ResultItem.NomorBarcode = dt.Rows[i]["NomorBarcode"].ToString();
			ResultItem.Title = dt.Rows[i]["Title"].ToString();
			ResultItem.Author = dt.Rows[i]["Author"].ToString();
			r.Add(ResultItem);
		}
		var result = new
		{
			items = r.ToArray()
		};
		return js.Serialize(result);
	}

	public static string SetServiceStatusON(Page Page, string BranchId)
	{
		try
		{
			Connection.SetConnection();
			TwoArrayList tar = new TwoArrayList();
			tar.Add("ONOFFSTATUS", 1);
			tar.Add("LASTONSTATUS", DateTime.Now);
			if (Command.ExecInsertOrUpdate("BRANCHS", tar, Command.InsertOrUpdate.Update, " WHERE ID=" + BranchId))
			{
				return "Sukses";
			}
			return "Error : " + Page.Session[MySession.CurrentErrorMessage].ToString();
		}
		catch (Exception ex)
		{
			return "Error : " + ex.Message;
		}
	}
}
