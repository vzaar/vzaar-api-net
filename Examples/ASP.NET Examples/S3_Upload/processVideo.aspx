<%@ Page Language="VB" %>

<script runat="server">

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim api As New VzaarAPI.Vzaar(VzaarAPI.Vzaar.URL_LIVE, getAppSettings("vzaar.token"), getAppSettings("vzaar.secret"))
		
		If (Request("guid") <> "") Then
			Dim apireply As String = api.ProcessVideo(Request("guid"), Request("title"), Request("description"), VzaarAPI.VideoProfile.Small)
			Response.Write(apireply)
		Else
			Response.Write("GUID is missing")
		End If
	End Sub
</script>
