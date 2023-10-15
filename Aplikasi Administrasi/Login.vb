Imports System.Data.Odbc
Public Class Login
    Dim CONN As OdbcConnection
    Dim CMD As OdbcCommand
    Dim DS As DataSet
    Dim DA As OdbcDataAdapter
    Dim RD As OdbcDataReader
    Dim lokasidata As String
    Sub koneksi()
        lokasidata = "Driver={MySQL ODBC 3.51 Driver};Database=adm;server=localhost;uid=root"
        CONN = New OdbcConnection(lokasidata)
        If CONN.State = ConnectionState.Closed Then
            CONN.Open()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call koneksi()
        CMD = New OdbcCommand("Select * from tbl_petugas where Nama_Petugas='" & TextBox1.Text & "' and Password='" & TextBox2.Text & "'", CONN)
        RD = CMD.ExecuteReader
        RD.Read()
        If Not RD.HasRows Then
            Call koneksi()
            MsgBox("Login Gagal")
            TextBox1.Text = ""
            TextBox2.Text = ""
            TextBox1.Focus()
        Else
            Me.Visible = False
            MenuUtama.Show()
            MenuUtama.ToolStripStatusLabel1.Text = RD.Item("Kode_Petugas")
            MenuUtama.Panel2.Text = RD.Item("Nama_Petugas")
            MenuUtama.Panel3.Text = RD.Item("status")
            If MenuUtama.Panel3.Text = "Administrator" Or MenuUtama.Panel3.Text = "Pemilik" Then 'Apabila masuk sebagai Admin/User 
                MenuUtama.PictureBox2.Enabled = True
                MenuUtama.PictureBox6.Enabled = True
                'MenuUtama.GantiPasswordToolStripMenuItem.Enabled = True

            Else
                'MenuUtama.UserToolStripMenuItem.Enabled = False
                'MenuUtama.GuruToolStripMenuItem.Enabled = False
                MenuUtama.PictureBox6.Enabled = False
                'MenuUtama.GuruToolStripMenuItem1.Enabled = False
                MenuUtama.PictureBox2.Enabled = False
                'MenuUtama.GantiPasswordToolStripMenuItem.Enabled = False
            End If
        End If
    End Sub


    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox2.Focus()
        End If
    End Sub
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            Button1.Focus()
        End If
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub
End Class