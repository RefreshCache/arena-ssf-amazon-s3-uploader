/**********************************************************************
* Description:	Interface to upload files to Amazon S3 through Arena, administrator provides bucket/path settings, user provides files
* Created By:   Joe Adams 
* Date Created:	June 2011
**********************************************************************/

using System;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
using com.flajaxian;
using Arena.Portal;
using Arena.Portal.UI;

namespace ArenaWeb.UserControls.Custom.ssf
{
	public partial class AmazonS3Uploader : PortalControl
	{		
		[TextSetting("S3 Access Key", "The access key for the S3 account.", true)]
        public string S3AccessKeySetting { get { return Setting("S3AccessKey", "-1", true); } }
		
		[TextSetting("S3 Secret Key", "The secret key for the S3 account.", true)]
        public string S3SecretKeySetting { get { return Setting("S3SecretKey", "-1", true); } }
		
		[TextSetting("S3 Bucket Name", "The bucket you will be uploading to.", true)]
        public string S3BucketNameSetting { get { return Setting("S3BucketName", "-1", true); } }
		
		[TextSetting("S3 Folder Path", "The folder path under the bucket you will be uploading to. (Leave blank to upload to root bucket.)", false)]
        public string S3PathSetting { get { return Setting("S3Path", "", false); } }
		
        [CustomListSetting("S3 File Access", "File permissions to give to uploaded files.", true, "", 
            new[] { "Public Read*", "Public Read/Write", "Authenticated Read", "Private" }, new[] { "PublicRead", "PublicReadWrite", "AuthenticatedRead", "Private"})]
        public string S3FileAccessSetting { get { return Setting("S3FileAccess", "", true); } }
		
		[TextSetting("Email Subject", "The default subject of the email. (This can be changed by the user during the session at any time.)", true)]
        public string S3EmailSubjectSetting { get { return Setting("S3EmailSubject", "", true); } }
		
		[TextSetting("Email Address(es) to send confirmation to", "Comma-delimited list of email addresses to send confirmation of file upload to. (Leave blank for no confirmation email to be sent.)", false)]
        public string S3EmailAddressSetting { get { return Setting("S3EmailAddress", "", false); } }
		
		[TextSetting("SMTP server", "Server used to send emails from", true)]
        public string S3SMTPServerSetting { get { return Setting("S3SMTPServer", "localhost", true); } }
		
		private void Page_Load(object sender, EventArgs e)
        {
			if (!Page.IsPostBack)
			{
				lblAccessKey.Text = S3AccessKeySetting;
				lblBucketName.Text = S3BucketNameSetting;
				lblPath.Text = S3PathSetting;
				
				if (S3EmailAddressSetting.Contains("@"))
				{
					txtEmailSubject.Text = S3EmailSubjectSetting;
					txtEmailAddress.Text = S3EmailAddressSetting;
				}
				
				LoadFlajaxian();
			}
		}
		
		private void LoadFlajaxian()
		{
			com.flajaxian.DirectAmazonUploader aU = (com.flajaxian.DirectAmazonUploader)flxUpload.Adapters[0];
				
			aU.AccessKey = S3AccessKeySetting;
			aU.SecretKey = S3SecretKeySetting;
			aU.BucketName = S3BucketNameSetting;
			aU.Path = S3PathSetting;
				
			switch(S3FileAccessSetting)
			{
				case "PublicReadWrite":
					aU.FileAccess = com.flajaxian.FileAccess.PublicReadWrite;
					break;
				case "AuthenticatedRead":
					aU.FileAccess = com.flajaxian.FileAccess.AuthenticatedRead;
					break;
				case "Private":
					aU.FileAccess = com.flajaxian.FileAccess.Private;
					break;
				case "PublicRead":
				default:
					aU.FileAccess = com.flajaxian.FileAccess.PublicRead;
					break;
			}
		}
		
		protected void flxUpload_ConfirmUpload(object sender, ConfirmUploadEventArgs e)
		{
			//could do something on the server here if necessary
		}
		
		protected void btnSendEmail_Click(object sender, EventArgs e)
		{			
			string strBody = txtUploadResults.Text + "<br /><br />";
			
			strBody += "Date/Time Uploaded: " + DateTime.Now.ToString("f") + "<br /><br />";
				
			strBody += "Additional Notes:" + "<br /><br />" + txtAdditionalEmailNotes.Text + "<br /><br />";
				
			if (txtEmailAddress.Text != "") // if email adddress is specified, prepare the message, otherwise don't
			{
				MailMessage mailObj = new MailMessage();
				mailObj.From = new MailAddress(CurrentPerson.Emails.FirstActive.ToString());
				mailObj.To.Add(txtEmailAddress.Text);
				mailObj.Subject = txtEmailSubject.Text;
				mailObj.Body = HttpUtility.UrlDecode(strBody);
				mailObj.IsBodyHtml = true;
				SmtpClient SMTPServer = new SmtpClient(S3SMTPServerSetting);
				try
				{
					SMTPServer.Send(mailObj);
					lblMessage.Text = "Message sent successfully.";
					lblMessage.CssClass = "msg_positive";
				}
				catch (System.Exception ex)
				{
					lblMessage.Text = "Unable to send email: <br>" + ex.ToString();
					lblMessage.CssClass = "msg_negative";
				}
			}
			LoadFlajaxian();
		}
	}
}

