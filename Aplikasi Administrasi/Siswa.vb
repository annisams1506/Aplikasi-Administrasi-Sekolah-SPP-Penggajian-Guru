Imports System.Data.Odbc
Public Class Siswa
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
    Sub nisnotomatis()
        Call koneksi()
        CMD = New OdbcCommand("Select * from tbl_siswa where Nis in (select max(Nis) from tbl_siswa)", CONN)
        RD = CMD.ExecuteReader
        RD.Read()
        If Not RD.HasRows Then
            txtnisn.Text = Format(Now, "yyMMdd") + "001"
        Else
            If Microsoft.VisualBasic.Left(RD.Item("Nis"), 6) = Format(Now, "yyMMdd") Then
                txtnisn.Text = RD.Item("Nis") + 1
            Else
                txtnisn.Text = Format(Now, "yyMMdd") + "001"
            End If
        End If
    End Sub
    Sub BERSIH()
        txtnama.Text = ""
        txtnisn.Text = ""
        txtbiaya.Text = Nothing
        txttahun.SelectedItem=Nothing 
        cbkelas.SelectedItem = Nothing
        Call nisnotomatis()
        txtnama.Focus()
    End Sub
   
    Sub databaru()
        txtnama.Clear()
        txtbiaya.Clear()
        txttahun.SelectedItem = Nothing
        cbkelas.SelectedItem = Nothing
        txtnama.Focus()
    End Sub
    Sub tampilgrid()
        DA = New OdbcDataAdapter("Select * from tbl_siswa", CONN)
        DS = New DataSet
        DA.Fill(DS, "tbl_siswa")
        DataGridView1.DataSource = (DS.Tables("tbl_siswa"))
        DataGridView1.ReadOnly = True
        DataGridView1.Columns(0).Width = 90
        DataGridView1.Columns(1).Width = 120
        DataGridView1.Columns(2).Width = 60
        DataGridView1.Columns(3).Width = 100
        DataGridView1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Columns(4).Width = 100
        DataGridView1.Columns(4).DefaultCellStyle.Format = "###,###,###"
        DataGridView1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Me.DataGridView1.Columns(0).HeaderText = "NIS"
        Me.DataGridView1.Columns(1).HeaderText = "Nama Siswa"
        Me.DataGridView1.Columns(2).HeaderText = "Kelas"
        Me.DataGridView1.Columns(3).HeaderText = "Tahun Ajaran"
        Me.DataGridView1.Columns(4).HeaderText = "Biaya"
    End Sub
    Sub tampilkelas()
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
    Private Sub txtnama_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnama.KeyPress
        txtnama.MaxLength = 50
        If e.KeyChar = Chr(13) Then
            txtnama.Text = UCase(txtnama.Text)
        End If
    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If txtnisn.Text = "" Or txtnama.Text = "" Or txttahun.Text = "" Or cbkelas.Text = "" Or txtbiaya.Text = "" Then
            MsgBox("HARAP ISI SEMUA DATA")
        Else
            Call koneksi()
            Dim simpan As String = "Insert tbl_siswa values ('" & txtnisn.Text & "','" & txtnama.Text & "','" & cbkelas.Text & "','" & txttahun.Text & "','" & txtbiaya.Text & "')"
            CMD = New OdbcCommand(simpan, CONN)
            CMD.ExecuteNonQuery()

            Call koneksi()
            CMD = New OdbcCommand("Select * from tbl_spp where nisn='" & txtnisn.Text & "'", CONN)
            RD = CMD.ExecuteReader
            For i As Integer = 1 To 12
                Dim tempO As Date = DateAdd(DateInterval.Month, i - 1, DateValue(txttempo.Text))
                Call koneksi()
                Dim hasil As String = txtnisn.Text
                Dim simpandetail2 As String = "Insert into tbl_spp values('" & txtnisn.Text & "','" & Format(tempO, "dd/MM/yyyy") & "','" & Format(tempO, "MMMM") + " " + Format(tempO, "yyyy") & "','" & txtnisn.Text & i & "','" & Format(Now, "yyyy/MM/dd") & "',0,'-','-')"
                CMD = New OdbcCommand(simpandetail2, CONN)
                CMD.ExecuteNonQuery()

            Next
            CMD = New OdbcCommand("Select * from tbl_spp order by 1,2", CONN)
            RD = CMD.ExecuteReader
            RD.Read()

        End If

        Call BERSIH()
        Call tampilgrid()
        Call tampilkelas()
        Call nisnotomatis()
    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        If txtnisn.Text = "" Then
            MsgBox("NIS HARUS DIISI")
            txtnisn.Focus()
            Exit Sub
        Else
            If MessageBox.Show("Yakin akan dihapus...", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Call koneksi()
                Dim hapus As String = "delete from tbl_siswa where Nis='" & txtnisn.Text & "'"
                CMD = New OdbcCommand(hapus, CONN)
                CMD.ExecuteNonQuery()

                Dim hapusnisspp As String = "delete from tbl_spp where nisn='" & txtnisn.Text & "'"
                CMD = New OdbcCommand(hapusnisspp, CONN)
                CMD.ExecuteNonQuery()
                Call BERSIH()
                '   Call biayaajaran()
                Call tampilgrid()
                Call nisnotomatis()
                Call tampilkelas()
            Else
                Call BERSIH()
            End If
        End If
    End Sub

    Private Sub btnbatal_Click(sender As Object, e As EventArgs) Handles btnbatal.Click
        Call BERSIH()
    End Sub

    Private Sub btntutup_Click(sender As Object, e As EventArgs) Handles btntutup.Click
        Me.Close()
        MenuUtama.Show()
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        txtnisn.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        txtnama.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value
        cbkelas.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value
        txttahun.Text = DataGridView1.Rows(e.RowIndex).Cells(3).Value
        txtbiaya.Text = DataGridView1.Rows(e.RowIndex).Cells(4).Value
        txtnisn.Focus()
    End Sub

    Private Sub Siswa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call TAHUNAJARAN()
        Call nisnotomatis()
        Call tampilkelas()
        Call tampilgrid()
        txttempo.Text = DateValue("10/01/" & Year(Today) & "")

    End Sub
    Sub TAHUNAJARAN()
        Call koneksi()
        Dim str As String
        str = "Select Tahun_Ajaran from tbl_tahunajaran"
        CMD = New OdbcCommand(str, CONN)
        RD = CMD.ExecuteReader
        If RD.HasRows Then
            Do While RD.Read
                txttahun.Items.Add(RD("Tahun_Ajaran"))
            Loop
        End If
    End Sub

    Private Sub txttahun_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txttahun.SelectedIndexChanged
        If txttahun.SelectedItem = Nothing Then
            txtbiaya.Text = ""

        Else
            If txttahun.Text = "2018/2019" Then
                txtbiaya.Text = 750000

            ElseIf txttahun.Text = "2019/2020" Then
                txtbiaya.Text = 750000
            Else
                txttahun.Text = "2020/2021"
                txtbiaya.Text = 500000
            End If
        End If
    End Sub

End Class