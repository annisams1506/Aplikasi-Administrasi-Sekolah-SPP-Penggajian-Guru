Imports System.Data.Odbc
Public Class DataSiswa
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
    Sub TAMPILGRID()
        Call koneksi()
        DA = New OdbcDataAdapter("Select *from tbl_siswa", CONN)
        DS = New DataSet
        DA.Fill(DS)
        DataGridView1.DataSource = DS.Tables(0)
        DataGridView1.Columns(0).Width = 75
        DataGridView1.Columns(1).Width = 150
        DataGridView1.Columns(2).Visible = False
        DataGridView1.Columns(3).Visible = False
        DataGridView1.Columns(4).Visible = False
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        TransaksiSPP.txtnisn.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        Me.Close()

    End Sub

    Private Sub DataSiswa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call TAMPILGRID()
    End Sub
End Class