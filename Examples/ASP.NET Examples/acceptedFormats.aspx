<%@ Page Language="VB" %>

<script runat="server">
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim api As New VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, getAppSettings("vzaar.token"), getAppSettings("vzaar.secret"))
		For Each file As VzaarAPI.AcceptedFileExtension In VzaarAPI.AcceptedFileExtension.GetAcceptedFormats
			'console.Text += file.GetExtension + " -- " + file.GetDescription + "<br/>"
			console.Text += "filter+=""|" + file.GetDescription + "|*" + file.GetExtension + """;<br/>"
		Next
	End Sub
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<asp:Literal runat="server" ID="console" />
</body>
</html>
