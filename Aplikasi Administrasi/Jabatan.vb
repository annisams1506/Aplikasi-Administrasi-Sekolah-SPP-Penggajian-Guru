Imports System.Data.Odbc
Public Class Jabatan
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
        txtjabatan.Text = ""
        txtkode.Text = ""
        txtpokok.Text = ""
        txttunjangan.Text = ""
    End Sub
    Sub DATABARU()
        txtjabatan.Text = ""
        txtpokok.Text = ""
        txttunjangan.Text = ""
        txtjabatan.Focus()
    End Sub
    Sub TAMPILGRID()
        Call koneksi()
        DA = New OdbcDataAdapter("Select * from tbl_jabatan", CONN)
        DS = New DataSet
        DA.Fill(DS, "tbl_jabatan")
        DataGridView1.DataSource = (DS.Tables("tbl_jabatan"))
        DataGridView1.ReadOnly = True
        DataGridView1.Columns(0).Width = 100
        DataGridView1.Columns(1).Width = 110

        DataGridView1.Columns(2).Width = 123
        DataGridView1.Columns(2).DefaultCellStyle.Format = "##,##,##"
        DataGridView1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        DataGridView1.Columns(3).Width = 123
        DataGridView1.Columns(3).DefaultCellStyle.Format = "##,##,##"
        DataGridView1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Me.DataGridView1.Columns(0).HeaderText = "Kode Jabatan"
        Me.DataGridView1.Columns(1).HeaderText = "Jabatan"
        Me.DataGridView1.Columns(2).HeaderText = "Gaji Pokok"
        Me.DataGridView1.Columns(3).HeaderText = "Tunjangan Jabatan"

    End Sub

    Private Sub txtkode_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtkode.KeyPress
        txtkode.MaxLength = 10
        If e.KeyChar = Chr(13) Then
            txtkode.Text = UCase(txtkode.Text)
            Call koneksi()
            CMD = New OdbcCommand("Select * from tbl_jabatan where Kode_Jabatan='" & txtkode.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If RD.HasRows Then
                txtjabatan.Text = RD.Item(1)
                txtpokok.Text = RD.Item(2)
                txttunjangan.Text = RD.Item(3)
                txtjabatan.Focus()
            Else
                Call DATABARU()
                txtjabatan.Focus()
            End If
        End If
    End Sub

    Private Sub txtjabatan_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtjabatan.KeyPress
        txtjabatan.MaxLength = 20
        If e.KeyChar = Chr(13) Then
            txtjabatan.Text = UCase(txtjabatan.Text)
            txtpokok.Focus()
        End If
    End Sub

    Private Sub txtpokok_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtpokok.KeyPress
        If e.KeyChar = Chr(13) Then
            txttunjangan.Focus()
            If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True
        End If
    End Sub

    Private Sub txttunjangan_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txttunjangan.KeyPress
        If e.KeyChar = Chr(13) Then
            btnsave.Focus()
            If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True
        End If
    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If txtkode.Text = "" Or txtjabatan.Text = "" Or txtpokok.Text = "" Or txttunjangan.Text = "" Then
            MsgBox("HARAP ISI SEMUA DATA")
            Exit Sub
        Else
            Call koneksi()
            CMD = New OdbcCommand("Select * from tbl_jabatan where Kode_Jabatan='" & txtkode.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If Not RD.HasRows Then
                Call koneksi()
                Dim SIMPAN As String = "Insert tbl_jabatan values('" & txtkode.Text & "','" & txtjabatan.Text & "','" & txtpokok.Text & "','" & txttunjangan.Text & "')"
                CMD = New OdbcCommand(SIMPAN, CONN)
                CMD.ExecuteNonQuery()
                Call KOSONGKANDATA()
                Call TAMPILGRID()

            End If
        End If
    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
         If txtkode.Text = "" Then
            MsgBox("PILIH KODE JABATAN YANG INGIN DIHAPUS")
        Else
            If MessageBox.Show("Yakin akan dihapus...", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Call koneksi()
                Dim hapus As String = "Delete from tbl_jabatan where Kode_Jabatan='" & txtkode.Text & "'"
                CMD = New OdbcCommand(hapus, CONN)
                CMD.ExecuteNonQuery()
                Call TAMPILGRID()
                Call KOSONGKANDATA()
            End If
        End If
    End Sub

    Private Sub btnbatal_Click(sender As Object, e As EventArgs) Handles btnbatal.Click
        Call KOSONGKANDATA()
    End Sub

    Private Sub btntutup_Click(sender As Object, e As EventArgs) Handles btntutup.Click
        Me.Close()
        MenuUtama.Show()
    End Sub

    Private Sub Jabatan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call TAMPILGRID()
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        txtkode.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        txtjabatan.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value
        txtpokok.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value
        txttunjangan.Text = DataGridView1.Rows(e.RowIndex).Cells(3).Value
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Call koneksi()
        Dim edit As String = "Update tbl_jabatan set Nama_Jabatan='" & txtjabatan.Text & "', Gaji_Pokok='" & txtpokok.Text & "', Tj_Jabatan='" & txttunjangan.Text & "' where Kode_Jabatan='" & txtkode.Text & "'"
        CMD = New OdbcCommand(edit, CONN)
        CMD.ExecuteNonQuery()
        MsgBox("DATA BERHASIL DI UBAH")
        Call TAMPILGRID()
        Call KOSONGKANDATA()
    End Sub
End Class