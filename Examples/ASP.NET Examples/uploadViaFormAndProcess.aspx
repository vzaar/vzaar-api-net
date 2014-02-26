<%@ Page Language="VB" %>

<script runat="server">
	Protected signature As VzaarAPI.UploadSignature
	Protected redirectUrl As String = HttpUtility.UrlEncode("http://vzaarnet.mywdk.com/uploadViaFormAndProcess.aspx", Encoding.UTF8)
	
	Protected videoId As Integer
	
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim api As New VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, getAppSettings("vzaar.token"), getAppSettings("vzaar.secret"))
		
		signature = api.GetUploadSignature(redirectUrl)
		
		If Request("guid") <> "" Then
			videoId = api.ProcessVideo(Request("guid"), "Some title", "some description", VzaarAPI.VideoProfile.Medium)
			Response.Write(videoId)
			Response.End()
		End If
	End Sub
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
	<meta name="author" content="Skitsanos.com" />
	<title></title>
</head>
<body>
	<form action="https://<%=signature.GetBucket()%>.s3.amazonaws.com/" method="post" enctype="multipart/form-data">
		<input type="hidden" name="acl" value="<%=signature.GetAcl()%>" /> 
		<input type="hidden" name="bucket" value="<%=signature.GetBucket()%>" /> 
		<input type="hidden" name="policy" value="<%=signature.GetPolicy()%>" /> 
		<input type="hidden" name="AWSAccessKeyId" value="<%=signature.GetAccessKeyId()%>" /> 		
		<input type="hidden" name="signature" value="<%=signature.GetSignature()%>" /> 
		<input type="hidden" name="success_action_status" value="201" /> 
		<input type="hidden" name="success_action_redirect" value="<%=HttpUtility.UrlDecode(redirectUrl)%>?guid=<%=signature.getGuid() %>" /> 
		<input type="hidden" name="key" value="<%=signature.GetKey()%>" /> File to upload to S3: <input name="file" type="file" />
		<br />
		<input type="submit" value="Upload File to S3" />
	</form>
</body>
</html>
