Imports System.Data.Odbc
Public Class LaporanGuru
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
    Sub tampiljabatan()
        Call koneksi()
        CMD = New OdbcCommand("Select distinct Kode_Jabatan from tbl_guru", CONN)
        RD = CMD.ExecuteReader
        ComboBox1.Items.Clear()
        Do While RD.Read
            ComboBox1.Items.Add(RD.Item("Kode_Jabatan"))
        Loop
    End Sub
    Sub tampilnip()
        Call koneksi()
        CMD = New OdbcCommand("Select distinct Nip from tbl_guru", CONN)
        RD = CMD.ExecuteReader
        ComboBox2.Items.Clear()
        Do While RD.Read
            ComboBox2.Items.Add(RD.Item("Nip"))
        Loop
    End Sub
    Sub tampilnama()
        Call koneksi()
        CMD = New OdbcCommand("Select distinct Nama_Guru from tbl_guru", CONN)
        RD = CMD.ExecuteReader
        ComboBox3.Items.Clear()
        Do While RD.Read
            ComboBox3.Items.Add(RD.Item("Nama_Guru"))
        Loop
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AxCrystalReport1.ReportFileName = "reportguru.rpt"
        AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport1.RetrieveDataFiles()
        AxCrystalReport1.Action = 1
    End Sub

    Private Sub LaporanGuru_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call tampiljabatan()
        Call tampilnama()
        Call tampilnip()
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        AxCrystalReport2.SelectionFormula = "{tbl_guru.Kode_Jabatan}='" & ComboBox1.Text & "'"
        AxCrystalReport2.ReportFileName = "reportguru.rpt"
        AxCrystalReport2.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport2.RetrieveDataFiles()
        AxCrystalReport2.Action = 1
    End Sub
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        AxCrystalReport5.SelectionFormula = "{tbl_guru.Nip}='" & ComboBox2.Text & "'"
        AxCrystalReport5.ReportFileName = "guru.rpt"
        AxCrystalReport5.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport5.RetrieveDataFiles()
        AxCrystalReport5.Action = 1
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        AxCrystalReport3.SelectionFormula = "{tbl_guru.Nama_Guru}='" & ComboBox3.Text & "'"
        AxCrystalReport3.ReportFileName = "guru.rpt"
        AxCrystalReport3.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport3.RetrieveDataFiles()
        AxCrystalReport3.Action = 1
    End Sub

    Private Sub PictureBox9_Click(sender As Object, e As EventArgs) Handles PictureBox9.Click
        MenuUtama.Show()
        Me.Close()
    End Sub
End Class