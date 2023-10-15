Imports System.Data.Odbc
Public Class Petugas
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
    Sub tampilgrid()
        Call koneksi()
        DA = New OdbcDataAdapter("Select * from tbl_petugas", CONN)
        DS = New DataSet
        DA.Fill(DS, "tbl_petugas")
        DataGridView1.DataSource = DS.Tables("tbl_petugas")
        DataGridView1.ReadOnly = True
        DataGridView1.Columns(0).Width = 100
        DataGridView1.Columns(1).Width = 250
        DataGridView1.Columns(2).Visible = False
        DataGridView1.Columns(3).Width = 130
        DataGridView1.Columns(4).Visible = False
        Me.DataGridView1.Columns(0).HeaderText = "Kode Petugas"
        Me.DataGridView1.Columns(1).HeaderText = "Nama Petugas"
        Me.DataGridView1.Columns(3).HeaderText = "Status"
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbstatus.Items.Add("Administrator")
        cbstatus.Items.Add("Operator")
        cbstatus.AutoCompleteSource = AutoCompleteSource.ListItems
        cbstatus.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        Call koneksi()
        Call tampilgrid()

    End Sub
    Sub kosongkan()
        txtkode.Text = ""
        txtnama.Text = ""
        txtpw.Text = ""
        textbox4.Text = ""
        cbstatus.SelectedItem = Nothing
        PictureBox1.ImageLocation = Nothing
    End Sub
    Private Sub txtkode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtkode.KeyPress
        txtkode.MaxLength = 10
        If e.KeyChar = Chr(13) Then
            Call koneksi()
            CMD = New OdbcCommand("Select * from tbl_petugas where Kode_Petugas='" & txtkode.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If RD.HasRows Then
                txtnama.Text = RD.Item("Nama_Petugas")
                txtpw.Text = RD.Item("Password")
                cbstatus.Text = RD.Item("Status")
                textbox4.Text = RD.Item("Foto")
                PictureBox1.ImageLocation = Replace((RD("Foto")), ";", "/")
                txtnama.Focus()
            End If
        End If
    End Sub

    Private Sub txtnama_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnama.KeyPress
        txtnama.MaxLength = 50
        If e.KeyChar = Chr(13) Then
            txtpw.Focus()

        End If
    End Sub
    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If txtkode.Text = "" Or txtnama.Text = "" Or txtpw.Text = "" Or cbstatus.Text = "" Then
            MsgBox("SILAHKAN ISI SEMUA DATA")
        Else
            Call koneksi()
            Dim simpan As String = "insert tbl_petugas values ('" & txtkode.Text & "','" & txtnama.Text & "','" & txtpw.Text & "','" & cbstatus.Text & "','" & textbox4.Text & "')"
            CMD = New OdbcCommand(simpan, CONN)
            CMD.ExecuteNonQuery()
            MsgBox("Input Data Berhasil")
            Call tampilgrid()
            Call kosongkan()

        End If
    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        If txtkode.Text = "" Then
            MsgBox("SILAHKAN PILIH DATA YANG AKAN DIHAPUS DENGAN MEMASUKKAN KODE PETUGAS LALU TEKAN ENTER")
        Else
            If MessageBox.Show("Yakin akan dihapus...", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Call koneksi()
                Dim hapus As String = "delete from tbl_petugas where Kode_Petugas='" & txtkode.Text & "'"
                CMD = New OdbcCommand(hapus, CONN)
                CMD.ExecuteNonQuery()
                Call tampilgrid()
                Call kosongkan()

            End If
        End If
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        txtkode.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        txtkode.Focus()
    End Sub

    Private Sub btlbatal_Click(sender As Object, e As EventArgs) Handles btlbatal.Click
        Call kosongkan()
    End Sub

    Private Sub btltutup_Click(sender As Object, e As EventArgs) Handles btltutup.Click
        Me.Close()
        MenuUtama.Show()
    End Sub

    Private Sub btngambar_Click(sender As Object, e As EventArgs) Handles btngambar.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String

        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "D:\"
        fd.Filter = "Image File (*.png*)|*.png*|Image File (*.jpg)|*.jpg*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
            textbox4.Text = strFileName
        End If
        PictureBox1.ImageLocation = textbox4.Text


    End Sub
    Private Sub BTNEDIT_Click(sender As Object, e As EventArgs) Handles BTNEDIT.Click
        Call koneksi()
        Dim EDIT As String = "Update tbl_petugas set Nama_Petugas='" & txtnama.Text & "' ,Password='" & txtpw.Text & "' ,Status='" & cbstatus.Text & "' ,Foto='" & textbox4.Text & "' where Kode_Petugas='" & txtkode.Text & "'"
        CMD = New OdbcCommand(EDIT, CONN)
        CMD.ExecuteNonQuery()
        MsgBox("Data Berhasil Di Edit")
        Call tampilgrid()
        Call kosongkan()

    End Sub

End Class
