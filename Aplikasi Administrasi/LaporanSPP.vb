Imports System.Data.Odbc
Public Class LaporanSPP
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
    Sub tampilkelas()
        Call koneksi()
        CMD = New OdbcCommand("Select distinct Kelas from tbl_siswa", CONN)
        RD = CMD.ExecuteReader
        ComboBox1.Items.Clear()
        Do While RD.Read
            ComboBox1.Items.Add(RD.Item("Kelas"))
            ComboBox2.Items.Add(RD.Item("Kelas"))
        Loop
    End Sub

    Private Sub LaporanSPP_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call tampilkelas()

        DateTimePicker2.Text = Format(Now, "MMMM/yyyy")
        DateTimePicker3.Text = Format(Now, "MMMM/yyyy")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AxCrystalReport1.SelectionFormula = "totext({tbl_spp.Tgl_Bayar})='" & Format(DateTimePicker1.Value, "dd/MM/yyyy") & "' and {tbl_spp.Keterangan}<>'-'"
        AxCrystalReport1.ReportFileName = "sppharian.rpt"
        AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport1.RetrieveDataFiles()
        AxCrystalReport1.Action = 1
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        AxCrystalReport2.SelectionFormula = "{tbl_spp.Keterangan}<>'-' and month({tbl_spp.Tgl_Bayar} )= (" & Month(DateTimePicker2.Text) & ") and year ({tbl_spp.Tgl_Bayar}) = (" & Year(DateTimePicker2.Text) & ")"
        AxCrystalReport2.ReportFileName = "sppbulanan.rpt"
        AxCrystalReport2.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport2.RetrieveDataFiles()
        AxCrystalReport2.Action = 1
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        AxCrystalReport3.SelectionFormula = "{tbl_siswa.Kelas}='" & ComboBox1.Text & "' and {tbl_spp.Keterangan}<>'-'"
        AxCrystalReport3.ReportFileName = "sppkelas.rpt"
        AxCrystalReport3.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport3.RetrieveDataFiles()
        AxCrystalReport3.Action = 1
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        AxCrystalReport4.SelectionFormula = "{tbl_spp.Keterangan}<>'-' and {tbl_siswa.Kelas}='" & ComboBox2.Text & "' and month({tbl_spp.Tgl_Bayar} )= (" & Month(DateTimePicker3.Text) & ") and year ({tbl_spp.Tgl_Bayar}) = (" & Year(DateTimePicker3.Text) & ")"
        AxCrystalReport4.ReportFileName = "sppnama.rpt"
        AxCrystalReport4.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport4.RetrieveDataFiles()
        AxCrystalReport4.Action = 1
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub PictureBox9_Click(sender As Object, e As EventArgs) Handles PictureBox9.Click
        MenuUtama.Show()
        Me.Close()
    End Sub
End Class