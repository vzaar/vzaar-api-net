<%@ Page Language="VB" %>


<script runat="server">

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		If (Request("token") <> "" Or Request("secret") <> "") Then
			Dim api As New VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, Request("token"), Request("secret"))
		
			Dim signature As VzaarAPI.UploadSignature = api.GetUploadSignature()
		
			status.Text = signature.signature
		Else
			status.Text = "Missed Query string params like: token=your_api_token&secret=your_vzaar_username"
		End If
		
	End Sub
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <asp:Literal runat="server" ID="status" />
</body>
</html>
