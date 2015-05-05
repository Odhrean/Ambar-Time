<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.vistaMenu = New wyDay.Controls.VistaMenu(Me.components)
        Me.contextMenu = New System.Windows.Forms.ContextMenu
        Me.mnuRename = New System.Windows.Forms.MenuItem
        Me.mnuAddFiles = New System.Windows.Forms.MenuItem
        Me.mnuAddFolder = New System.Windows.Forms.MenuItem
        Me.mnuNewFolder = New System.Windows.Forms.MenuItem
        Me.menuItem12 = New System.Windows.Forms.MenuItem
        Me.mnuRemoveFolder = New System.Windows.Forms.MenuItem
        Me.mainMenu = New System.Windows.Forms.MainMenu(Me.components)
        Me.mnuFile = New System.Windows.Forms.MenuItem
        Me.menuItem4 = New System.Windows.Forms.MenuItem
        Me.menuItem11 = New System.Windows.Forms.MenuItem
        Me.menuItem13 = New System.Windows.Forms.MenuItem
        Me.menuItem14 = New System.Windows.Forms.MenuItem
        Me.menuItem15 = New System.Windows.Forms.MenuItem
        Me.menuItem16 = New System.Windows.Forms.MenuItem
        Me.menuItem17 = New System.Windows.Forms.MenuItem
        Me.mnuEdit = New System.Windows.Forms.MenuItem
        Me.menuItem6 = New System.Windows.Forms.MenuItem
        Me.menuItem7 = New System.Windows.Forms.MenuItem
        Me.mnuTools = New System.Windows.Forms.MenuItem
        Me.mnuOptions = New System.Windows.Forms.MenuItem
        Me.menuItem8 = New System.Windows.Forms.MenuItem
        Me.menuItem9 = New System.Windows.Forms.MenuItem
        Me.menuItem10 = New System.Windows.Forms.MenuItem
        Me.menuItem1 = New System.Windows.Forms.MenuItem
        Me.menuItem2 = New System.Windows.Forms.MenuItem
        Me.btnClearOptionsImage = New System.Windows.Forms.Button
        Me.btnSetOptionsImage = New System.Windows.Forms.Button
        CType(Me.vistaMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'vistaMenu
        '
        Me.vistaMenu.ContainerControl = Me
        '
        'contextMenu
        '
        Me.contextMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuRename, Me.mnuAddFiles, Me.mnuAddFolder, Me.mnuNewFolder, Me.menuItem12, Me.mnuRemoveFolder})
        '
        'mnuRename
        '
        Me.vistaMenu.SetImage(Me.mnuRename, CType(resources.GetObject("mnuRename.Image"), System.Drawing.Image))
        Me.mnuRename.Index = 0
        Me.mnuRename.Text = "Rename"
        '
        'mnuAddFiles
        '
        Me.vistaMenu.SetImage(Me.mnuAddFiles, CType(resources.GetObject("mnuAddFiles.Image"), System.Drawing.Image))
        Me.mnuAddFiles.Index = 1
        Me.mnuAddFiles.Text = "Add Files"
        '
        'mnuAddFolder
        '
        Me.vistaMenu.SetImage(Me.mnuAddFolder, CType(resources.GetObject("mnuAddFolder.Image"), System.Drawing.Image))
        Me.mnuAddFolder.Index = 2
        Me.mnuAddFolder.Text = "Add Folder"
        '
        'mnuNewFolder
        '
        Me.vistaMenu.SetImage(Me.mnuNewFolder, CType(resources.GetObject("mnuNewFolder.Image"), System.Drawing.Image))
        Me.mnuNewFolder.Index = 3
        Me.mnuNewFolder.Text = "New Folder"
        '
        'menuItem12
        '
        Me.menuItem12.Index = 4
        Me.menuItem12.Text = "-"
        '
        'mnuRemoveFolder
        '
        Me.vistaMenu.SetImage(Me.mnuRemoveFolder, CType(resources.GetObject("mnuRemoveFolder.Image"), System.Drawing.Image))
        Me.mnuRemoveFolder.Index = 5
        Me.mnuRemoveFolder.Text = "Remove"
        '
        'mainMenu
        '
        Me.mainMenu.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuFile, Me.mnuEdit, Me.mnuTools, Me.menuItem8})
        '
        'mnuFile
        '
        Me.mnuFile.Index = 0
        Me.mnuFile.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItem4, Me.menuItem11, Me.menuItem13, Me.menuItem14, Me.menuItem15, Me.menuItem16, Me.menuItem17})
        Me.mnuFile.Text = "&File"
        '
        'menuItem4
        '
        Me.vistaMenu.SetImage(Me.menuItem4, CType(resources.GetObject("menuItem4.Image"), System.Drawing.Image))
        Me.menuItem4.Index = 0
        Me.menuItem4.Shortcut = System.Windows.Forms.Shortcut.CtrlN
        Me.menuItem4.Text = "&New Project"
        '
        'menuItem11
        '
        Me.vistaMenu.SetImage(Me.menuItem11, CType(resources.GetObject("menuItem11.Image"), System.Drawing.Image))
        Me.menuItem11.Index = 1
        Me.menuItem11.Shortcut = System.Windows.Forms.Shortcut.CtrlO
        Me.menuItem11.Text = "&Open Project..."
        '
        'menuItem13
        '
        Me.menuItem13.Index = 2
        Me.menuItem13.Text = "&Close"
        '
        'menuItem14
        '
        Me.menuItem14.Index = 3
        Me.menuItem14.Text = "-"
        '
        'menuItem15
        '
        Me.vistaMenu.SetImage(Me.menuItem15, CType(resources.GetObject("menuItem15.Image"), System.Drawing.Image))
        Me.menuItem15.Index = 4
        Me.menuItem15.Shortcut = System.Windows.Forms.Shortcut.CtrlS
        Me.menuItem15.Text = "&Save"
        '
        'menuItem16
        '
        Me.menuItem16.Index = 5
        Me.menuItem16.Text = "-"
        '
        'menuItem17
        '
        Me.menuItem17.Index = 6
        Me.menuItem17.Text = "E&xit"
        '
        'mnuEdit
        '
        Me.mnuEdit.Index = 1
        Me.mnuEdit.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItem6, Me.menuItem7})
        Me.mnuEdit.Text = "&Edit"
        '
        'menuItem6
        '
        Me.vistaMenu.SetImage(Me.menuItem6, CType(resources.GetObject("menuItem6.Image"), System.Drawing.Image))
        Me.menuItem6.Index = 0
        Me.menuItem6.Shortcut = System.Windows.Forms.Shortcut.CtrlZ
        Me.menuItem6.Text = "&Undo"
        '
        'menuItem7
        '
        Me.menuItem7.Enabled = False
        Me.vistaMenu.SetImage(Me.menuItem7, CType(resources.GetObject("menuItem7.Image"), System.Drawing.Image))
        Me.menuItem7.Index = 1
        Me.menuItem7.Shortcut = System.Windows.Forms.Shortcut.CtrlY
        Me.menuItem7.Text = "&Redo"
        '
        'mnuTools
        '
        Me.mnuTools.Index = 2
        Me.mnuTools.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.mnuOptions})
        Me.mnuTools.Text = "&Tools"
        '
        'mnuOptions
        '
        Me.mnuOptions.Index = 0
        Me.mnuOptions.Text = "&Options..."
        '
        'menuItem8
        '
        Me.menuItem8.Index = 3
        Me.menuItem8.MenuItems.AddRange(New System.Windows.Forms.MenuItem() {Me.menuItem9, Me.menuItem10, Me.menuItem1, Me.menuItem2})
        Me.menuItem8.Text = "&Help"
        '
        'menuItem9
        '
        Me.menuItem9.Index = 0
        Me.menuItem9.RadioCheck = True
        Me.menuItem9.Text = "&Check for Updates"
        '
        'menuItem10
        '
        Me.menuItem10.Index = 1
        Me.menuItem10.Text = "-"
        '
        'menuItem1
        '
        Me.vistaMenu.SetImage(Me.menuItem1, CType(resources.GetObject("menuItem1.Image"), System.Drawing.Image))
        Me.menuItem1.Index = 2
        Me.menuItem1.Shortcut = System.Windows.Forms.Shortcut.F1
        Me.menuItem1.Text = "Online &Help"
        '
        'menuItem2
        '
        Me.menuItem2.Index = 3
        Me.menuItem2.Text = "&About"
        '
        'btnClearOptionsImage
        '
        Me.btnClearOptionsImage.Location = New System.Drawing.Point(12, 83)
        Me.btnClearOptionsImage.Name = "btnClearOptionsImage"
        Me.btnClearOptionsImage.Size = New System.Drawing.Size(177, 27)
        Me.btnClearOptionsImage.TabIndex = 3
        Me.btnClearOptionsImage.Text = "Clear 'Options' menu item image"
        Me.btnClearOptionsImage.UseVisualStyleBackColor = True
        '
        'btnSetOptionsImage
        '
        Me.btnSetOptionsImage.Location = New System.Drawing.Point(12, 50)
        Me.btnSetOptionsImage.Name = "btnSetOptionsImage"
        Me.btnSetOptionsImage.Size = New System.Drawing.Size(177, 27)
        Me.btnSetOptionsImage.TabIndex = 2
        Me.btnSetOptionsImage.Text = "Set 'Options' menu item image"
        Me.btnSetOptionsImage.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(305, 122)
        Me.Controls.Add(Me.btnClearOptionsImage)
        Me.Controls.Add(Me.btnSetOptionsImage)
        Me.Menu = Me.mainMenu
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "VistaMenu demo"
        CType(Me.vistaMenu, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents vistaMenu As wyDay.Controls.VistaMenu
    Private WithEvents mainMenu As System.Windows.Forms.MainMenu
    Private WithEvents mnuFile As System.Windows.Forms.MenuItem
    Private WithEvents menuItem4 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem11 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem13 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem14 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem15 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem16 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem17 As System.Windows.Forms.MenuItem
    Private WithEvents mnuEdit As System.Windows.Forms.MenuItem
    Private WithEvents menuItem6 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem7 As System.Windows.Forms.MenuItem
    Private WithEvents mnuTools As System.Windows.Forms.MenuItem
    Private WithEvents mnuOptions As System.Windows.Forms.MenuItem
    Private WithEvents menuItem8 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem9 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem10 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem1 As System.Windows.Forms.MenuItem
    Private WithEvents menuItem2 As System.Windows.Forms.MenuItem
    Private WithEvents contextMenu As System.Windows.Forms.ContextMenu
    Private WithEvents mnuRename As System.Windows.Forms.MenuItem
    Private WithEvents mnuAddFiles As System.Windows.Forms.MenuItem
    Private WithEvents mnuAddFolder As System.Windows.Forms.MenuItem
    Private WithEvents mnuNewFolder As System.Windows.Forms.MenuItem
    Private WithEvents menuItem12 As System.Windows.Forms.MenuItem
    Private WithEvents mnuRemoveFolder As System.Windows.Forms.MenuItem
    Private WithEvents btnClearOptionsImage As System.Windows.Forms.Button
    Private WithEvents btnSetOptionsImage As System.Windows.Forms.Button

End Class
