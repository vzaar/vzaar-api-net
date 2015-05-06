<%@ Page Language="C#" %>

<%@ Import Namespace="com.vzaar.api" %>

<!DOCTYPE html>

<script runat="server">

	protected void Page_Load(object sender, EventArgs e)
	{
		var api = new Vzaar("username", "token");
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
