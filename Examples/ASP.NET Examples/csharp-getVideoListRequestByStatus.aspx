<%@ Page Language="C#" %>

<%@ Import Namespace="System.Globalization" %>

<script runat="server">
	
	protected string getAppSettings(string key)
	{
		return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath).AppSettings.Settings[key].Value;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		var api = new VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, getAppSettings("vzaar.token"), getAppSettings("vzaar.secret"));

		var request = new VzaarAPI.VideoListRequest(getAppSettings("vzaar.secret"))
		{
			count = 100,
			page = 1,
			includeSize = true,
			sortAscending = true,
			title = "s3",
			status = VzaarAPI.VideoStatusFilter.PROCESSING
		};

		var videoList = api.GetVideoList(request);

		status.Text = videoList.Count.ToString(CultureInfo.InvariantCulture) + " videos in processing found<p/>";

		foreach (var video in videoList)
		{
			status.Text += video.GetTitle() + "<br/>";
		}
	}
</script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<form id="form1" runat="server">
		<asp:Literal runat="server" ID="status" />
	</form>
</body>
</html>
