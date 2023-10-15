Imports System.Data.Odbc
Public Class Tahunajaran
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
    Sub KOSONGKANDATA()
        txtkode.Text = ""
        txtajaran.Text = ""
    End Sub
    Sub TAMPILGRID()
        Call koneksi()
        DA = New OdbcDataAdapter("Select * from tbl_tahunajaran", CONN)
        DS = New DataSet
        DA.Fill(DS, "tbl_tahunajaran")
        DataGridView1.DataSource = (DS.Tables("tbl_tahunajaran"))
        DataGridView1.ReadOnly = True
        DataGridView1.Columns(0).Width = 50
        DataGridView1.Columns(1).Width = 120
        Me.DataGridView1.Columns(0).HeaderText = "Kode"
        Me.DataGridView1.Columns(1).HeaderText = "Tahun Ajaran"
    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If txtkode.Text = "" Or txtajaran.Text = "" Then
            MsgBox("ISI SEMUA DATA")
        Else
            Call koneksi()
            Dim simpan As String = "Insert tbl_tahunajaran values ('" & txtkode.Text & "','" & txtajaran.Text & "')"
            CMD = New OdbcCommand(simpan, CONN)
            CMD.ExecuteNonQuery()
            MsgBox("INPUT DATA BERHASIL")
            Call TAMPILGRID()
            Call KOSONGKANDATA()
            txtkode.Focus()
        End If
    End Sub

    Private Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click
        Call koneksi()
        Dim edit As String = "Update tbl_tahunajaran set Tahun_Ajaran='" & txtajaran.Text & "' where Kode_Ajaran='" & txtkode.Text & "'"
        CMD = New OdbcCommand(edit, CONN)
        CMD.ExecuteNonQuery()
        MsgBox("DATA BERHASIL DI UBAH")
        Call TAMPILGRID()
        Call KOSONGKANDATA()
    End Sub

    Private Sub txtdelete_Click(sender As Object, e As EventArgs) Handles txtdelete.Click
        If txtkode.Text = "" Then
            MsgBox("PILIH TAHUN AJARAN YANG INGIN DIHAPUS")
        Else
            If MessageBox.Show("Yakin akan dihapus...", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Call koneksi()
                Dim hapus As String = "Delete from tbl_tahunajaran where Kode_Ajaran='" & txtkode.Text & "'"
                CMD = New OdbcCommand(hapus, CONN)
                CMD.ExecuteNonQuery()
                Call TAMPILGRID()
                Call KOSONGKANDATA()
            End If
        End If
    End Sub

    Private Sub btnclose_Click(sender As Object, e As EventArgs) Handles btnclose.Click
        Me.Close()
        MenuUtama.Show()
    End Sub

    Private Sub Tahunajaran_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call TAMPILGRID()
    End Sub
    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        txtkode.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        txtajaran.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        txtajaran.Clear()
        txtkode.Clear()
    End Sub
End Class