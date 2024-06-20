Public Class MainForm
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Set the form title
        Me.Text = "Ginni Jain, Slidely Task 2 - Slidely Form App"

        ' Adjust the form size and center it
        Me.Size = New Size(530, 400)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.BackColor = Color.FromArgb(240, 240, 240) ' Light gray background

        ' Create the title label
        Dim lblTitle As New Label With {
            .Text = "Slidely Form App",
            .Font = New Font("Arial", 20, FontStyle.Bold),
            .Location = New Point(150, 30),
            .AutoSize = True,
            .ForeColor = Color.FromArgb(52, 73, 94) ' Dark blue-gray text color
        }

        ' Create the action prompt label
        Dim lblTitle1 As New Label With {
            .Text = "Choose an Action:",
            .Font = New Font("Arial", 14, FontStyle.Bold),
            .Location = New Point(175, 70),
            .AutoSize = True,
            .ForeColor = Color.FromArgb(44, 62, 80) ' Dark blue text color
        }

        ' Create the View Submissions button
        Dim btnViewSubmissions As New Button With {
            .Text = "View Submissions (Ctrl + &V)",
            .Location = New Point(80, 140),
            .Size = New Size(350, 50),
            .Font = New Font("Arial", 14, FontStyle.Bold),
            .BackColor = Color.FromArgb(241, 196, 15), ' Yellow
            .ForeColor = Color.White
        }

        ' Create the Create New Submission button
        Dim btnCreateSubmission As New Button With {
            .Text = "Create New Submission (Ctrl + &N)",
            .Location = New Point(80, 210),
            .Size = New Size(350, 50),
            .Font = New Font("Arial", 14, FontStyle.Bold),
            .BackColor = Color.FromArgb(41, 128, 185), ' Blue
            .ForeColor = Color.White
        }

        ' Add labels and buttons to the form
        Me.Controls.Add(lblTitle)
        Me.Controls.Add(lblTitle1)
        Me.Controls.Add(btnViewSubmissions)
        Me.Controls.Add(btnCreateSubmission)

        ' Add event handlers
        AddHandler btnViewSubmissions.Click, AddressOf Me.BtnViewSubmissions_Click
        AddHandler btnCreateSubmission.Click, AddressOf Me.BtnCreateSubmission_Click

        ' Assign keyboard shortcuts
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf MainForm_KeyDown
    End Sub

    Private Sub BtnViewSubmissions_Click(sender As Object, e As EventArgs)
        Dim viewForm As New ViewSubmissionsForm()
        viewForm.ShowDialog()
    End Sub

    Private Sub BtnCreateSubmission_Click(sender As Object, e As EventArgs)
        Dim createForm As New CreateSubmissionForm()
        createForm.ShowDialog()
    End Sub

    Private Sub MainForm_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.V Then
            BtnViewSubmissions_Click(Nothing, Nothing)
        ElseIf e.Control AndAlso e.KeyCode = Keys.N Then
            BtnCreateSubmission_Click(Nothing, Nothing)
        End If
    End Sub
End Class
