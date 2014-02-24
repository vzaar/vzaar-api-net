<%@ Page Language="VB" %>

<script runat="server">
	Protected signature As VzaarAPI.UploadSignature
	
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim api As New VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, getAppSettings("vzaar.token"), getAppSettings("vzaar.secret"))
		
		signature = api.GetUploadSignature()
	End Sub
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form action="https://<%=signature.GetBucket()%>.s3.amazonaws.com/" method="post" enctype="multipart/form-data">
		<input type="hidden" name="key" value="<%=signature.key()%>" /> 
		<input type="hidden" name="AWSAccessKeyId" value="<%=signature.GetAccessKeyId()%>" /> 
		<input type="hidden" name="acl" value="<%=signature.GetAcl()%>" /> 
		<input type="hidden" name="policy" value="<%=signature.GetPolicy()%>" /> 
		<input type="hidden" name="success_action_status" value="201" /> 
		<input type="hidden" name="signature" value="<%=signature.GetSignature()%>" /> 
		File to upload to S3: <input name="file" type="file" />
		<br />
		<input type="submit" value="Upload File to S3" />
	</form>
</body>
</html>
