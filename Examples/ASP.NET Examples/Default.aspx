<%@ Page Language="VB" %>

<script runat="server">

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim api As New VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, getAppSettings("vzaar.token"), getAppSettings("vzaar.secret"))
		Response.Write(api.WhoAmI)
		
		'Dim ret As Integer = api.UploadVideo("demo video", "just demonstration on how to upload video", VzaarAPI.VideoProfile.Medium, AppDomain.CurrentDomain.BaseDirectory + "video.flv")
		'uploadStatus.Text = ret.ToString
	End Sub
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<asp:Literal ID="uploadStatus" runat="server" />
	</div>
	</form>
</body>
</html>
