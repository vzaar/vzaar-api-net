<%@ Page Language="VB" %>

<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		Dim api As New VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, getAppSettings("vzaar.token"), getAppSettings("vzaar.secret"))
			
		Dim req As New VzaarAPI.VideoListRequest(getAppSettings("vzaar.secret"))
		req.status = VzaarAPI.VideoStatusFilter.PROCESSING
		
		Dim ret As List(Of VzaarAPI.Video) = api.GetVideoList(req)
		Status.Text = ret.Count.ToString + " videos found"
		
		'Assign list to a repeater
		repeater.DataSource = ret
		repeater.DataBind()
	End Sub
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link href="http://fonts.googleapis.com/css?family=Open+Sans:300italic,400italic,600italic,700italic,800italic,400,600,700,300,800"
		rel="stylesheet" type="text/css" />
	<style type="text/css">
		* {
			font-family: 'Open Sans', sans-serif;
		}

		h3 {
			margin-bottom: 0;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
		<div>
			<asp:Literal ID="Status" runat="server" />

			<asp:Repeater runat="server" ID="repeater">
				<ItemTemplate>
					<h3><%# CType(Container.DataItem, VzaarAPI.Video).GetTitle()%> (<small><%# CType(Container.DataItem, VzaarAPI.Video).GetPlayCount()%></small>)</h3>
					<br />
					by <%# CType(Container.DataItem, VzaarAPI.Video).GetAuthorName()%> (#<%# CType(Container.DataItem, VzaarAPI.Video).GetAuthorAccount()%>)
					<hr style="border: none; border-bottom: dotted 1px #ddd; margin-top: 10px; margin-bottom: 10px;" />
				</ItemTemplate>
			</asp:Repeater>
		</div>
	</form>
</body>
</html>
