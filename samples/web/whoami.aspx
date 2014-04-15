<%@ Page Language="C#" %>

<%@ Import Namespace="com.vzaar.api" %>

<!DOCTYPE html>

<script runat="server">

	protected void Page_Load(object sender, EventArgs e)
	{
		var api = new Vzaar("skitsanos", "dbeFa3DVI71jQ7dTtz9mHA953XeIQeodmZvSE6AbTX8");
		status.Text = api.whoAmI();
	}
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<asp:Literal ID="status" runat="server" />
</body>
</html>
