Imports System.IO
Imports LibVLC_audio_test.mDeclarations
Imports LibVLCSharp.Shared
Class MainWindow
    Private Sub ButtonLoadA_Click(sender As Object, e As RoutedEventArgs)
        Dim file As String = FilesA.First
        FilesA.Add(FilesA.First)
        FilesA.RemoveAt(0)
        VLC_LoadSound(ePlayer.VideoA, file)

        'If Not IsNothing(VideoA_Player.Media) Then
        'VideoA_Player.Stop()
        'End If
        'VideoA_Player.Media = New Media(LibVLCMainControl, file)
    End Sub
    Private Sub ButtonLoadB_Click(sender As Object, e As RoutedEventArgs)
        Dim file As String = FilesB.First
        FilesB.Add(FilesB.First)
        FilesB.RemoveAt(0)
        VLC_LoadSound(ePlayer.VideoB, file)
        'If Not IsNothing(VideoB_Player.Media) Then
        'VideoB_Player.Stop()
        'End If
        'Debug.WriteLine($"VideoB_Player.State={VideoB_Player.State}")
        'VideoB_Player.Media = New Media(LibVLCMainControl, file)
    End Sub
    Private Sub ButtonPlayPauseA_Click(sender As Object, e As RoutedEventArgs)
        'Select Case VideoA_Player.State
        '    Case VLCState.Playing, VLCState.Paused
        '        VideoA_Player.Pause()
        '    Case Else
        '        VideoA_Player.Play()
        'End Select
        Select Case VLC_GetPlayerStatus(ePlayer.VideoA)
            Case VLCState.Playing, VLCState.Paused
                VLC_PauseSound(ePlayer.VideoA)
            Case Else
                VLC_PlaySound(ePlayer.VideoA)
        End Select
    End Sub
    Private Sub ButtonPlayPauseB_Click(sender As Object, e As RoutedEventArgs)
        'Select Case VideoB_Player.State
        '    Case VLCState.Playing, VLCState.Paused
        '        VideoB_Player.Pause()
        '    Case Else
        '        VideoB_Player.Play()
        'End Select
        Select Case VLC_GetPlayerStatus(ePlayer.VideoB)
            Case VLCState.Playing, VLCState.Paused
                VLC_PauseSound(ePlayer.VideoB)
            Case Else
                VLC_PlaySound(ePlayer.VideoB)
        End Select
    End Sub

    Private Sub ButtonStopA_Click(sender As Object, e As RoutedEventArgs)
        VideoA_Player.Stop()
        'VLC_StopSound(ePlayer.VideoA)
    End Sub
    Private Sub ButtonStopB_Click(sender As Object, e As RoutedEventArgs)
        VideoB_Player.Stop()
        'VLC_StopSound(ePlayer.VideoB)
    End Sub

    Private Sub ButtonShowA_Click(sender As Object, e As RoutedEventArgs)
        Dim v As Visibility = VLC_GetVideoWindowVisibility(ePlayer.VideoA)
        If v = Visibility.Visible Then
            v = Visibility.Hidden
        Else
            v = Visibility.Visible
        End If
        VLC_SetVideoWindowVisibility(ePlayer.VideoA, v)
        'Dim v As Visibility = VideoRendererA.Visibility
        'If v = Visibility.Visible Then
        '    v = Visibility.Hidden
        'Else
        '    v = Visibility.Visible
        'End If
        'VideoRendererA.Visibility = v
    End Sub
    Private Sub ButtonShowB_Click(sender As Object, e As RoutedEventArgs)
        Dim v As Visibility = VLC_GetVideoWindowVisibility(ePlayer.VideoB)
        If v = Visibility.Visible Then
            v = Visibility.Hidden
        Else
            v = Visibility.Visible
        End If
        VLC_SetVideoWindowVisibility(ePlayer.VideoB, v)
        'Dim v As Visibility = VideoRendererB.Visibility
        'If v = Visibility.Visible Then
        '    v = Visibility.Hidden
        'Else
        '    v = Visibility.Visible
        'End If
        'VideoRendererB.Visibility = v
    End Sub
    Private Sub VolumeVideoA_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
        If Loading Then Return
        VLC_SetAudioVolume(ePlayer.VideoA, VolumeVideoA.Value)
        'VideoA_Player.Volume = VolumeVideoA.Value
    End Sub
    Private Sub VolumeVideoB_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
        If Loading Then Return
        VLC_SetAudioVolume(ePlayer.VideoB, VolumeVideoB.Value)
        'VideoB_Player.Volume = VolumeVideoB.Value
    End Sub

    Private Sub ListVideoA_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ListVideoA.SelectionChanged
        Return
        If Loading Then Return
        VLC_SetOutputDevice(ePlayer.VideoA, OutputDevicesAudio(ListVideoA.SelectedIndex))
    End Sub
    Private Sub ListVideoB_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ListVideoB.SelectionChanged
        Return
        If Loading Then Return
        VLC_SetOutputDevice(ePlayer.VideoB, OutputDevicesAudio(ListVideoB.SelectedIndex))
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        CreateVLCEngines()
        Loading = False

        Dim SourceFolder As String = "C:\sv7 bestanden"
        If Debugger.IsAttached Then
            SourceFolder = "E:\Media Bestanden\MP4"
        End If
        Dim files_effe As New List(Of String)
        files_effe.AddRange(Directory.GetFiles(SourceFolder, "*.mp4").ToList)
        files_effe.AddRange(Directory.GetFiles(SourceFolder, "*.sv7").ToList)

        FilesA = New List(Of String)
        FilesA.AddRange(RandomizeList(files_effe))
        FilesB = New List(Of String)
        FilesB.AddRange(RandomizeList(files_effe))

        For Each itm As Structures.AudioOutputDevice In OutputDevicesAudio
            ListVideoA.Items.Add(itm.Description)
            ListVideoB.Items.Add(itm.Description)
        Next
        If ListVideoA.Items.Count > 0 Then
            ListVideoA.SelectedIndex = 0
            ListVideoB.SelectedIndex = 0
        End If

    End Sub


End Class
