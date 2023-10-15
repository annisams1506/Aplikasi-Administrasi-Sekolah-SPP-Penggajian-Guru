Imports System.Data.Odbc
Public Class Kelas
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
    Sub kosongkandata()
        txtkelas.Text = ""
        txtkode.Text = ""
    End Sub
    Sub TAMPILGRID()
        Call koneksi()
        DA = New OdbcDataAdapter("Select * from tbl_kelas", CONN)
        DS = New DataSet
        DA.Fill(DS, "tbl_kelas")
        DataGridView1.DataSource = (DS.Tables("tbl_kelas"))
        DataGridView1.ReadOnly = True
        DataGridView1.Columns(0).Width = 50
        DataGridView1.Columns(1).Width = 120
        Me.DataGridView1.Columns(0).HeaderText = "Kode Kelas"
        Me.DataGridView1.Columns(1).HeaderText = "Nama Kelas"
    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If txtkode.Text = "" Or txtkelas.Text = "" Then
            MsgBox("ISI SEMUA DATA")
        Else
            Call koneksi()
            Dim simpan As String = "Insert tbl_kelas values ('" & txtkode.Text & "','" & txtkelas.Text & "')"
            CMD = New OdbcCommand(simpan, CONN)
            CMD.ExecuteNonQuery()
            MsgBox("INPUT DATA BERHASIL")
            Call TAMPILGRID()
            Call kosongkandata()
            txtkode.Focus()
        End If
    End Sub

    Private Sub Kelas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call TAMPILGRID()
        Dim jmldata As Integer
        jmldata = DataGridView1.RowCount

    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        txtkode.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        txtkelas.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value
        txtkelas.Focus()
    End Sub

    Private Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click
        Call koneksi()
        Dim edit As String = "Update tbl_kelas set Nama_Kelas='" & txtkelas.Text & "' where Id_Kelas='" & txtkode.Text & "'"
        CMD = New OdbcCommand(edit, CONN)
        CMD.ExecuteNonQuery()
        MsgBox("DATA BERHASIL DI UBAH")
        Call TAMPILGRID()
        Call kosongkandata()
    End Sub

    Private Sub txtdelete_Click(sender As Object, e As EventArgs) Handles txtdelete.Click
        If txtkode.Text = "" Then
            MsgBox("PILIH KELAS YANG INGIN DIHAPUS")
        Else
            If MessageBox.Show("Yakin akan dihapus...", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Call koneksi()
                Dim hapus As String = "Delete from tbl_kelas where Id_Kelas='" & txtkode.Text & "'"
                CMD = New OdbcCommand(hapus, CONN)
                CMD.ExecuteNonQuery()
                Call TAMPILGRID()
                Call kosongkandata()
            End If
        End If
    End Sub

    Private Sub btnclose_Click(sender As Object, e As EventArgs) Handles btnclose.Click
        Me.Close()
        MenuUtama.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        txtkode.Clear()
        txtkelas.Clear()
    End Sub
End Class