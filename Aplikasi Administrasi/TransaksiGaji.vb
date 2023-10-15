Imports System.Data.Odbc
Public Class TransaksiGaji
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
    Sub tampilgrid()
        Call koneksi()
        DA = New OdbcDataAdapter("Select * from tbl_potongan", CONN)
        DS = New DataSet
        DA.Fill(DS)
        DS.Clear()
        DA.Fill(DS, "tbl_potongan")
        DataGridView1.DataSource = (DS.Tables("tbl_potongan"))
        DataGridView1.Columns.Add("Jumlah", "Jumlah")
        DataGridView1.Columns(0).ReadOnly = True
        DataGridView1.Columns(1).ReadOnly = True
        DataGridView1.Columns(0).Width = 50
        Me.DataGridView1.Columns(0).HeaderText = "Kode Potongan"
        Me.DataGridView1.Columns(1).HeaderText = "Jenis Potongan"
        DataGridView1.Columns(1).Width = 100
        DataGridView1.Columns("Jumlah").Width = 70
    End Sub
    Sub TOTALPOTONGAN()
        Dim hitung As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            hitung = hitung + Val(DataGridView1.Rows(i).Cells(2).Value)
            txtpotongan.Text = hitung
        Next
    End Sub
    Sub nomorotomatis()
        Call koneksi()
        CMD = New OdbcCommand("Select * from tbl_gaji where Nomor_Slip in (select max(Nomor_Slip) from tbl_gaji)", CONN)
        RD = CMD.ExecuteReader
        RD.Read()
        If Not RD.HasRows Then
            txtnoslip.Text = Format(Now, "yyMMdd") + "0001"
        Else
            If Microsoft.VisualBasic.Left(RD.Item("Nomor_Slip"), 6) = Format(Now, "yyMMdd") Then
                txtnoslip.Text = RD.Item("Nomor_Slip") + 1
            Else
                txtnoslip.Text = Format(Now, "yyMMdd") + "0001"
            End If
        End If
        txttanggal.Text = Format(Now, "yyyy-MM-dd")
    End Sub
    Sub bersihkan()
        txtpendapatan.Clear()
        txtpotongan.Clear()
        txtgajibersih.Clear()
        txtjmlhadir.Clear()
        txtgajikehadiran.Clear()
        txttjlembur.Clear()
        txtgajitjanak.Clear()
        txtgajimasakerja.Clear()
        txtgajilembur.Clear()
    End Sub
    Sub bersihkangrid()
        DataGridView1.Columns.Clear()
        Call tampilgrid()
    End Sub
    Sub batalkan()
        txtanak.Clear()
        txtgajibersih.Clear()
        txtgajikehadiran.Clear()
        txtgajilembur.Clear()
        txtgajimasakerja.Clear()
        txtgajitjanak.Clear()
        txthonor.Clear()
        txtjabatan.Clear()
        txtjml.Clear()
        txtjmlhadir.Clear()
        txtmasa.Clear()
        txtnama.Clear()
        txtnip.Clear()
        txtpendapatan.Clear()
        txtpict.Clear()
        txtpokok.Clear()
        txtpotongan.Clear()
        txttjlembur.Clear()
        txttransport.Clear()
        txttunjangan.Clear()
        txtwali.Clear()
        txtjmlmasa.Clear()
        txtkodejabatan.Clear()
        txtgolongan.Clear()
        txtstatus.Clear()
        PictureBox1.ImageLocation = Nothing
    End Sub
    Private Sub txtnip_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtnip.KeyPress
        If e.KeyChar = Chr(13) Then
            txtnip.Text = UCase(txtnip.Text)
            Call bersihkan()
            Call bersihkangrid()
            CMD = New OdbcCommand("Select * from tbl_gaji where nip='" & txtnip.Text & "' and month(Tanggal) ='" & Month(txttanggal.Text) & "' and year(Tanggal) ='" & Year(txttanggal.Text) & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If RD.HasRows Then
                MsgBox("MAAF NIP " & txtnip.Text & " BULAN INI SUDAH MENERIMA GAJI")
                Call batalkan()
                Exit Sub
            End If
            Call koneksi()
            CMD = New OdbcCommand("Select * from tbl_guru where Nip='" & txtnip.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If RD.HasRows Then
                txtnama.Text = RD.Item("Nama_Guru")
                txtkodejabatan.Text = RD.Item("Kode_Jabatan")
                txtgolongan.Text = RD.Item("Golongan")
                txtstatus.Text = RD.Item("Status")
                txtjmlmasa.Text = RD.Item("Msa_Kerja")
                txtjml.Text = RD.Item("Jml_Anak")

                PictureBox1.ImageLocation = Replace((RD("Foto")), ";", "\")
            Else
                MsgBox("NIP TIDAK TERDAFTAR")
                Call batalkan()
                DataGuru.Show()
            End If
        End If
        Call koneksi()
        CMD = New OdbcCommand("Select * from tbl_jabatan where Nama_Jabatan='" & txtkodejabatan.Text & "'", CONN)
        RD = CMD.ExecuteReader
        If RD.HasRows Then
            txtjabatan.Text = RD.Item("Kode_Jabatan")
            txtpokok.Text = RD.Item("Gaji_Pokok")
            txttunjangan.Text = RD.Item("Tj_Jabatan")
        End If
        Call koneksi()
        CMD = New OdbcCommand("Select * from tbl_golongan where Golongan='" & txtgolongan.Text & "'", CONN)
        RD = CMD.ExecuteReader
        If RD.HasRows Then
            If txtstatus.Text = "Menikah" Then
                txthonor.Text = RD.Item("Honor_Piket")
                txttransport.Text = RD.Item("Trs_Kehadiran")
                txtwali.Text = RD.Item("Tj_WaliKelas")
                txtmasa.Text = RD.Item("Tj_MasaKerja")
                txtanak.Text = RD.Item("Tj_Anak")
                txtgajitjanak.Text = Val(txtanak.Text) * Val(txtjml.Text)
            Else
                txthonor.Text = RD.Item("Honor_Piket")
                txttransport.Text = RD.Item("Trs_Kehadiran")
                txtwali.Text = 0
                txtmasa.Text = RD.Item("Tj_MasaKerja")
                txtanak.Text = 0
                txtgajitjanak.Text = 0
            End If
            txtjmlhadir.Focus()
        End If
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled = True

    End Sub
    Private Sub txtjmlhadir_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtjmlhadir.KeyPress
        If e.KeyChar = Chr(13) Then
            txtgajikehadiran.Text = Val(txttransport.Text) * Val(txtjmlhadir.Text)
            txtgajimasakerja.Text = Val(txtmasa.Text) * Val(txtjmlmasa.Text)
            txttjlembur.Focus()
        End If
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        txtpendapatan.Text = Val(txtpokok.Text) + Val(txttunjangan.Text) + Val(txtwali.Text) + Val(txtgajikehadiran.Text) + Val(txtgajilembur.Text) + Val(txtgajimasakerja.Text) + Val(txtgajitjanak.Text)
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtnip.Focus()
        Call tampilgrid()
        Call nomorotomatis()
    End Sub
    Private Sub DataGridView1_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        If e.ColumnIndex = 2 Then
            Call TOTALPOTONGAN()
            ' txtpotongan.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value
            txtgajibersih.Text = Val(txtpendapatan.Text) - Val(txtpotongan.Text)
        End If
    End Sub
    Private Sub DataGridView1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DataGridView1.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True
    End Sub

    Private Sub txttjlembur_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txttjlembur.KeyPress
        If e.KeyChar = Chr(13) Then
            txtgajilembur.Text = Val(txttjlembur.Text) * Val(txthonor.Text)
        End If
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True
    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If txtnip.Text = "" Or txtjmlhadir.Text = "" Or txtjml.Text = "" Or txtgajibersih.Text = "" Then
            MsgBox("DATA BELUM LENGKAP")
            Exit Sub
        Else
            Call koneksi()
            DA = New OdbcDataAdapter("Select * from tbl_gaji where Nomor_Slip='" & txtnoslip.Text & "'", CONN)
            DS = New DataSet
            DA.Fill(DS)
            Call koneksi()
            Dim simpan As String = "Insert into tbl_gaji values('" & txtnoslip.Text & "','" & txttanggal.Text & "','" & txtpendapatan.Text & "','" & txtpotongan.Text & "','" & txtgajibersih.Text & "','" & txtnip.Text & "','" & MenuUtama.ToolStripStatusLabel1.Text & "')"
            CMD = New OdbcCommand(simpan, CONN)
            CMD.ExecuteNonQuery()
            For baris As Integer = 0 To DataGridView1.Rows.Count - 2
                DA = New OdbcDataAdapter("Select * from tbl_detailgaji where Nomor_Slip='" & txtnoslip.Text & "'", CONN)
                DS = New DataSet
                DA.Fill(DS)
                Call koneksi()
                Dim SIMPANDETAIL As String = "Insert into tbl_detailgaji values " & "('" & txtnoslip.Text & "','" & DataGridView1.Rows(baris).Cells(0).Value & "','" & DataGridView1.Rows(baris).Cells(2).Value & "')"
                CMD = New OdbcCommand(SIMPANDETAIL, CONN)
                CMD.ExecuteNonQuery()
            Next baris

            MsgBox("DATA BERHASIL DI INPUT")
            Call batalkan()
            Call bersihkan()
            Call nomorotomatis()
            txtnip.Focus()
        End If
    End Sub

    Private Sub btnbatal_Click(sender As Object, e As EventArgs) Handles btnbatal.Click
        Call batalkan()
        Call bersihkangrid()
        txtnip.Focus()
    End Sub

    Private Sub btntutup_Click(sender As Object, e As EventArgs) Handles btntutup.Click
        Me.Close()
        MenuUtama.Show()
    End Sub

    Private Sub txtnip_TextChanged(sender As Object, e As EventArgs) Handles txtnip.TextChanged

    End Sub

    Private Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click
        Call koneksi()
        Dim EDIT As String = "Update tbl_gaji set Pendapatan='" & txtpendapatan.Text & "' ,Potongan='" & txtpotongan.Text & "' ,Gaji_Bersih='" & txtgajibersih.Text & "' ,Kode_Petugas='" & MenuUtama.ToolStripStatusLabel1.Text & "' where nip='" & txtnip.Text & "'"
        CMD = New OdbcCommand(EDIT, CONN)
        CMD.ExecuteNonQuery()
        MsgBox("DATA BERHASIL DI UBAH")
        Call batalkan()
        Call bersihkan()
        Call nomorotomatis()
        txtnip.Focus()
    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        If txtnip.Text = "" Then
            MsgBox("SILAHKAN PILIH DATA YANG AKAN DIHAPUS DENGAN MEMASUKKAN KODE PETUGAS LALU TEKAN ENTER")
        Else
            If MessageBox.Show("Yakin akan dihapus...", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Call koneksi()
                Dim hapus As String = "delete from tbl_gaji where nip='" & txtnip.Text & "'"
                CMD = New OdbcCommand(hapus, CONN)
                CMD.ExecuteNonQuery()
                Call batalkan()
                Call bersihkangrid()
                txtnip.Focus()
            End If
        End If
    End Sub

    Private Sub GroupBox5_Enter(sender As Object, e As EventArgs) Handles GroupBox5.Enter

    End Sub
End Class