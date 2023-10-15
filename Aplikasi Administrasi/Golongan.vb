Imports System.Data.Odbc
Public Class Golongan
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
    Sub kosongkandata()
        txtanak.Text = ""
        txtgolongan.Text = ""
        txthonor.Text = ""
        txtmasa.Text = ""
        txttransport.Text = ""
        txtwali.Text = ""
        txtgolongan.Focus()
    End Sub
    Sub DATABARU()
        txtanak.Text = ""
        txthonor.Text = ""
        txtmasa.Text = ""
        txttransport.Text = ""
        txtwali.Text = ""
        txtanak.Focus()
    End Sub
    Sub TAMPILGRID()
        Call koneksi()
        DA = New OdbcDataAdapter("Select * from tbl_golongan", CONN)
        DS = New DataSet
        DA.Fill(DS, "tbl_golongan")
        DataGridView1.DataSource = (DS.Tables("tbl_golongan"))
        DataGridView1.ReadOnly = True
        DataGridView1.Columns(0).Width = 70
        DataGridView1.Columns(1).Width = 95
        DataGridView1.Columns(2).Width = 100
        DataGridView1.Columns(3).Width = 110
        DataGridView1.Columns(4).Width = 110
        DataGridView1.Columns(5).Width = 90
        Me.DataGridView1.Columns(0).HeaderText = "Golongan"
        Me.DataGridView1.Columns(1).HeaderText = "Tunjangan Lembur"
        Me.DataGridView1.Columns(2).HeaderText = "Transport Kehadiran"
        Me.DataGridView1.Columns(3).HeaderText = "Tunjangan Suami Istri"
        Me.DataGridView1.Columns(4).HeaderText = "Tunjangan Masa Kerja"
        Me.DataGridView1.Columns(5).HeaderText = "Tunjangan Anak"
    End Sub

    Private Sub txtgolongan_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtgolongan.KeyPress
        If e.KeyChar = Chr(13) Then
            CMD = New OdbcCommand("Select * from tbl_golongan where Golongan='" & txtgolongan.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If RD.HasRows Then
                txthonor.Text = RD.Item(1)
                txttransport.Text = RD.Item(2)
                txtwali.Text = RD.Item(3)
                txtmasa.Text = RD.Item(4)
                txtanak.Text = RD.Item(5)
                txthonor.Focus()
            Else
                Call DATABARU()
                txthonor.Focus()
            End If
        End If
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True
    End Sub

    Private Sub btnsave_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If txtgolongan.Text = "" Or txthonor.Text = "" Or txttransport.Text = "" Or txtwali.Text = "" Or txtmasa.Text = "" Or txtanak.Text = "" Then
            MsgBox("HARAP ISI SEMUA DATA")
            Exit Sub
        Else
            Call koneksi()
            CMD = New OdbcCommand("Select * from tbl_golongan where Golongan='" & txtgolongan.Text & "'", CONN)
            RD = CMD.ExecuteReader
            RD.Read()
            If Not RD.HasRows Then
                Call koneksi()
                Dim SIMPAN As String = "Insert tbl_golongan values('" & txtgolongan.Text & "','" & txthonor.Text & "','" & txttransport.Text & "','" & txtwali.Text & "','" & txtmasa.Text & "','" & txtanak.Text & "')"
                CMD = New OdbcCommand(SIMPAN, CONN)
                CMD.ExecuteNonQuery()
                Call kosongkandata()
                Call TAMPILGRID()

            End If
        End If
    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        If txtgolongan.Text = "" Then
            MsgBox("HARAP MASUKAN KODE GOLONGAN TERLEBIH DAHULU")
            txtgolongan.Focus()
            Exit Sub
        Else
            If MessageBox.Show("Yakin akan dihapus...", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                Call koneksi()
                Dim hapus As String = "delete from tbl_golongan where Golongan='" & txtgolongan.Text & "'"
                CMD = New OdbcCommand(hapus, CONN)
                CMD.ExecuteNonQuery()
                Call TAMPILGRID()
                Call kosongkandata()

            End If
        End If
    End Sub

    Private Sub Golongan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call TAMPILGRID()

    End Sub

    Private Sub btnbatal_Click(sender As Object, e As EventArgs) Handles btnbatal.Click
        kosongkandata()

    End Sub

    Private Sub btntutup_Click(sender As Object, e As EventArgs) Handles btntutup.Click
        Me.Close()
        MenuUtama.Show()
    End Sub

    Private Sub txthonor_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txthonor.KeyPress
        If e.KeyChar = Chr(13) Then txttransport.Focus()
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True

    End Sub

    Private Sub txttransport_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txttransport.KeyPress
        If e.KeyChar = Chr(13) Then txtwali.Focus()
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True

    End Sub

    Private Sub txtwali_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtwali.KeyPress
        If e.KeyChar = Chr(13) Then txtmasa.Focus()
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True

    End Sub

    Private Sub txtmasa_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtmasa.KeyPress
        If e.KeyChar = Chr(13) Then txtanak.Focus()
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True

    End Sub

    Private Sub txtanak_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtanak.KeyPress
        If e.KeyChar = Chr(13) Then btnsave.Focus()
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled() = True

    End Sub

    Private Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click
        Call koneksi()
        Dim EDIT As String = "Update tbl_golongan set Honor_Piket='" & txthonor.Text & "' ,Trs_Kehadiran='" & txttransport.Text & "' ,Tj_WaliKelas='" & txtwali.Text & "', Tj_MasaKerja='" & txtmasa.Text & "', Tj_Anak='" & txtanak.Text & "' where Golongan='" & txtgolongan.Text & "'"
        CMD = New OdbcCommand(EDIT, CONN)
        CMD.ExecuteNonQuery()
        MsgBox("Data Berhasil Di Edit")
        Call TAMPILGRID()
        Call kosongkandata()
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        On Error Resume Next
        txtgolongan.Text = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        txthonor.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value
        txttransport.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value
        txtwali.Text = DataGridView1.Rows(e.RowIndex).Cells(3).Value
        txtmasa.Text = DataGridView1.Rows(e.RowIndex).Cells(4).Value
        txtanak.Text = DataGridView1.Rows(e.RowIndex).Cells(5).Value
    End Sub
End Class