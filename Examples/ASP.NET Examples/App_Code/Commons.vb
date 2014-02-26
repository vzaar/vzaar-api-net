Imports System.Web

Public Module Commons

#Region " getAppSettings "
	Public Function getAppSettings(ByVal key As String) As String
		Return Web.Configuration.WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath).AppSettings.Settings(key).Value
	End Function
#End Region

End Module
