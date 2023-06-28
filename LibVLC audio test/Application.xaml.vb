Class Application
    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup
        MainWndw = New MainWindow With {
            .Title = "MainWndw",
            .WindowStyle = WindowStyle.SingleBorderWindow,
            .Width = 1366,
            .Height = 768
        }
        MainWndw.Show()
    End Sub
End Class
