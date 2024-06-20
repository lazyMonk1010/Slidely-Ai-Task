Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class CreateSubmissionForm
    Private stopwatch As Stopwatch
    Private isRunning As Boolean
    Private updateTimer As Timer ' Timer for updating the stopwatch

    ' Define text boxes as class members
    Private txtName As TextBox
    Private txtEmail As TextBox
    Private txtPhone As TextBox
    Private txtGithub As TextBox
    Private lblStopwatchTime As Label ' New label to display stopwatch time

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        AddHandler Me.Load, AddressOf CreateSubmissionForm_Load
    End Sub

    Private Sub CreateSubmissionForm_Load(sender As Object, e As EventArgs)
        ' Set the form title and size
        Me.Text = "Ginni Jain, Slidely Task 2 - Create New Submission"
        Me.Size = New Size(490, 490)

        ' Initialize stopwatch
        stopwatch = New Stopwatch()

        ' Initialize the timer for updating the stopwatch
        updateTimer = New Timer With {
            .Interval = 1000
        }
        AddHandler updateTimer.Tick, AddressOf UpdateStopwatchTime

        ' Create the form fields and buttons
        ' Title label
        Dim lblTitle As New Label With {
            .Text = "Create New Submission",
            .Location = New Point(110, 20),
            .AutoSize = True,
            .Font = New Font("Arial", 18, FontStyle.Bold),
            .ForeColor = Color.FromArgb(52, 152, 219) ' Blue color
        }
        Me.Controls.Add(lblTitle)

        ' Name
        Dim lblName As New Label With {
            .Text = "Name:",
            .Location = New Point(30, 80),
            .AutoSize = True,
            .Font = New Font("Arial", 12, FontStyle.Bold)
        }
        txtName = New TextBox With {
            .Location = New Point(170, 80),
            .Size = New Size(200, 20)
        }

        ' Email
        Dim lblEmail As New Label With {
            .Text = "Email:",
            .Location = New Point(30, 130),
            .AutoSize = True,
            .Font = New Font("Arial", 12, FontStyle.Bold)
        }
        txtEmail = New TextBox With {
            .Location = New Point(170, 130),
            .Size = New Size(200, 20)
        }

        ' Phone Number
        Dim lblPhone As New Label With {
            .Text = "Phone Number:",
            .Location = New Point(30, 180),
            .AutoSize = True,
            .Font = New Font("Arial", 12, FontStyle.Bold)
        }
        txtPhone = New TextBox With {
            .Location = New Point(170, 180),
            .Size = New Size(200, 20)
        }

        ' GitHub repo link
        Dim lblGithub As New Label With {
            .Text = "GitHub Link:",
            .Location = New Point(30, 230),
            .AutoSize = True,
            .Font = New Font("Arial", 12, FontStyle.Bold)
        }
        txtGithub = New TextBox With {
            .Location = New Point(170, 230),
            .Size = New Size(200, 20)
        }

        ' Stopwatch button
        Dim btnStopwatch As New Button With {
            .Text = "Start Stopwatch (Ctrl + T)",
            .Location = New Point(30, 280),
            .Size = New Size(250, 35),
            .Font = New Font("Arial", 12),
            .BackColor = Color.FromArgb(52, 152, 219), ' Blue color
            .ForeColor = Color.White
        }

        ' Stopwatch time label
        lblStopwatchTime = New Label With {
            .Text = "00:00:00",
            .Location = New Point(290, 285),
            .AutoSize = True,
            .Font = New Font("Arial", 12),
            .ForeColor = Color.FromArgb(52, 152, 219) ' Blue color
        }

        ' Submit button
        Dim btnSubmit As New Button With {
            .Text = "Submit (Ctrl + &S)",
            .Location = New Point(90, 340),
            .Size = New Size(300, 50),
            .Font = New Font("Arial", 12),
            .BackColor = Color.FromArgb(46, 204, 113), ' Green color
            .ForeColor = Color.White
        }

        ' Add controls to the form
        Me.Controls.Add(lblName)
        Me.Controls.Add(txtName)
        Me.Controls.Add(lblEmail)
        Me.Controls.Add(txtEmail)
        Me.Controls.Add(lblPhone)
        Me.Controls.Add(txtPhone)
        Me.Controls.Add(lblGithub)
        Me.Controls.Add(txtGithub)
        Me.Controls.Add(btnStopwatch)
        Me.Controls.Add(lblStopwatchTime) ' Add the stopwatch time label
        Me.Controls.Add(btnSubmit)

        ' Add event handlers
        AddHandler btnStopwatch.Click, AddressOf Me.BtnStopwatch_Click
        AddHandler btnSubmit.Click, AddressOf Me.BtnSubmit_Click

        ' Assign keyboard shortcut for submit
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf CreateSubmissionForm_KeyDown
    End Sub

    Private Sub BtnStopwatch_Click(sender As Object, e As EventArgs)
        If isRunning Then
            stopwatch.Stop()
            CType(sender, Button).Text = "Start Stopwatch"
            updateTimer.Stop() ' Stop updating the stopwatch time
        Else
            stopwatch.Start()
            CType(sender, Button).Text = "Pause Stopwatch"
            updateTimer.Start() ' Start updating the stopwatch time
        End If
        isRunning = Not isRunning
    End Sub

    Private Sub UpdateStopwatchTime(sender As Object, e As EventArgs)
        lblStopwatchTime.Text = stopwatch.Elapsed.ToString("hh\:mm\:ss")
    End Sub

    Private Async Function BtnSubmit_Click(sender As Object, e As EventArgs) As Task
        ' Validate input fields
        If ValidateInputs() Then
            ' Create the data object
            Dim data As New SubmissionData() With {
                .Name = txtName.Text,
                .Email = txtEmail.Text,
                .PhoneNumber = txtPhone.Text,
                .GithubLink = txtGithub.Text
            }


            ' Serialize to JSON
            Dim jsonData As String = JsonConvert.SerializeObject(data)
            Dim content As New StringContent(jsonData, Encoding.UTF8, "application/json")


            ' Send the POST request
            Using client As New HttpClient()
                Try
                    Dim response As HttpResponseMessage = Await client.PostAsync("http://localhost:3000/submit", content)
                    If response.IsSuccessStatusCode Then
                        MessageBox.Show("Submission successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ClearFields()
                    Else
                        Dim responseBody As String = Await response.Content.ReadAsStringAsync()
                        MessageBox.Show("Error during submission: " & response.StatusCode & " - " & responseBody, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error during submission: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End If
    End Function

    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(txtName.Text) OrElse
           String.IsNullOrWhiteSpace(txtEmail.Text) OrElse
           String.IsNullOrWhiteSpace(txtPhone.Text) OrElse
           String.IsNullOrWhiteSpace(txtGithub.Text) Then
            MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    Private Sub ClearFields()
        txtName.Text = String.Empty
        txtEmail.Text = String.Empty
        txtPhone.Text = String.Empty
        txtGithub.Text = String.Empty
    End Sub

    Private Sub CreateSubmissionForm_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.S Then
            BtnSubmit_Click(Nothing, Nothing)
        End If
    End Sub
End Class