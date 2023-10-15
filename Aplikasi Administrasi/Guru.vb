Imports System.Data.Odbc
Public Class Guru
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

        txtjml.Text = ""
        txtnama.Text = ""
        txtnip.Text = ""
        txtpict.Text = ""
        cbgolongan.SelectedItem = Nothing
        cbjabatan.SelectedItem = Nothing
        cbmasa.SelectedItem = Nothing
        ComboBox1.SelectedItem = Nothing
        PictureBox1.ImageLocation = Nothing
        txtnama.Focus()
    End Sub
    Sub DATABARU()

        txtjml.Text = ""
        txtnama.Text = ""
        txtpict.Text = ""
        cbgolongan.SelectedItem = Nothing
        cbjabatan.SelectedItem = Nothing
        cbmasa.SelectedItem = Nothing
        ComboBox1.SelectedItem = Nothing
        PictureBox1.ImageLocation = Nothing
        txtnama.Focus()
    End Sub

    Sub TABELPEGAWAI()
        Call koneksi()
        DA = New OdbcDataAdapter("Select * from tbl_guru", CONN)
        DS = New DataSet
        DA.Fill(DS, "tbl_guru")
        DataGridView1.DataSource = DS.Tables("tbl_guru")
        DataGridView1.ReadOnly = True
        DataGridView1.Columns(0).Width = 50
        Me.DataGridView1.Columns(0).HeaderText = "NIP"
        DataGridView1.Columns(1).Width = 100
        Me.DataGridView1.Columns(1).HeaderText = "Nama Guru"
        DataGridView1.Columns(2).Width = 80
        Me.DataGridView1.Columns(2).HeaderText = "Jabatan"
        DataGridView1.Columns(3).Width = 80
        Me.DataGridView1.Columns(3).HeaderText = "Kode Golongan"
        DataGridView1.Columns(4).Width = 80
        Me.DataGridView1.Columns(4).HeaderText = "Status Pernikahan"
        Me.DataGridView1.Columns(5).HeaderText = "Masa Kerja"
        DataGridView1.Columns(5).Width = 80
        DataGridView1.Columns(6).Width = 80
        Me.DataGridView1.Columns(6).HeaderText = "Jumlah Anak"
        DataGridView1.Columns(7).Width = 70
    End Sub
    Sub TAMPILJABATAN()
        Call koneksi()
        Dim str As String
        str = "Select Nama_Jabatan from tbl_jabatan"
        CMD = New OdbcCommand(str, CONN)
        RD = CMD.ExecuteReader
        If RD.HasRows Then
            Do While RD.Read
                cbjabatan.Items.Add(RD("Nama_Jabatan"))
            Loop
        End If
    End Sub
    Sub TAMPILGOLONGAN()
        Call koneksi()
        Dim str As String
        str = "Select Golongan from tbl_golongan"
        CMD = New OdbcCommand(str, CONN)
        RD = CMD.ExecuteReader
        If RD.HasRows Then
            Do While RD.Read
                cbgolongan.Items.Add(RD("Golongan"))
            Loop
        End If
    End Sub
    Sub TAMPILTABEL()
        Call TABELPEGAWAI()
        txtnama.Focus()
    End Sub
    Private Sub txtnip_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnip.KeyPress
        txtnip.MaxLength = 10
        If e.KeyChar = Chr(13) Then
            txtnip.Text = UCase(txtnip.Text)
            Call koneksi()
            CMD = New OdbcCommand("Select * from tbl_guru where Nip='" & txtnip.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If RD.HasRows Then
                txtnama.Text = RD.Item("Nama_Guru")
                cbjabatan.Text = RD.Item("Kode_Jabatan")
                cbgolongan.Text = RD.Item("Golongan")
                ComboBox1.Text = RD.Item("Status")
                cbmasa.Text = RD.Item("Msa_Kerja")
                txtjml.Text = RD.Item("Jml_Anak")
                txtpict.Text = RD.Item("Foto")
                PictureBox1.ImageLocation = Replace((RD("Foto")), ";", "\")
                txtnama.Focus()
            Else
                Call DATABARU()
                txtnama.Focus()
            End If
        End If
    End Sub

    Private Sub txtnama_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnama.KeyPress
        txtnama.MaxLength = 30
        If e.KeyChar = Chr(13) Then
            txtnama.Text = UCase(txtnama.Text)
            cbjabatan.Focus()
        End If
    End Sub
    Private Sub txtjml_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtjml.KeyPress
        If e.KeyChar = Chr(13) Then btnsave.Focus()
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True
    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If txtnip.Text = "" Or txtnama.Text = "" Or txtjml.Text = "" Or txtpict.Text = "" Or cbgolongan.Text = "" Or cbjabatan.Text = "" Or cbmasa.Text = "" Or ComboBox1.Text = "" Then
            MsgBox("HARAP ISI SEMUA DATA")
            '  Exit Sub
        Else
            '   Call koneksi()
            '  CMD = New OdbcCommand("Select * from tbl_guru where Nip='" & txtnip.Text & "','", CONN)
            '  RD = CMD.ExecuteReader
            '  RD.Read()
            ' If Not RD.HasRows Then
            Call koneksi()
            Dim SIMPAN As String = "Insert tbl_guru values('" & txtnip.Text & "','" & txtnama.Text & "','" & cbjabatan.Text & "','" & cbgolongan.Text & "','" & ComboBox1.Text & "','" & cbmasa.Text & "','" & txtjml.Text & "','" & txtpict.Text & "')"
            CMD = New OdbcCommand(SIMPAN, CONN)
            CMD.ExecuteNonQuery()
            MsgBox("DATA BERHASIL DI TAMBAHKAN")
            Call KOSONGKANDATA()
            Call TABELPEGAWAI()
        End If
        'End If
    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        If txtnip.Text = "" Then
            MsgBox("NIP TIDAK TERSEDIA, HARAP ISI DENGAN BENAR")
            txtnip.Focus()
            Exit Sub
        Else
            If MessageBox.Show("Yakin akan dihapus...", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Call koneksi()
                Dim hapus As String = "delete from tbl_guru where Nip='" & txtnip.Text & "'"
                CMD = New OdbcCommand(hapus, CONN)
                CMD.ExecuteNonQuery()
                Call KOSONGKANDATA()
                Call TABELPEGAWAI()
            Else
                Call KOSONGKANDATA()
            End If
        End If
    End Sub

    Private Sub btnpilih_Click(sender As Object, e As EventArgs) Handles btnpilih.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String

        fd.Title = "Open File Dialog"
        fd.InitialDirectory = "D:\"
        fd.Filter = "Image File (*.png*)|*.png*|Image File (*.jpg)|*.jpg*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
            txtpict.Text = strFileName
        End If
        PictureBox1.ImageLocation = txtpict.Text
    End Sub

    Private Sub btnbatal_Click(sender As Object, e As EventArgs) Handles btnbatal.Click
        Call KOSONGKANDATA()

    End Sub

    Private Sub btntutup_Click(sender As Object, e As EventArgs) Handles btntutup.Click
        Me.Close()
        MenuUtama.Show()
    End Sub

    Private Sub Guru_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call TAMPILJABATAN()
        Call TAMPILGOLONGAN()
        ComboBox1.Items.Add("Menikah")
        ComboBox1.Items.Add("Belum Menikah")
        cbmasa.Items.Add("1")
        cbmasa.Items.Add("2")
        cbmasa.Items.Add("3")
        cbmasa.Items.Add("4")
        cbmasa.Items.Add("5")
        cbmasa.Items.Add("6")
        cbmasa.Items.Add(" ")
        cbmasa.AutoCompleteSource = AutoCompleteSource.ListItems
        cbmasa.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        
        Call koneksi()
        Call TAMPILTABEL()

    End Sub
   


    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        txtnip.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        txtnip.Focus()
    End Sub

    Private Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click
        Call koneksi()
        Dim edit As String = "Update tbl_guru set Nama_Guru='" & txtnama.Text & "' ,Kode_Jabatan='" & cbjabatan.Text & "' ,Golongan='" & cbgolongan.Text & "' ,Status='" & ComboBox1.Text & "' ,Msa_Kerja='" & cbmasa.Text & "' ,Jml_Anak='" & txtjml.Text & "' ,Foto='" & txtpict.Text & "' where Nip='" & txtnip.Text & "'"
        CMD = New OdbcCommand(edit, CONN)
        CMD.ExecuteNonQuery()
        MsgBox("DATA BERHASIL DI UBAH")
        Call TABELPEGAWAI()
        Call KOSONGKANDATA()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.Text = "Belum Menikah" Then
            txtjml.Text = "0"
        End If
    End Sub

    
End Class