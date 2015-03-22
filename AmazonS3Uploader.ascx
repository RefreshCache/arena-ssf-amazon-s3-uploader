<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AmazonS3Uploader.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.ssf.AmazonS3Uploader" %>
<%@ Register TagPrefix="fjx" Namespace="com.flajaxian" Assembly="com.flajaxian.FileUploader" %>
<%@ Register TagPrefix="fjx" Namespace="com.flajaxian" Assembly="com.flajaxian.DirectAmazonUploader" %>
<style>
.msg_negative {border:#b76060 3px solid;background:#d7a8a8;color:#b76060;font-size:12px;padding:6px;}
.msg_positive {border:#2e7f47 3px solid;background:#a8d7b7;color:#2e7f47;font-size:12px;padding:6px;}
</style>
<br />
<table border="0" cellspacing="0" cellpadding="4">
<tr><td>
<table border="0" cellspacing="0" cellpadding="4">
	<tr><td>Access Key:</td><td><asp:Label runat="server" id="lblAccessKey" /></td></tr>
	<tr><td>Bucket Name:</td><td><asp:Label runat="server" id="lblBucketName" /></td></tr>
	<tr><td>Path:</td><td><asp:Label runat="server" id="lblPath" /></td></tr>
</table>
<br /><br />
<script type="text/javascript">
function ConfirmUploadJsFunc(r)
{
	if (!r.isError)
	{
		updateResultsBox(r,0);
	}
	else
	{
		updateResultsBox(r,1);
	}
            
	if (r.isLast)
	{
		if (<%=Convert.ToInt32(S3EmailAddressSetting.Contains("@"))%>)
		{
			document.getElementById('<%=txtUploadResults.ClientID%>').value = escape(document.getElementById('<%=divUploadResults.ClientID%>').innerHTML);	
			document.getElementById('divEmailPanel').style.display="block";
		}
	}
}

function updateResultsBox(r,errorFlag)
{
	var UploadResultsBox = "<%=divUploadResults.ClientID%>";
	
	if (errorFlag) // if there is an error
	{
		document.getElementById(UploadResultsBox).innerHTML += "<div class='msg_negative'>"
		+ "There was an error uploading the file: " + r.name + " (" + r.httpStatus + ")" + "</div>";
	}
	else //if no error
	{
		var fullPath = "http://<%=S3BucketNameSetting%>.s3.amazonaws.com/" + r.path + r.name;
		
		document.getElementById(UploadResultsBox).innerHTML += "<div class='msg_positive'>"
		+ "File Uploaded Successfully." + "<br />"
		+ "HTTP Status: " + r.httpStatus + "<br />"
		+ "File Name: " + "<a target='_blank' href='" + fullPath + "'>" + r.name + "</a>" + "<br />"
		+ "File Size: " + r.bytes + "<br />"
		+ getExtraFileInfo(fullPath)
		+ "</div><br />";
	}
}

function getExtraFileInfo(path)
{
	var extension = (/[.]/.exec(path)) ? /[^.]+$/.exec(path) : undefined;
	var returnString = "";
	
	switch(extension)
	{
		case "mp3":
			//objAudio = new Audio(path);
			//returnString = "MP3 Duration: " + objAudio.duration;
			break;
		default:
			break;
	}
	
	return returnString;
}
</script>
<fjx:FileUploader ID="flxUpload" runat="server">
    <Adapters>
        <fjx:DirectAmazonUploader OnConfirmUpload="flxUpload_ConfirmUpload" ConfirmUploadJsFunc="ConfirmUploadJsFunc" />
    </Adapters>
</fjx:FileUploader>
<asp:Label runat="server" id="lblUploadResults" text="Upload Results:" style="display:none;" /><br />
</td><td>
	<div runat="server" id="divUploadResults"></div>
	<asp:Textbox runat="server" id="txtUploadResults" style="display:none;" />
	<asp:Label runat="server" id="lblMessage" />
</td></tr>
<tr><td colspan="2">
	<div id="divEmailPanel" style="display:none;background-color:#eee;">
	<table border="0" cellspacing="0" cellpadding="3">
	<tr><td>Send email to:</td><td><asp:Textbox runat="server" id="txtEmailAddress" style="width:250px;" /></td></tr>
	<tr><td>Subject:</td><td><asp:Textbox runat="server" id="txtEmailSubject" style="width:350px;" /></td></tr>
	<tr><td>Additional Email Notes (audio time duration, etc.):</td><td><asp:Textbox id="txtAdditionalEmailNotes" rows="5" TextMode="multiline" runat="server" /></td></tr>
	<tr><td>
		<table border="0" cellspacing="0" cellpadding="3">
			<tr>
				<td><asp:LinkButton runat="server" id="btnSendEmail" onClick="btnSendEmail_Click" text="Send Email" /></td>
				<td>&nbsp;&nbsp;|&nbsp;&nbsp;</td>
				<td><a href="#" onclick="document.getElementById('divEmailPanel').style.display='none';">Cancel</a></td>
			</tr>
		</table>
	</td></tr>
	</table>
	</div>
</td>
</tr>
</table>