Imports System.Data.Odbc
Public Class LaporanSiswa
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
        ComboBox2.Items.Clear()
        Do While RD.Read
            ComboBox1.Items.Add(RD.Item("Kelas"))
            ComboBox2.Items.Add(RD.Item("Kelas"))
        Loop
    End Sub
    Sub tampiltahun()
        Call koneksi()
        CMD = New OdbcCommand("Select distinct Tahun_Ajaran from tbl_siswa", CONN)
        RD = CMD.ExecuteReader
        ComboBox3.Items.Clear()
        ComboBox4.Items.Clear()
        Do While RD.Read
            ComboBox3.Items.Add(RD.Item("Tahun_Ajaran"))
            ComboBox4.Items.Add(RD.Item("Tahun_Ajaran"))
        Loop
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AxCrystalReport1.ReportFileName = "reportsiswa.rpt"
        AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport1.RetrieveDataFiles()
        AxCrystalReport1.Action = 1
    End Sub
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        AxCrystalReport2.SelectionFormula = "{tbl_siswa.Kelas}='" & ComboBox1.Text & "'"
        AxCrystalReport2.ReportFileName = "reportsiswa.rpt"
        AxCrystalReport2.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport2.RetrieveDataFiles()
        AxCrystalReport2.Action = 1
    End Sub
    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        AxCrystalReport3.SelectionFormula = "{tbl_siswa.Tahun_Ajaran}='" & ComboBox3.Text & "'"
        AxCrystalReport3.ReportFileName = "reportsiswa.rpt"
        AxCrystalReport3.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport3.RetrieveDataFiles()
        AxCrystalReport3.Action = 1
    End Sub
    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        AxCrystalReport4.SelectionFormula = "{tbl_siswa.Kelas}='" & ComboBox2.Text & "' and {tbl_siswa.Tahun_Ajaran}='" & ComboBox4.Text & "'"
        AxCrystalReport4.ReportFileName = "reportsiswa.rpt"
        AxCrystalReport4.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport4.RetrieveDataFiles()
        AxCrystalReport4.Action = 1
    End Sub
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call tampilkelas()
        Call tampiltahun()
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox1.Text = UCase(TextBox1.Text)
            If TextBox1.Text <> "" Then
                AxCrystalReport5.SelectionFormula = "{tbl_siswa.Nama_Siswa} like '*" & TextBox1.Text & "*'"
                AxCrystalReport5.ReportFileName = "siswanama.rpt"
                AxCrystalReport5.WindowState = Crystal.WindowStateConstants.crptMaximized
                AxCrystalReport5.RetrieveDataFiles()
                AxCrystalReport5.Action = 1
            Else
                AxCrystalReport5.ReportFileName = "siswanama.rpt"
                AxCrystalReport5.WindowState = Crystal.WindowStateConstants.crptMaximized
                AxCrystalReport5.RetrieveDataFiles()
                AxCrystalReport5.Action = 1
            End If
        End If
    End Sub
    Private Sub PictureBox9_Click(sender As Object, e As EventArgs) Handles PictureBox9.Click
        MenuUtama.Show()
        Me.Close()
    End Sub
End Class