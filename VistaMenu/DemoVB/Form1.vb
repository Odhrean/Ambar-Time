Public Class Form1

    Private Sub btnSetOptionsImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetOptionsImage.Click
        vistaMenu.SetImage(mnuOptions, vistaMenu.GetImage(mnuAddFiles))
    End Sub

    Private Sub btnClearOptionsImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearOptionsImage.Click
        vistaMenu.SetImage(mnuOptions, Nothing)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Font = SystemFonts.MessageBoxFont

        Me.contextMenu = contextMenu
    End Sub
End Class
