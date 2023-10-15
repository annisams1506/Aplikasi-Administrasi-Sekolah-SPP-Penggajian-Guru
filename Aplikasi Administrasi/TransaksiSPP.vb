Imports System.Data.Odbc
Public Class TransaksiSPP
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
    Sub bayarotomatis()
        Call koneksi()
        txtnobayar.MaxLength = 13
        CMD = New OdbcCommand("Select * from tbl_spp where No_Bayar in (select max(No_Bayar) from tbl_spp)", CONN)
        RD = CMD.ExecuteReader
        RD.Read()
        If Not RD.HasRows Then
            txtnobayar.Text = Format(Now, "yyMMdd") + "0001"
        Else
            If Microsoft.VisualBasic.Left(RD.Item("No_Bayar"), 6) = Format(Now, "yyMMdd") Then
                txtnobayar.Text = RD.Item("No_Bayar") + 1
            Else
                txtnobayar.Text = Format(Now, "yyMMdd") + "0001"
            End If
        End If
    End Sub
    Sub kondisipertama()
        txtnisn.Clear()
        txtnama.Text = ""
        cbkelas.SelectedItem = Nothing
        txttahun.Text = ""
        txtbiaya.Text = ""
        txttotaldibayar.Text = ""
        txttotalblmdibayar.Text = ""
        txtbulanblmdibayar.Text = ""
        txttotaldibayar.Text = ""
        txtbulandibayar.Text = ""
        txtlunasi.Text = ""
        btncetakpernomor.Enabled = False
        btnbayar.Enabled = False
        btncetakpertanggal.Enabled = False
        DataGridView1.Columns.Clear()
        txtnisn.Focus()
    End Sub
    Sub TOTALBELUMDIBAYAR()
        Call koneksi()
        CMD = New OdbcCommand("Select count(Keterangan) from tbl_spp where nisn='" & txtnisn.Text & "' and Keterangan='-'", CONN)
        RD = CMD.ExecuteReader
        RD.Read()
        txtbulanblmdibayar.Text = RD.Item(0) & "  Bulan"
        txttotalblmdibayar.Text = Val(Microsoft.VisualBasic.Str(txtbiaya.Text) * RD.Item(0))
        txttotalblmdibayar.Text = FormatNumber(txttotalblmdibayar.Text, 0)
    End Sub
    Sub TOTALTELAHDIBAYAR()
        Call koneksi()
        Dim HITUNG As Integer = 0
        For baris As Integer = 0 To DataGridView1.RowCount - 1
            HITUNG = HITUNG + DataGridView1.Rows(baris).Cells(5).Value
            txttotaldibayar.Text = Format(HITUNG, "###,###,###")
        Next
        Call koneksi()
        CMD = New OdbcCommand("Select count(Keterangan) from tbl_spp where nisn='" & txtnisn.Text & "' and Keterangan<>'-'", CONN)
        RD = CMD.ExecuteReader
        RD.Read()
        txtbulandibayar.Text = RD.Item(0) & "  Bulan"
    End Sub
    Sub TAMPILKELAS()
        Call koneksi()
        Dim str As String
        str = "Select Nama_Kelas from tbl_kelas"
        CMD = New OdbcCommand(str, CONN)
        RD = CMD.ExecuteReader
        If RD.HasRows Then
            Do While RD.Read
                cbkelas.Items.Add(RD("Nama_Kelas"))
            Loop
        End If
    End Sub
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Call TAMPILKELAS()
        Call bayarotomatis()
        Call bayarotomatis()
        txtnisn.Focus()
        btnbayar.Enabled = False
        btncetakpernomor.Enabled = False
        btncetakpertanggal.Enabled = False
        Me.Top = 100
        Call bayarotomatis()

    End Sub

    Private Sub txtnisn_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnisn.KeyPress
        txtnisn.MaxLength = 10
        If e.KeyChar = Chr(13) Then
            Call koneksi()
            CMD = New OdbcCommand("Select * from tbl_siswa where Nis='" & txtnisn.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If RD.HasRows Then
                txtnama.Text = RD.Item("Nama_Siswa")
                cbkelas.Text = RD.Item("Kelas")
                txttahun.Text = RD.Item("Tahun_Ajaran")
                txtbiaya.Text = RD.Item("Biaya")
                txtbiaya.Text = FormatNumber(txtbiaya.Text, 0)
            Else
                MsgBox("NIS TIDAK TERDAFTAR")
                Call kondisipertama()
                DataSiswa.Show()
            End If
        End If
        Call koneksi()
        CMD = New OdbcCommand("Select * from tbl_spp where nisn='" & txtnisn.Text & "'", CONN)
        RD = CMD.ExecuteReader
        RD.Read()
        If RD.HasRows Then
            Call koneksi()
            DS = New DataSet
            DA = New OdbcDataAdapter("Select * from tbl_spp where nisn='" & txtnisn.Text & "' ORDER BY 3", CONN)
            DA.Fill(DS, "tbl_spp")
            Dim gridview As New DataView(DS.Tables("tbl_spp"))
            DataGridView1.DataSource = gridview
            DataGridView1.Columns(0).Visible = True
            DataGridView1.ReadOnly = True
            DataGridView1.Columns(1).Width = 135
            DataGridView1.Columns(2).Width = 135
            DataGridView1.Columns(5).Width = 135
            DataGridView1.Columns(5).DefaultCellStyle.Format = "###,###,###"
            DataGridView1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns(4).Width = 135
            DataGridView1.Columns(7).Visible = False
            DataGridView1.Columns(6).Width = 138
            DataGridView1.Columns(0).Width = 135
            Me.DataGridView1.Columns(3).Width = 135
            Me.DataGridView1.Columns(0).HeaderText = "NISN"
            Me.DataGridView1.Columns(1).HeaderText = "Jatuh Tempo"
            Me.DataGridView1.Columns(2).HeaderText = "Bulan"
            Me.DataGridView1.Columns(3).HeaderText = "Nomor Bayar"
            Me.DataGridView1.Columns(4).HeaderText = "Tanggal Bayar"
            Me.DataGridView1.Columns(5).HeaderText = "Jumlah"
            Me.DataGridView1.Columns(6).HeaderText = "Keterangan"
            Call TOTALTELAHDIBAYAR()
            Call TOTALBELUMDIBAYAR()
            Call bayarotomatis()
        End If
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled = True
    End Sub
    Private Sub DataGridView1_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.RowEnter
        Call koneksi()
        txtlunasi.Text = DataGridView1.Rows(e.RowIndex).Cells(6).Value
        If txtlunasi.Text = "-" Then
            btnbayar.Enabled = True
            btncetakpernomor.Enabled = False
            btncetakpertanggal.Enabled = False
            Call bayarotomatis()

        ElseIf txtlunasi.Text = "LUNAS" Or txtlunasi.Text = "LUNAS" Then

            btnbayar.Enabled = False
            btncetakpernomor.Enabled = True
            btncetakpertanggal.Enabled = True
            Call bayarotomatis()
        End If
    End Sub

    Private Sub btnbayar_Click(sender As Object, e As EventArgs) Handles btnbayar.Click
        Try
            Call koneksi()
            Dim KONDISIBAYAR As String
            If DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).Cells(1).Value > DateTimePicker1.Value Then
                KONDISIBAYAR = "LUNAS"
            Else
                KONDISIBAYAR = "LUNAS"
            End If

            Call koneksi()
            Dim bayar As String = "Update tbl_spp set No_Bayar='" & txtnobayar.Text & "', Tgl_Bayar='" & Format(DateTimePicker1.Value, "yyyy/MM/dd") & "', Jumlah='" & Microsoft.VisualBasic.Str(txtbiaya.Text) & "' ,Keterangan='" & KONDISIBAYAR & "' ,Kode_Petugas='" & MenuUtama.ToolStripStatusLabel1.Text & "' where No_Bayar='" & DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).Cells(3).Value & "'"
            CMD = New OdbcCommand(bayar, CONN)
            CMD.ExecuteNonQuery()

            Call koneksi()
            DA = New OdbcDataAdapter("Select * from tbl_spp where nisn='" & txtnisn.Text & "' ORDER BY 3", CONN)
            DS = New DataSet
            DA.Fill(DS)
            DataGridView1.DataSource = DS.Tables(0)
            DataGridView1.Columns(0).Visible = True
            DataGridView1.ReadOnly = True
            DataGridView1.Columns(1).Width = 135
            DataGridView1.Columns(2).Width = 135
            DataGridView1.Columns(5).Width = 135
            DataGridView1.Columns(5).DefaultCellStyle.Format = "###,###,###"
            DataGridView1.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            DataGridView1.Columns(4).Width = 135
            DataGridView1.Columns(7).Visible = False
            DataGridView1.Columns(6).Width = 138
            DataGridView1.Columns(0).Width = 135
            Me.DataGridView1.Columns(3).Width = 135
            Me.DataGridView1.Columns(0).HeaderText = "NISN"
            Me.DataGridView1.Columns(1).HeaderText = "Jatuh Tempo"
            Me.DataGridView1.Columns(2).HeaderText = "Bulan"
            Me.DataGridView1.Columns(3).HeaderText = "Nomor Bayar"
            Me.DataGridView1.Columns(4).HeaderText = "Tanggal Bayar"
            Me.DataGridView1.Columns(5).HeaderText = "Jumlah"
            Me.DataGridView1.Columns(6).HeaderText = "Keterangan"
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
        Call TOTALTELAHDIBAYAR()
        Call TOTALBELUMDIBAYAR()
        Call bayarotomatis()
        txtnisn.Focus()
    End Sub

    Private Sub btnbatal_Click(sender As Object, e As EventArgs) Handles btnbatal.Click
        Call kondisipertama()
        Call bayarotomatis()

    End Sub

    Private Sub btntutup_Click(sender As Object, e As EventArgs) Handles btntutup.Click
        Me.Close()
        MenuUtama.Show()
    End Sub

    Private Sub btncetakpernomor_Click(sender As Object, e As EventArgs) Handles btncetakpernomor.Click
        AxCrystalReport1.ReportFileName = Nothing
        AxCrystalReport1.SelectionFormula = "{tbl_spp.No_Bayar}='" & DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).Cells(3).Value & "'"
        AxCrystalReport1.ReportFileName = "reportspp.rpt"
        AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport1.RetrieveDataFiles()
        AxCrystalReport1.Action = 1
    End Sub

    Private Sub btncetakpertanggal_Click(sender As Object, e As EventArgs) Handles btncetakpertanggal.Click
        AxCrystalReport2.ReportFileName = Nothing
        AxCrystalReport2.SelectionFormula = "{tbl_spp.nisn}='" & txtnisn.Text & "' and totext({tbl_spp.Tgl_Bayar})='" & DataGridView1.Rows(DataGridView1.CurrentCell.RowIndex).Cells(4).Value & "'"
        AxCrystalReport2.ReportFileName = "reportspp.rpt"
        AxCrystalReport2.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport2.RetrieveDataFiles()
        AxCrystalReport2.Action = 1
    End Sub

    Private Sub btncetaktotalpembayaran_Click(sender As Object, e As EventArgs) Handles btncetaktotalpembayaran.Click
        AxCrystalReport4.ReportFileName = Nothing
        AxCrystalReport4.SelectionFormula = "{tbl_spp.nisn}='" & txtnisn.Text & "' and {tbl_spp.Jumlah}> 0 "
        AxCrystalReport4.ReportFileName = "reportspp.rpt"
        AxCrystalReport4.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport4.RetrieveDataFiles()
        AxCrystalReport4.Action = 1
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AxCrystalReport3.ReportFileName = Nothing
        AxCrystalReport3.SelectionFormula = "{tbl_spp.nisn}='" & txtnisn.Text & "' and {tbl_spp.Jumlah}= 0 "
        AxCrystalReport3.ReportFileName = "reportspp.rpt"
        AxCrystalReport3.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport3.RetrieveDataFiles()
        AxCrystalReport3.Action = 1
    End Sub

    Private Sub btncetaksemua_Click(sender As Object, e As EventArgs) Handles btncetaksemua.Click
        AxCrystalReport5.ReportFileName = Nothing
        AxCrystalReport5.SelectionFormula = "{tbl_spp.nisn}='" & txtnisn.Text & "'"
        AxCrystalReport5.ReportFileName = "reportspp.rpt"
        AxCrystalReport5.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport5.RetrieveDataFiles()
        AxCrystalReport5.Action = 1
    End Sub
End Class