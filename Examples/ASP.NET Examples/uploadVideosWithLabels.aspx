<%@ Page Language="VB" %>

<!DOCTYPE html>

<script runat="server">

	Protected Sub Page_Load(sender As Object, e As EventArgs)
		Dim api As New VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, getAppSettings("vzaar.token"), getAppSettings("vzaar.secret"))
			
		Dim ret As Integer = api.UploadVideo("demo labeled video", "just demonstration on how to upload video with labels", "vzaar,demo", VzaarAPI.VideoProfile.Original, AppDomain.CurrentDomain.BaseDirectory + "video.flv")
		uploadStatus.Text = ret.ToString
	End Sub
</script>

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
