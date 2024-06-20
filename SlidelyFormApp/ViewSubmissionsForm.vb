Imports System.Net.Http
Imports Newtonsoft.Json

Public Class ViewSubmissionsForm
    Private currentIndex As Integer = 0

    ' Define text boxes as class members
    Private txtName As TextBox
    Private txtEmail As TextBox
    Private txtPhone As TextBox
    Private txtGithubLink As TextBox

    Private Async Sub ViewSubmissionsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the form title and size
        Me.Text = "Ginni Jain, Slidely Task 2 - View Submissions"
        Me.Size = New Size(490, 490)
        Me.BackColor = Color.FromArgb(240, 240, 240) ' Light gray background

        ' Create and style the form title
        Dim lblTitle As New Label With {
            .Text = "View Submissions",
            .Location = New Point(110, 20),
            .AutoSize = True,
            .Font = New Font("Arial", 18, FontStyle.Bold),
            .ForeColor = Color.FromArgb(52, 152, 219) ' Blue color
        }
        Me.Controls.Add(lblTitle)

        ' Create the form fields and buttons with styles
        ' Name
        Dim lblName As New Label With {
            .Text = "Name:",
            .Location = New Point(30, 80),
            .AutoSize = True,
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(44, 62, 80) ' Dark blue text color
        }
        txtName = New TextBox With {
            .Location = New Point(170, 80),
            .Size = New Size(250, 30),
            .Font = New Font("Arial", 12),
            .ReadOnly = True,
            .BackColor = Color.White
        }

        ' Email
        Dim lblEmail As New Label With {
            .Text = "Email:",
            .Location = New Point(30, 130),
            .AutoSize = True,
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(44, 62, 80) ' Dark blue text color
        }
        txtEmail = New TextBox With {
            .Location = New Point(170, 130),
            .Size = New Size(250, 30),
            .Font = New Font("Arial", 12),
            .ReadOnly = True,
            .BackColor = Color.White
        }

        ' Phone Number
        Dim lblPhone As New Label With {
            .Text = "Phone Number:",
            .Location = New Point(30, 180),
            .AutoSize = True,
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(44, 62, 80) ' Dark blue text color
        }
        txtPhone = New TextBox With {
            .Location = New Point(170, 180),
            .Size = New Size(250, 30),
            .Font = New Font("Arial", 12),
            .ReadOnly = True,
            .BackColor = Color.White
        }

        ' GitHub repo link
        Dim lblGithub As New Label With {
            .Text = "GitHub Link:",
            .Location = New Point(30, 230),
            .AutoSize = True,
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .ForeColor = Color.FromArgb(44, 62, 80) ' Dark blue text color
        }
        txtGithubLink = New TextBox With {
            .Location = New Point(170, 230),
            .Size = New Size(250, 30),
            .Font = New Font("Arial", 12),
            .ReadOnly = True,
            .BackColor = Color.White
        }

        ' Previous button
        Dim btnPrevious As New Button With {
            .Text = "Previous (Ctrl + P)",
            .Location = New Point(30, 300),
            .Size = New Size(180, 50),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.FromArgb(52, 152, 219), ' Blue color
            .ForeColor = Color.White
        }

        ' Next button
        Dim btnNext As New Button With {
            .Text = "Next (Ctrl + N)",
            .Location = New Point(280, 300),
            .Size = New Size(150, 50),
            .Font = New Font("Arial", 12, FontStyle.Bold),
            .BackColor = Color.FromArgb(52, 73, 94), ' Dark blue-gray color
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
        Me.Controls.Add(txtGithubLink)
        Me.Controls.Add(btnPrevious)
        Me.Controls.Add(btnNext)

        ' Add event handlers
        AddHandler btnPrevious.Click, AddressOf Me.BtnPrevious_Click
        AddHandler btnNext.Click, AddressOf Me.BtnNext_Click

        ' Assign keyboard shortcuts
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf ViewSubmissionForm_KeyDown

        ' Load the first submission
        Await LoadSubmission(currentIndex)
    End Sub

    Private Async Function LoadSubmission(index As Integer) As Task
        Using client As New HttpClient()
            Try
                Dim response As HttpResponseMessage = Await client.GetAsync("http://localhost:3000/read?index=" & index)
                If response.IsSuccessStatusCode Then
                    Dim responseBody As String = Await response.Content.ReadAsStringAsync()
                    Dim submission As SubmissionData = JsonConvert.DeserializeObject(Of SubmissionData)(responseBody)

                    ' Update text fields with submission data
                    txtName.Text = submission.Name
                    txtEmail.Text = submission.Email
                    txtPhone.Text = submission.PhoneNumber
                    txtGithubLink.Text = submission.GitHubLink
                Else
                    MessageBox.Show("No More Submissions", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Try
        End Using
    End Function

    Private Async Sub BtnPrevious_Click(sender As Object, e As EventArgs)
        If currentIndex > 0 Then
            currentIndex -= 1
            Await LoadSubmission(currentIndex)
        Else
            MessageBox.Show("No previous submissions.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Async Sub BtnNext_Click(sender As Object, e As EventArgs)
        ' Assuming we don't know the maximum index, we'll handle the error when it occurs
        currentIndex += 1
        Await LoadSubmission(currentIndex)
    End Sub

    Private Sub ViewSubmissionForm_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.P Then
            BtnPrevious_Click(Nothing, Nothing)
        ElseIf e.Control AndAlso e.KeyCode = Keys.N Then
            BtnNext_Click(Nothing, Nothing)
        End If
    End Sub
End Class
