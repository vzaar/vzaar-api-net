<%@ Page Language="VB" ContentType="text/xml" %>
<script runat="server">

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)	
		Dim api As New VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, getAppSettings("vzaar.token"), getAppSettings("vzaar.secret"))
		Dim signature As String = api.GetUploadSignatureAsXml(True)
		
		Response.Write(signature)
	End Sub
</script>