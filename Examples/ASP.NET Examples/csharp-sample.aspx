<%@ Page Language="C#" %>

<script runat="server">
	
	protected string getAppSettings(string key)
	{
		return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath).AppSettings.Settings[key].Value;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		VzaarAPI.Vzaar api = new VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, "api_key", "vzaar_username");
		status.Text = api.WhoAmI();
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
