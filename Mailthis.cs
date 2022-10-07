// Mailthis
using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Threading;

public class Mailthis
{
	public static bool Activation(string nomember, string mailaddress, string membername, string activationcode)
	{
		Thread th = new Thread((ThreadStart)delegate
		{
			DoActivation(nomember, mailaddress, membername, activationcode);
		});
		th.Start();
		return true;
	}

	private static bool DoActivation(string nomember, string mailaddress, string membername, string activationcode)
	{
		Thread.Sleep(2000);
		string Host = "mail.pnri.go.id";
		int Port = 587;
		string CredentialEmail = "layananpasswordanggota@pnri.go.id";
		string CredentialPassword = "pusn4s";
		bool EnableSsl = false;
		string MailDisplayName = "Perpustakaan Nasional RI";
		try
		{
			DataTable dt = Command.ExecDataAdapter("SELECT * FROM MAILSERVER WHERE Modul = 'Email Center Keanggotaan'");
			if (dt.Rows.Count > 0)
			{
				Host = dt.Rows[0]["HOST"].ToString();
				Port = int.Parse(dt.Rows[0]["PORT"].ToString());
				CredentialEmail = dt.Rows[0]["CREDENTIALMAIL"].ToString();
				CredentialPassword = dt.Rows[0]["CREDENTIALPASSWORD"].ToString();
				EnableSsl = Util.ConvertToBoolean(dt.Rows[0]["ENABLESSL"].ToString());
				MailDisplayName = dt.Rows[0]["MAILDISPLAYNAME"].ToString();
			}
			MailMessage mailMessage = new MailMessage();
			mailMessage.Subject = "Aktivasi Pendaftaran Anggota Perpustakaan Nasional RI";
			mailMessage.Body = "Selamat " + membername + ", akun anda telah dibuat.<br/>No Anggota anda adalah : " + nomember + "<br/>Kode aktivasi anda adalah : " + activationcode + "<br/>Klik link dibawah ini untuk mengaktifkan akun anda : <br/>" + MyApplication.GetActivationServerURL() + "MailActivation.aspx?id=" + nomember + "&activationcode=" + activationcode;
			mailMessage.IsBodyHtml = true;
			mailMessage.From = new MailAddress(CredentialEmail, MailDisplayName);
			MailMessage email = mailMessage;
			email.To.Add(new MailAddress(mailaddress, membername));
			SmtpClient server = new SmtpClient();
			server.Host = Host;
			server.Port = Port;
			server.Credentials = new NetworkCredential(CredentialEmail, CredentialPassword);
			server.EnableSsl = EnableSsl;
			server.Send(email);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public static bool SendPassword(string nomember, string mailaddress, string membername, string password)
	{
		Thread th = new Thread((ThreadStart)delegate
		{
			DoSendPassword(nomember, mailaddress, membername, password);
		});
		th.Start();
		return true;
	}

	private static bool DoSendPassword(string nomember, string mailaddress, string membername, string password)
	{
		Thread.Sleep(2000);
		try
		{
			string Host = "mail.pnri.go.id";
			int Port = 587;
			string CredentialEmail = "layananpasswordanggota@pnri.go.id";
			string CredentialPassword = "pusn4s";
			bool EnableSsl = false;
			string MailDisplayName = "Perpustakaan Nasional RI";
			DataTable dt = Command.ExecDataAdapter("SELECT * FROM MAILSERVER WHERE Modul = 'Email Center Keanggotaan'");
			if (dt.Rows.Count > 0)
			{
				Host = dt.Rows[0]["HOST"].ToString();
				Port = int.Parse(dt.Rows[0]["PORT"].ToString());
				CredentialEmail = dt.Rows[0]["CREDENTIALMAIL"].ToString();
				CredentialPassword = dt.Rows[0]["CREDENTIALPASSWORD"].ToString();
				EnableSsl = Util.ConvertToBoolean(dt.Rows[0]["ENABLESSL"].ToString());
				MailDisplayName = dt.Rows[0]["MAILDISPLAYNAME"].ToString();
			}
			MailMessage mailMessage = new MailMessage();
			mailMessage.Subject = "Password Anda";
			mailMessage.Body = "Hai " + membername + " <br/> Ini adalah surat penjawab otomatis atas permintaan Anda pada halaman lupa password keanggotaan Perpusnas. <br/> Nomor anggota : " + nomember + " <br/> Password sementara : " + password + "<br/>Silahkan login ke portal layanan keanggotaan online Perpusnas (keanggotaan.perpusnas.go.id) untuk mengganti password sementara dengan password yang anda inginkan. <br/><br/> Apabila masih terdapat kesulitan dalam memanfaatkan layanan ini silahkan menghubungi kelompok layanan keanggotaan melalui email :  info@perpusnas.go.id atau datang langsung ke kantor layanan Perpustakaan Nasional Republik Indonesia <br/>Atas perhatian dan kerjasama yang baik saya ucapkan terima kasih.";
			mailMessage.IsBodyHtml = true;
			mailMessage.From = new MailAddress(CredentialEmail, MailDisplayName);
			MailMessage email = mailMessage;
			email.To.Add(new MailAddress(mailaddress, membername));
			SmtpClient server = new SmtpClient();
			server.Host = Host;
			server.Port = Port;
			server.Credentials = new NetworkCredential(CredentialEmail, CredentialPassword);
			server.EnableSsl = EnableSsl;
			server.Send(email);
			return true;
		}
		catch (Exception ex)
		{
			Util.CreateLog(ex.ToString());
			return false;
		}
	}
}
