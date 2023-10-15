Imports System.Data.Odbc
Public Class LaporanGaji
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

    Sub TAMPILKAN()
        Call koneksi()
        Call koneksi()
        DA = New OdbcDataAdapter("Select tbl_guru.Nip,Nama_Guru,Tanggal,Nomor_Slip from tbl_gaji,tbl_guru where tbl_guru.Nip=tbl_gaji.nip", CONN)
        DS = New DataSet
        DA.Fill(DS)
        DS.Clear()
        DA.Fill(DS, "tbl_guru")
        dgv.DataSource = (DS.Tables("tbl_guru"))

        dgv.ReadOnly = True
        dgv.Columns(0).Width = 75
        dgv.Columns(1).Width = 130
        dgv.Columns(2).Width = 75
        dgv.Columns(3).Width = 75
    End Sub

    Private Sub dgv_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.RowEnter
        TextBox3.Text = dgv.Rows(e.RowIndex).Cells(0).Value
        TextBox4.Text = dgv.Rows(e.RowIndex).Cells(1).Value
        TextBox5.Text = dgv.Rows(e.RowIndex).Cells(2).Value
        TextBox6.Text = dgv.Rows(e.RowIndex).Cells(3).Value
    End Sub


    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DateTimePicker2.Text = Format(Now, "MMMM/yyyy")

        Call TAMPILKAN()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox3.Text = "" Then
            MsgBox("ISI")
        Else
            AxCrystalReport2.ReportFileName = Nothing
            AxCrystalReport2.SelectionFormula = "totext({tbl_guru.Nip})='" & TextBox3.Text & "'"
            AxCrystalReport2.ReportFileName = "Report3.rpt"
            AxCrystalReport2.WindowState = Crystal.WindowStateConstants.crptMaximized
            AxCrystalReport2.RetrieveDataFiles()
            AxCrystalReport2.Action = 1
        End If
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        AxCrystalReport3.SelectionFormula = "Month({tbl_gaji.Tanggal})=(" & Month(DateTimePicker2.Text) & ") and Year({tbl_gaji.Tanggal})=(" & Year(DateTimePicker2.Text) & ")"
        AxCrystalReport3.ReportFileName = "reportgajibulanan.rpt"
        AxCrystalReport3.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport3.RetrieveDataFiles()
        AxCrystalReport3.Action = 1
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PictureBox9_Click(sender As Object, e As EventArgs) Handles PictureBox9.Click
        MenuUtama.Show()
        Me.Close()
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub
End Class