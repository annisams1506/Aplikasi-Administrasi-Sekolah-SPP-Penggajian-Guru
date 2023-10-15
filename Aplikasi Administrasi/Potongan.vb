Imports System.Data.Odbc
Public Class Potongan
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
    Sub KOSONGKAN()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox1.Focus()
    End Sub
    Sub DATABARU()
        TextBox2.Text = ""
        TextBox2.Focus()
    End Sub
    Sub tampilgrid()
        Call koneksi()
        DA = New OdbcDataAdapter("Select * from tbl_potongan", CONN)
        DS = New DataSet
        DS.Clear()
        DA.Fill(DS, "tbl_potongan")
        DataGridView1.DataSource = (DS.Tables("tbl_potongan"))
        DataGridView1.ReadOnly = True
        DataGridView1.Columns(0).Width = 100
        DataGridView1.Columns(1).Width = 120
        Me.DataGridView1.Columns(0).HeaderText = "Kode Potongan"
        Me.DataGridView1.Columns(1).HeaderText = "Nama Potongan"
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        TextBox1.MaxLength = 5
        If e.KeyChar = Chr(13) Then
            CMD = New OdbcCommand("Select * from tbl_potongan where Kode_Potongan='" & TextBox1.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If RD.HasRows = True Then
                TextBox2.Text = RD.Item(1)
                TextBox2.Focus()
            Else
                Call DATABARU()
                TextBox2.Focus()
            End If
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        TextBox2.MaxLength = 30
        If e.KeyChar = Chr(13) Then
            TextBox2.Text = UCase(TextBox2.Text)
            Button1.Focus()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Then
            MsgBox("Maaf, Data Yang Anda Masukan Belum Lengkap")

        Else

            Call koneksi()
            Dim Simpan As String = "Insert tbl_potongan values('" & TextBox1.Text & "','" & TextBox2.Text & "')"
            CMD = New OdbcCommand(Simpan, CONN)
            CMD.ExecuteNonQuery()
            Call KOSONGKAN()
            Call tampilgrid()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If TextBox1.Text = "" Then
            MsgBox("SILAHKAN PILIH DATA YANG AKAN DIHAPUS DENGAN MEMASUKKAN KODE PETUGAS LALU TEKAN ENTER")
        Else
            If MessageBox.Show("Yakin akan dihapus...", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Call koneksi()
                Dim hapus As String = "delete from tbl_potongan where Kode_Potongan='" & TextBox1.Text & "'"
                CMD = New OdbcCommand(hapus, CONN)
                CMD.ExecuteNonQuery()
                Call tampilgrid()
                Call KOSONGKAN()

            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Call KOSONGKAN()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Close()
        MenuUtama.Show()
    End Sub

    Private Sub Potongan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call tampilgrid()
    End Sub

    Private Sub BTNEDIT_Click(sender As Object, e As EventArgs) Handles BTNEDIT.Click
        Call koneksi()
        Dim EDIT As String = "Update tbl_potongab set Nama_Potongan='" & TextBox2.Text & "' where Kode_Petugas='" & TextBox1.Text & "'"
        CMD = New OdbcCommand(EDIT, CONN)
        CMD.ExecuteNonQuery()
        MsgBox("Data Berhasil Di Edit")
        Call tampilgrid()
        Call KOSONGKAN()
    End Sub
End Class