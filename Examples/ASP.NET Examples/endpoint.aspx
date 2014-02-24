<%@ Page Language="VB" %>

<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
		Dim params As NameValueCollection = Nothing
		
		If Request.HttpMethod = "GET" Then
			params = Request.QueryString
		Else
			params = Request.Form
		End If
		
		Dim tmp As New List(Of String)
		
		For Each key As String In params
			tmp.Add(key + "=" + params(key))
		Next
		
		FileIO.FileSystem.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "bot.txt", Join(tmp.ToArray, "&"), False)
				
		Response.Write("{""status"": 200}")
	End Sub
</script>
