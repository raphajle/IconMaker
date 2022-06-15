Imports System.IO
Imports System.Drawing.Imaging

Public Class Form1
    Public Sub ConvertToIco(ByVal openimage As Bitmap, ByVal icoName As String, ByVal Icon_size As Integer)
        Dim icon As Icon
        Dim msImg = New MemoryStream
        Dim msIco = New MemoryStream
        Dim newBitmap = New Bitmap(openimage, New Size(Icon_size, Icon_size))
        Dim recDest As New Rectangle(0, 0, newBitmap.Width, newBitmap.Height)
        Dim gphCrop As Graphics = Graphics.FromImage(newBitmap)
        newBitmap.Save(msImg, ImageFormat.Png)
        Dim bw = New BinaryWriter(msIco)
        bw.Write(CType(0, Short))
        bw.Write(CType(1, Short))
        bw.Write(CType(1, Short))
        bw.Write(CType(Icon_size, Byte))
        bw.Write(CType(Icon_size, Byte))
        bw.Write(CType(0, Byte))
        bw.Write(CType(0, Byte))
        bw.Write(CType(0, Short))
        bw.Write(CType(32, Short))
        bw.Write(CType(msImg.Length, Integer))
        bw.Write(22)
        bw.Write(msImg.ToArray)
        bw.Flush()
        bw.Seek(0, SeekOrigin.Begin)
        icon = New Icon(msIco)
        Dim fs = New FileStream(icoName, FileMode.Create, FileAccess.Write)
        icon.Save(fs)
        fs.Close()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        selectImage()
    End Sub

    Function selectImage() As String
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                PictureBox1.ImageLocation = OpenFileDialog1.FileName
                TextBox1.Text = OpenFileDialog1.FileName
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation)
            End Try
        End If
        Return OpenFileDialog1.FileName
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            MsgBox("Veuillez saisir le nom de l'image")
        Else
            Try
                Dim filename = TextBox1.Text.Replace("." & TextBox1.Text.Split(".").Last, " x " & NumericUpDown1.Value & ".ico")
                ConvertToIco(Bitmap.FromFile(TextBox1.Text), filename, NumericUpDown1.Value)
                MsgBox("Icon creer avec succes !", MsgBoxStyle.Information)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Exclamation)
            End Try
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        selectImage()
    End Sub

    Private Sub TextBox1_Click(sender As Object, e As EventArgs) Handles TextBox1.Click
        selectImage()
    End Sub
End Class
