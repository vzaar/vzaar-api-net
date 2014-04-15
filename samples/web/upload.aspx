<%@ Page Language="C#" %>

<%@ Import Namespace="com.vzaar.api" %>
<!DOCTYPE html>

<script runat="server">
	protected UploadSignature signature;

	protected void Page_Load(object sender, EventArgs e)
	{
		var api = new Vzaar("skitsanos", "u2nd3DVI71jQ7dTtz9mHA953XeIQeodmZvSE6AbTX8");
		signature = api.getUploadSignature();
	}
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form action="https://<%=signature.bucket%>.s3.amazonaws.com/" method="post" enctype="multipart/form-data">
		<input type="hidden" name="key" value="<%=signature.key%>" /> 
		<input type="hidden" name="AWSAccessKeyId" value="<%=signature.accessKeyId%>" /> 
		<input type="hidden" name="acl" value="<%=signature.acl%>" /> 
		<input type="hidden" name="policy" value="<%=signature.policy%>" /> 
		<input type="hidden" name="success_action_status" value="201" /> 
		<input type="hidden" name="signature" value="<%=signature.signature%>" /> 
		File to upload to S3: <input name="file" type="file" />
		<br />
		<input type="submit" value="Upload File to S3" />
	</form>
</body>
</html>
