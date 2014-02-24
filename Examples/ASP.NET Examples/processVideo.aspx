<%@ Page Language="VB" %>

<script runat="server">

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim api As New VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, getAppSettings("vzaar.token"), getAppSettings("vzaar.secret"))
		Response.Write(api.WhoAmI)
		
		Dim videoId As Integer = api.ProcessVideo("vz22091fbaed324e658367cb26146a2863", "demo", "demo", "", VzaarAPI.VideoProfile.Original)
		Status.Text = "Video ID: " + videoId.ToString
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
		<asp:Literal ID="Status" runat="server" />
	</div>
	</form>
</body>
</html>
