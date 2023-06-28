Imports System.IO
Imports System.Windows.Threading
Imports LibVLC_audio_test.mDeclarations
Imports LibVLCSharp.Shared

Module mVideoEngine


    Public WithEvents LibVLCMainControlA As LibVLC
    'Public WithEvents LibVLCMainControlB As LibVLC
    Public WithEvents VideoA_Player As MediaPlayer
    Public WithEvents VideoB_Player As MediaPlayer


#Region "  AudioVideoPlayer Vars + Events  "
    Private Function GetPlayerIndex(Player As MediaPlayer) As ePlayer
        Try
            Select Case True
                Case IsNothing(Player) : Return ePlayer.App
                Case Player Is VideoA_Player : Return ePlayer.VideoA
                Case Player Is VideoB_Player : Return ePlayer.VideoB
            End Select
            Return ePlayer.App
        Catch ex As Exception
            Debug.WriteLine($"GetPlayerIndex_Error  ex={ex}")
            Return ePlayer.App
        End Try
    End Function

#Region "  Playing  "
    Private Sub Video_PlayerA_Playing(sender As Object, e As EventArgs) 'Handles VideoA_Player.Playing
        Video_Player_Playing(ePlayer.VideoA)
    End Sub
    Private Sub Video_PlayerB_Playing(sender As Object, e As EventArgs) 'Handles VideoB_Player.Playing
        Video_Player_Playing(ePlayer.VideoB)
    End Sub
    Private Sub Video_Player_Playing(nPlayerIndex As ePlayer)
        Try
            'Debug.WriteLine($"Video_Player_Playing  Player={ConvertPlayerIndexToText(nPlayerIndex)}  (nPlayerIndex={nPlayerIndex})")

            'Select Case nPlayerIndex
            '    Case ePlayer.VideoA
            '        Select Case True
            '            'Case Timer_Volume_Fade_VideoA.IsEnabled
            '            Case VLC_GetAudioVolume(nPlayerIndex) = Max_Volume_VideoA
            '            Case Else
            '                Debug.WriteLine($"Video_Player_Playing  Set Volume PlayerA to {Max_Volume_VideoA}")
            '                VLC_SetAudioVolume(nPlayerIndex, Max_Volume_VideoA)
            '        End Select

            '    Case ePlayer.VideoB
            '        Select Case True
            '            'Case Timer_Volume_Fade_VideoB.IsEnabled
            '            Case VLC_GetAudioVolume(nPlayerIndex) = Max_Volume_VideoB
            '            Case Else
            '                Debug.WriteLine($"Video_Player_Playing  Set Volume PlayerB to {Max_Volume_VideoB}")
            '                VLC_SetAudioVolume(nPlayerIndex, Max_Volume_VideoB)
            '        End Select

            'End Select

            Set_ZIndex(nPlayerIndex)

        Catch ex As Exception
            Debug.WriteLine($"Video_Player_Playing_Error  ex={ex}")
        End Try
    End Sub

#End Region

#Region "  MediaChanged  "
    Private Sub Video_PlayerA_MediaChanged(sender As Object, e As MediaPlayerMediaChangedEventArgs) 'Handles VideoA_Player.MediaChanged
        Video_Player_MediaChanged(ePlayer.VideoA, VideoA_Player)
    End Sub
    Private Sub Video_PlayerB_MediaChanged(sender As Object, e As MediaPlayerMediaChangedEventArgs) 'Handles VideoB_Player.MediaChanged
        Video_Player_MediaChanged(ePlayer.VideoB, VideoB_Player)
    End Sub
    Private Sub Video_Player_MediaChanged(nPlayerIndex As ePlayer, Player As MediaPlayer)
        Try
            If IsNothing(Player) Then Return
            If IsNothing(Player.Media) Then
#Region "  MediaClosed  "
                'MediaClosed
                Debug.WriteLine($"Video_Player_MediaChanged (MediaClosed)  Player={ConvertPlayerIndexToText(nPlayerIndex)}  (nPlayerIndex={nPlayerIndex})")
#End Region

            Else
#Region "  MediaOpended  "
                'MediaOpended
                Debug.WriteLine($"Video_Player_MediaChanged (MediaOpended)  Player={ConvertPlayerIndexToText(nPlayerIndex)}  (nPlayerIndex={nPlayerIndex})")
#End Region

            End If

        Catch ex As Exception
            Debug.WriteLine($"Video_Player_MediaChanged_Error  ex={ex}")
        End Try
    End Sub
#End Region

#Region "  EndReached  "
    Private Sub Video_PlayerA_EndReached(sender As Object, e As EventArgs) 'Handles VideoA_Player.EndReached
        Video_Player_EndReached(ePlayer.VideoA)
    End Sub
    Private Sub Video_PlayerB_EndReached(sender As Object, e As EventArgs) 'Handles VideoB_Player.EndReached
        Video_Player_EndReached(ePlayer.VideoB)
    End Sub
    Private Sub Video_Player_EndReached(nPlayerIndex As ePlayer)
        Try
            Debug.WriteLine($"Video_Player_EndReached  Player={ConvertPlayerIndexToText(nPlayerIndex)}  (nPlayerIndex={nPlayerIndex})")

            Select Case nPlayerIndex
                Case ePlayer.VideoA
#Region "  VideoA  "
                    Debug.WriteLine($"VideoA_MediaEnded_or_Stopped")
                    VLC_CloseSound(ePlayer.VideoA)

                    Select Case True
                        Case VLC_GetPlayerStatus(ePlayer.VideoB) = VLCState.Playing
                            Set_ZIndex(ePlayer.VideoB)
                        Case Else
                            Set_ZIndex(ePlayer.App)
                    End Select
#End Region

                Case ePlayer.VideoB
#Region "  VideoB  "
                    Debug.WriteLine($"VideoB_MediaEnded_or_Stopped")
                    VLC_CloseSound(ePlayer.VideoB)

                    Select Case True
                        Case VLC_GetPlayerStatus(ePlayer.VideoA) = VLCState.Playing
                            Set_ZIndex(ePlayer.VideoA)
                        Case Else
                            Set_ZIndex(ePlayer.App)
                    End Select

#End Region

            End Select

        Catch ex As Exception
            Debug.WriteLine($"Video_Player_EndReached_Error  ex={ex}")
        End Try

    End Sub
#End Region

#Region "  Stopped  "
    Private Sub Video_PlayerA_Stopped(sender As Object, e As EventArgs) 'Handles VideoA_Player.Stopped
        Video_Player_Stopped(ePlayer.VideoA)
    End Sub
    Private Sub Video_PlayerB_Stopped(sender As Object, e As EventArgs) 'Handles VideoB_Player.Stopped
        Video_Player_Stopped(ePlayer.VideoB)
    End Sub
    Private Sub Video_Player_Stopped(nPlayerIndex As ePlayer)
        Try
            Debug.WriteLine($"Video_Player_Stopped  Player={ConvertPlayerIndexToText(nPlayerIndex)}  (nPlayerIndex={nPlayerIndex})")
            Dim psA As VLCState = VLC_GetPlayerStatus(ePlayer.VideoA)
            Dim psB As VLCState = VLC_GetPlayerStatus(ePlayer.VideoB)

            Select Case True
                Case nPlayerIndex = ePlayer.VideoA
                    If Get_ZIndex() <> ePlayer.VideoA Then Return
                    If psB = VLCState.Playing Then
                        Set_ZIndex(ePlayer.VideoB)
                        Return
                    End If
                    Set_ZIndex(ePlayer.App)

                Case nPlayerIndex = ePlayer.VideoB
                    If Get_ZIndex() <> ePlayer.VideoB Then Return
                    If psA = VLCState.Playing Then
                        Set_ZIndex(ePlayer.VideoA)
                        Return
                    End If
                    Set_ZIndex(ePlayer.App)

            End Select

        Catch ex As Exception
            Debug.WriteLine($"Video_Player_Stopped_Error  ex={ex}")
        End Try
    End Sub
#End Region
#End Region
    'Public Sub CreateVLCEngines()
    '    'If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
    '    '    Windows.Application.Current.Dispatcher.Invoke(Sub() CreateVLCEngines())
    '    '    Return
    '    'End If

    '    Try
    '        Debug.WriteLine($"CreateVLCEngines")

    '        LibVLCMainControlA = New LibVLC
    '        'LibVLCMainControlB = New LibVLC

    '        VideoA_Player = New MediaPlayer(LibVLCMainControlA) With {.EnableHardwareDecoding = True}
    '        MainWndw.VideoRendererA.MediaPlayer = VideoA_Player

    '        VideoB_Player = New MediaPlayer(LibVLCMainControlA) With {.EnableHardwareDecoding = True}
    '        MainWndw.VideoRendererB.MediaPlayer = VideoB_Player

    '    Catch ex As Exception
    '        Debug.WriteLine($"CreateVisioForgeEngines_Error  ex={ex}")
    '    End Try

    '    'VLC_SetAudioVolume(ePlayer.VideoA, 0)
    '    'VLC_SetAudioVolume(ePlayer.VideoB, 0)

    '    ''NOTE : Verberg alle videoschermen (VideoA/VideoB/VideoPromo)
    '    'Set_ZIndex(ePlayer.App)

    '    'Output devices zetten
    '    If GetOutputDevices() Then
    '        If GetOutputDevice2Use() Then
    '            'SetOutputDeviceDS()
    '            SetOutputDevice()
    '        End If
    '    End If
    '    'SetDelay()

    'End Sub
    Public Sub CreateVLCEngines()
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Windows.Application.Current.Dispatcher.Invoke(Sub() CreateVLCEngines())
            Return
        End If

        Try
            Debug.WriteLine($"CreateVLCEngines")

            Dim options As New List(Of String) 'From {"--aout=directsound"}
            options.Add("enableDebugLogs: true")

            LibVLCMainControlA = New LibVLC(options.ToArray)

            VideoA_Player = New MediaPlayer(LibVLCMainControlA) With {.EnableHardwareDecoding = True}
            MainWndw.VideoRendererA.MediaPlayer = VideoA_Player

            AddHandler VideoA_Player.Playing, AddressOf Video_PlayerA_Playing
            AddHandler VideoA_Player.MediaChanged, AddressOf Video_PlayerA_MediaChanged
            AddHandler VideoA_Player.EndReached, AddressOf Video_PlayerA_EndReached
            AddHandler VideoA_Player.Stopped, AddressOf Video_PlayerA_Stopped

            VideoB_Player = New MediaPlayer(LibVLCMainControlA) With {.EnableHardwareDecoding = True}
            MainWndw.VideoRendererB.MediaPlayer = VideoB_Player

            AddHandler VideoB_Player.Playing, AddressOf Video_PlayerB_Playing
            AddHandler VideoB_Player.MediaChanged, AddressOf Video_PlayerB_MediaChanged
            AddHandler VideoB_Player.EndReached, AddressOf Video_PlayerB_EndReached
            AddHandler VideoB_Player.Stopped, AddressOf Video_PlayerB_Stopped

        Catch ex As Exception
            Debug.WriteLine($"CreateVisioForgeEngines_Error  ex={ex}")
        End Try

        VLC_SetAudioVolume(ePlayer.VideoA, 0)
        VLC_SetAudioVolume(ePlayer.VideoB, 0)

        ''NOTE : Verberg alle videoschermen (VideoA/VideoB/VideoPromo)
        'Set_ZIndex(ePlayer.App)

        'Output devices zetten
        If GetOutputDevices() Then
            If GetOutputDevice2Use() Then
                'SetOutputDeviceDS()
                SetOutputDevice()
            End If
        End If
        'SetDelay()

    End Sub
    Public Sub Set_ZIndex(Player As ePlayer) ',
        Return
        'Optional HidePlayer As Boolean = False,
        'Optional DoNotLog As Boolean = False)

        'debug.writeline($"Set_ZIndex Player={Player}")
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Windows.Application.Current.Dispatcher.Invoke(Sub() Set_ZIndex(Player))
            Return
        End If

        Select Case Player
            Case ePlayer.App
                VLC_SetVideoWindowVisibility(ePlayer.VideoA, Show:=False)
                VLC_SetVideoWindowVisibility(ePlayer.VideoB, Show:=False)

            Case Else
                If Player <> ePlayer.VideoA Then VLC_SetVideoWindowVisibility(ePlayer.VideoA, Show:=False)
                If Player <> ePlayer.VideoB Then VLC_SetVideoWindowVisibility(ePlayer.VideoB, Show:=False)

                If Player = ePlayer.VideoA Then VLC_SetVideoWindowVisibility(ePlayer.VideoA, Show:=True)
                If Player = ePlayer.VideoB Then VLC_SetVideoWindowVisibility(ePlayer.VideoB, Show:=True)

        End Select
    End Sub
    Public Function Get_ZIndex() As ePlayer
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() Get_ZIndex())
        End If
        Try
            Select Case Visibility.Visible
                Case MainWndw.VideoRendererA.Visibility : Return ePlayer.VideoA
                Case MainWndw.VideoRendererB.Visibility : Return ePlayer.VideoB
            End Select
            Return ePlayer.App

        Catch ex As Exception
            Debug.WriteLine($"Get_ZIndex_Error   ex={ex}")
            Return ePlayer.App
        End Try
    End Function
    Public Function GetOutputDevices() As Boolean
        Try
            OutputDevicesAudio = New List(Of Structures.AudioOutputDevice)
            OutputDevicesAudio.AddRange(VLC_GetOutputDevices(VideoA_Player))
            'Dim counter As Short = 0
            'For Each itm As Structures.AudioOutputDevice In VLC_GetOutputDevices(VideoA_Player)
            '    Debug.WriteLine($"{counter} - {itm.Description} / {itm.DeviceIdentifier}")
            '    counter += 1
            'Next
            'counter = 0
            'For Each itm As Structures.AudioOutputDevice In VLC_GetOutputDevices(VideoB_Player)
            '    Debug.WriteLine($"{counter} - {itm.Description} / {itm.DeviceIdentifier}")
            '    counter += 1
            'Next
            Return True
        Catch ex As Exception
            Debug.WriteLine($"LoadOutputDevices_Error (1)   ex={ex}")
            Return False
        End Try
    End Function
    Public Function SetOutputDevice() As Boolean
        Dim GotError As Boolean = False
        Dim DeviceIndex_VideoA As Short = -1
        Dim DeviceIndex_VideoB As Short = -1

        Try
#Region "  Bepaal Index VideoA  "
            For Index As Short = 0 To OutputDevicesAudio.Count - 1
                If InStr((OutputDevicesAudio(Index).Description.ToUpper), DeviceSettings_VideoA.DeviceName.ToUpper, CompareMethod.Text) > 0 Then
                    DeviceIndex_VideoA = Index
                End If
            Next
#End Region

#Region "  Bepaal Index VideoB  "
            For Index As Short = 0 To OutputDevicesAudio.Count - 1
                If InStr((OutputDevicesAudio(Index).Description.ToUpper), DeviceSettings_VideoB.DeviceName.ToUpper, CompareMethod.Text) > 0 Then
                    DeviceIndex_VideoB = Index
                End If
            Next
#End Region

            Debug.WriteLine($"SetOutputDevice     DeviceIndex_VideoA={DeviceIndex_VideoA}")
            Debug.WriteLine($"SetOutputDevice     DeviceIndex_VideoB={DeviceIndex_VideoB}")

        Catch ex As Exception
            Debug.WriteLine($"SetOutputDevice_Error (1) ex={ex}")
            GotError = True
        End Try

        Try
#Region "  Zet Index VideoA  "
            If DeviceIndex_VideoA <> -1 Then
                VLC_SetOutputDevice(VideoA_Player, OutputDevicesAudio(DeviceIndex_VideoA))
            End If
        Catch ex As Exception
            Debug.WriteLine($"SetOutputDevice_Error (2) ex={ex}")
            GotError = True
        End Try
#End Region

        Try
#Region "  Zet Index VideoB  "
            If DeviceIndex_VideoB <> -1 Then
                VLC_SetOutputDevice(VideoB_Player, OutputDevicesAudio(DeviceIndex_VideoB))
            End If
#End Region
        Catch ex As Exception
            Debug.WriteLine($"SetOutputDevice_Error (3) ex={ex}")
            GotError = True
        End Try

        Return Not (GotError)
    End Function
    Public Sub StartPlayer(Player As ePlayer, Optional StartPosition As Integer = 0)
        Try
            Debug.WriteLine($"StartPlayer Player={Player}")
            If StartPosition > 0 Then
                VLC_SeekSound(Player, StartPosition)
            End If
            Dim myErr As Boolean = VLC_PlaySound(Player)
            If Not myErr Then
                Debug.WriteLine($"StartPlayer {ConvertPlayerIndexToText(Player)} myErr={myErr}")
            End If
            Set_ZIndex(Player)


        Catch ex As Exception
            Debug.WriteLine($"StartPlayer_Error  Player={Player}  ex={ex}")
        End Try
    End Sub
    Public Sub StopPlayer(Player As ePlayer)
        Try
            Debug.WriteLine($"StopPlayer  Player={Player}")
            Dim nStatus As VLCState = VLC_GetPlayerStatus(Player)
            Debug.WriteLine($"StopPlayer  {Player}  nStatus={nStatus}")

            If nStatus = VLCState.Playing Or nStatus = VLCState.Paused Then
                VLC_StopSound(Player)
            End If


        Catch ex As Exception
            Debug.WriteLine($"StopPlayer_Error  Player={Player}  ex={ex}")
        End Try
    End Sub
    Public Sub PausePlayer(Player As ePlayer)
        Try
            Debug.WriteLine($"PausePlayer Player={Player}")
            Dim myErr As Boolean = VLC_PauseSound(Player)
            If Not myErr Then
                Debug.WriteLine($"PausePlayer {ConvertPlayerIndexToText(Player)} myErr={myErr}")
            End If
            Debug.WriteLine($"PausePlayer (2)  Set_ZIndex {ConvertPlayerIndexToText(Player)}")
            Set_ZIndex(Player)


        Catch ex As Exception
            Debug.WriteLine($"PausePlayer_Error  Player={Player}  ex={ex}")
        End Try
    End Sub
    'Public Sub StartFadePlayer(Player As ePlayer)
    '    Try
    '        Debug.WriteLine($"StartFadePlayer Player={Player}")
    '        Select Case Player
    '            Case ePlayer.VideoA
    '                VolumeStepSize_VideoA = Max_Volume_VideoA / 50
    '                Debug.WriteLine($"StartFadePlayer Player={Player} VolumeStepSize_VideoA={VolumeStepSize_VideoA}")
    '                Timer_Volume_Fade_VideoA.Stop()

    '                Select Case VLC_GetPlayerStatus(Player)
    '                    Case VLCState.Paused,
    '                         VLCState.Stopped,
    '                         VLCState.Ended,
    '                         VLCState.NothingSpecial

    '                        Dim playPos As Long = VLC_GetPosition(Player)
    '                        If playPos < 0 Then playPos = 0
    '                        If playPos >= MI_Data_VideoA.SoundDuration Then playPos = 0
    '                        StartPlayer(Player, playPos)

    '                End Select

    '                VideoA_AudioFade = Determine_Volume_Stepsize(Max_Volume_VideoA, Player)
    '                Timer_Volume_Fade_VideoA.Start()

    '            Case ePlayer.VideoB
    '                VolumeStepSize_VideoB = Max_Volume_VideoB / 50
    '                Debug.WriteLine($"StartFadePlayer Player={Player} VolumeStepSize_VideoB={VolumeStepSize_VideoB}")
    '                Timer_Volume_Fade_VideoB.Stop()

    '                Select Case VLC_GetPlayerStatus(Player)
    '                    Case VLCState.Paused,
    '                         VLCState.Stopped,
    '                         VLCState.Ended,
    '                         VLCState.NothingSpecial

    '                        Dim playPos As Long = VLC_GetPosition(Player)
    '                        If playPos < 0 Then playPos = 0
    '                        If playPos >= MI_Data_VideoB.SoundDuration Then playPos = 0
    '                        StartPlayer(Player, playPos)

    '                End Select

    '                VideoB_AudioFade = Determine_Volume_Stepsize(Max_Volume_VideoB, Player)
    '                Timer_Volume_Fade_VideoB.Start()


    '        End Select

    '    Catch ex As Exception
    '        Debug.WriteLine($"StartFadePlayer_Error Player={Player} ex={ex}")
    '    End Try
    'End Sub
    'Public Sub StopFadePlayer(Player As ePlayer)
    '    Try
    '        Debug.WriteLine($"StopFadePlayer Player={Player}")
    '        Select Case Player
    '            Case ePlayer.VideoA
    '                Timer_Volume_Fade_VideoA.Stop()

    '                VideoA_AudioFade = Determine_Volume_Stepsize(0, Player)
    '                Ignore_Max_Volume_VideoA = True
    '                Timer_Volume_Fade_VideoA.Start()

    '            Case ePlayer.VideoB
    '                Timer_Volume_Fade_VideoB.Stop()

    '                VideoB_AudioFade = Determine_Volume_Stepsize(0, Player)
    '                Ignore_Max_Volume_VideoB = True
    '                Timer_Volume_Fade_VideoB.Start()

    '        End Select

    '    Catch ex As Exception
    '        Debug.WriteLine($"StopFadePlayer_Error Player={Player}  ex={ex}")
    '    End Try
    'End Sub
    'Public Function Determine_Volume_Stepsize(FadeToValue As Short,
    '                                          Player As ePlayer) As eFade

    '    'Dim VolumeValue As Int16 = VLC_GetAudioVolume(Player)
    '    Dim VolumeValue As Integer = VLC_GetAudioVolume(Player)
    '    Dim TimerIntervalvalue As TimeSpan
    '    Dim FadeDirection As eFade = eFade.FadeIn

    '    Select Case Player
    '        Case ePlayer.VideoA : TimerIntervalvalue = Timer_Volume_Fade_VideoA.Interval
    '        Case ePlayer.VideoB : TimerIntervalvalue = Timer_Volume_Fade_VideoB.Interval
    '        Case Else : Return eFade.DoNothing
    '    End Select

    '    Dim CountToVal As Short = 0
    '    Dim QtySteps As Short = VolumeFadeTime_FadeOut / TimerIntervalvalue.Milliseconds
    '    Debug.WriteLine($"Determine_Volume_Stepsize  VolumeValue={VolumeValue}")
    '    Debug.WriteLine($"Determine_Volume_Stepsize  VolumeFadeTime_FadeOut={VolumeFadeTime_FadeOut}")
    '    Debug.WriteLine($"Determine_Volume_Stepsize  TimerIntervalvalue.Milliseconds={TimerIntervalvalue.Milliseconds}")
    '    Debug.WriteLine($"Determine_Volume_Stepsize  QtySteps={QtySteps}")

    '    Select Case VolumeValue
    '        Case Is = FadeToValue
    '            FadeDirection = eFade.DoNothing

    '        Case Is > FadeToValue
    '            FadeDirection = eFade.FadeOut
    '            If VolumeValue < 0 Or VolumeValue > 200 Then
    '                VolumeValue = 100
    '            End If
    '            CountToVal = VolumeValue - FadeToValue

    '        Case Is < FadeToValue
    '            FadeDirection = eFade.FadeIn
    '            If VolumeValue < 0 Or VolumeValue > 200 Then
    '                VolumeValue = 0
    '            End If
    '            CountToVal = FadeToValue - VolumeValue

    '    End Select

    '    Dim StepSize As Integer = CountToVal / QtySteps
    '    If StepSize = 0 Then StepSize = 1
    '    Debug.WriteLine($"Determine_Volume_Stepsize  StepSize={StepSize}")
    '    Select Case Player
    '        Case ePlayer.VideoA : VolumeStepSize_VideoA = StepSize
    '        Case ePlayer.VideoB : VolumeStepSize_VideoB = StepSize
    '        Case ePlayer.VideoPromo : VolumeStepSize_VideoPromo = StepSize
    '    End Select
    '    Return FadeDirection
    'End Function
    Public Function SetPlayPosition(Player As ePlayer, nPosition As Long) As Boolean
        Try
            Dim nStatus As VLCState = VLC_GetPlayerStatus(Player)
            If nStatus = VLCState.Playing Then
                VLC_PauseSound(Player, usefixedvalue:=True, value:=True)
                VLC_SeekSound(Player, nPosition)
                Dim myErr As Boolean = VLC_PauseSound(Player, usefixedvalue:=True, value:=False)
                If Not myErr Then
                    Debug.WriteLine($"SetPosition {ConvertPlayerIndexToText(Player)} myErr={myErr}")
                End If
            Else
                VLC_SeekSound(Player, nPosition)
            End If

            Return True
        Catch ex As Exception
            Debug.WriteLine($"SetPosition_Error  Player={Player}  ex={ex}")
            Return False
        End Try
    End Function

    'Public Function LoadFilename(Player As ePlayer,
    '                             Optional FileName As String = "") As Boolean

    '    Try
    '        Debug.WriteLine($"LoadFilename={FileName}  Player={Player}")
    '        VLC_LoadSound(Player, FileName)

    '        Return True
    '    Catch ex As Exception
    '        Debug.WriteLine($"LoadFilename_Error  Player={Player}  (Filename={FileName})    ex={ex}")
    '        Return False
    '    End Try
    'End Function

#Region "  Calls  "
    <DebuggerStepThrough> Public Function ConvertPlayerIndexToText(Index As Short) As String
        Dim e As ePlayer = Index
        Return e.ToString
    End Function

#Region "  VLC Calls to Invoke  "
    Public Function VLC_LoadSound(Player As ePlayer, fFilename As String) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_LoadSound(Player, fFilename))
        End If
        Try
            Debug.WriteLine($"VLC_LoadSound  Player={ConvertPlayerIndexToText(Player)} ({Player})  fFilename={fFilename}")
            If String.IsNullOrEmpty(fFilename) Then Return False
            If Not File.Exists(fFilename) Then Return False
            Select Case Player
                Case ePlayer.VideoA
                    Select Case True
                        'Case IsNothing(VideoA_Player.Media)
                        'Case VideoA_Player.State = VLCState.Stopped
                        'Case VideoA_Player.State = VLCState.Ended
                        'Case VideoA_Player.State = VLCState.Paused
                        Case Else
                            VideoA_Player.Stop()
                    End Select
                    VideoA_Player.Media = New Media(LibVLCMainControlA, fFilename)
                Case ePlayer.VideoB
                    Select Case True
                        'Case IsNothing(VideoB_Player.Media)
                        'Case VideoB_Player.State = VLCState.Stopped
                        'Case VideoB_Player.State = VLCState.Ended
                        'Case VideoB_Player.State = VLCState.Paused
                        Case Else
                            VideoB_Player.Stop()
                    End Select
                    VideoB_Player.Media = New Media(LibVLCMainControlA, fFilename)
                Case Else : Return False
            End Select
            Return True
        Catch ex As Exception
            Debug.WriteLine($"VLC_LoadSound_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_PlaySound(Player As ePlayer, Optional fFilename As String = "") As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_PlaySound(Player, fFilename))
        End If
        Try
            Debug.WriteLine($"VLC_PlaySound  Player={ConvertPlayerIndexToText(Player)} ({Player})  fFilename={fFilename}")

            If Not String.IsNullOrEmpty(fFilename) Then
                VLC_LoadSound(Player, fFilename)
            End If
            Select Case Player
                Case ePlayer.VideoA : Return VideoA_Player.Play()
                Case ePlayer.VideoB : Return VideoB_Player.Play()
                Case Else : Return False
            End Select
        Catch ex As Exception
            Debug.WriteLine($"VLC_PlaySound_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_StopSound(Player As ePlayer) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_StopSound(Player))
        End If
        Try
            Debug.WriteLine($"VLC_StopSound  Player={ConvertPlayerIndexToText(Player)} ({Player})")
            Select Case Player
                Case ePlayer.VideoA : VideoA_Player.Stop()
                Case ePlayer.VideoB : VideoB_Player.Stop()
                Case Else : Return False
            End Select
            Return True
        Catch ex As Exception
            Debug.WriteLine($"VLC_StopSound_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_PauseSound(Player As ePlayer, Optional usefixedvalue As Boolean = False, Optional value As Boolean = False) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_PauseSound(Player, usefixedvalue, value))
        End If
        Try
            Debug.WriteLine($"VLC_PauseSound  Player={ConvertPlayerIndexToText(Player)} ({Player})  usefixedvalue={usefixedvalue}  value={value}")
            Select Case Player
                Case ePlayer.VideoA
                    If usefixedvalue Then
                        VideoA_Player.SetPause(value)
                    Else
                        VideoA_Player.Pause()
                    End If

                Case ePlayer.VideoB
                    If usefixedvalue Then
                        VideoB_Player.SetPause(value)
                    Else
                        VideoB_Player.Pause()
                    End If

                Case Else : Return False
            End Select
            Return True
        Catch ex As Exception
            Debug.WriteLine($"VLC_PauseSound_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_CloseSound(Player As ePlayer) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_CloseSound(Player))
        End If
        Try
            'debug.writeline($"VLC_CloseSound  Player={ConvertPlayerIndexToText(Player)} ({Player})")
            'Select Case Player
            '    Case ePlayer.VideoA
            '        Select Case True
            '            Case VLC_PlayerIsNothing(ePlayer.VideoA) : Return True
            '            Case VLC_PlayerMediaIsNothing(ePlayer.VideoA) : Return True
            '        End Select
            '        VideoA_Player.Media.Dispose()
            '    Case ePlayer.VideoB
            '        Select Case True
            '            Case VLC_PlayerIsNothing(ePlayer.VideoB) : Return True
            '            Case VLC_PlayerMediaIsNothing(ePlayer.VideoB) : Return True
            '        End Select
            '        VideoB_Player.Media.Dispose()
            '    Case ePlayer.VideoPromo
            '        Select Case True
            '            Case VLC_PlayerIsNothing(ePlayer.VideoPromo) : Return True
            '            Case VLC_PlayerMediaIsNothing(ePlayer.VideoPromo) : Return True
            '        End Select
            '        VideoPromo_Player.Media.Dispose()
            '    Case ePlayer.VideoRNG
            '        Select Case True
            '            Case VLC_PlayerIsNothing(ePlayer.VideoRNG) : Return True
            '            Case VLC_PlayerMediaIsNothing(ePlayer.VideoRNG) : Return True
            '        End Select
            '        RNG_Player.Media.Dispose()
            '    Case ePlayer.AmbientPlayerA
            '        Select Case True
            '            Case VLC_PlayerIsNothing(ePlayer.AmbientPlayerA) : Return True
            '            Case VLC_PlayerMediaIsNothing(ePlayer.AmbientPlayerB) : Return True
            '        End Select
            '        Ambient_PlayerA.Media.Dispose()
            '    Case ePlayer.AmbientPlayerB
            '        Select Case True
            '            Case VLC_PlayerIsNothing(ePlayer.AmbientPlayerB) : Return True
            '            Case VLC_PlayerMediaIsNothing(ePlayer.AmbientPlayerB) : Return True
            '        End Select
            '        Ambient_PlayerB.Media.Dispose()
            '    Case ePlayer.SlideshowPlayerA
            '        Select Case True
            '            Case VLC_PlayerIsNothing(ePlayer.SlideshowPlayerA) : Return True
            '            Case VLC_PlayerMediaIsNothing(ePlayer.SlideshowPlayerA) : Return True
            '        End Select
            '        Slideshow_PlayerA.Media.Dispose()
            '    Case ePlayer.SlideshowPlayerB
            '        Select Case True
            '            Case VLC_PlayerIsNothing(ePlayer.SlideshowPlayerB) : Return True
            '            Case VLC_PlayerMediaIsNothing(ePlayer.SlideshowPlayerB) : Return True
            '        End Select
            '        Slideshow_PlayerB.Media.Dispose()
            '    Case Else : Return False
            'End Select
            Return True
        Catch ex As Exception
            Debug.WriteLine($"VLC_CloseSound_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_GetDuration(Player As ePlayer) As Long
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_GetDuration(Player))
        End If
        Try
            Dim Retval As Long = -20
            Select Case Player
                Case ePlayer.VideoA
                    If Not IsNothing(VideoA_Player.Media) Then
                        Retval = VideoA_Player.Media.Duration
                    End If
                Case ePlayer.VideoB
                    If Not IsNothing(VideoB_Player.Media) Then
                        Retval = VideoB_Player.Media.Duration()
                    End If
            End Select
            'debug.writeline($"VLC_GetDuration  Player={ConvertPlayerIndexToText(Player)} ({Player})  Duration={Retval}")
            Return Retval
        Catch ex As Exception
            Debug.WriteLine($"VLC_GetDuration_Error  ex={ex}")
            Return -20
        End Try
    End Function
    Public Function VLC_GetFormattedDuration(Player As ePlayer) As String
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_GetFormattedDuration(Player))
        End If
        Return Duration_ms_to_formatted(VLC_GetDuration(Player))
    End Function
    Public Function VLC_GetPosition(Player As ePlayer) As Long
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_GetPosition(Player))
        End If
        Try
            Select Case Player
                Case ePlayer.VideoA : Return VLC_GetPosition(VideoA_Player)
                Case ePlayer.VideoB : Return VLC_GetPosition(VideoB_Player)
            End Select
            Return 0
        Catch ex As Exception
            Debug.WriteLine($"VLC_GetPosition_Error  ex={ex}")
            Return 0
        End Try
    End Function
    Public Function VLC_GetPosition(Player As MediaPlayer) As Long
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_GetPosition(Player))
        End If
        Try
            If IsNothing(Player) Then Return 0
            Return Player.Time
        Catch ex As Exception
            Debug.WriteLine($"VLC_GetPosition_Error  ex={ex}")
            Return 0
        End Try
    End Function
    Public Function VLC_GetFormattedPosition(Player As ePlayer) As String
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_GetFormattedPosition(Player))
        End If
        Return Duration_ms_to_formatted(VLC_GetPosition(Player))
    End Function
    Public Function VLC_GetPlayerStatus(Player As ePlayer) As VLCState
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_GetPlayerStatus(Player))
        End If
        Try
            Select Case Player
                Case ePlayer.VideoA : Return VideoA_Player.State
                Case ePlayer.VideoB : Return VideoB_Player.State
            End Select
            Return VLCState.Error
        Catch ex As Exception
            Debug.WriteLine($"VLC_GetPlayerStatus_Error  ex={ex}")
            Return VLCState.Error
        End Try
    End Function
    Public Function VLC_SeekSound(Player As ePlayer, nPosition As Long) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_SeekSound(Player, nPosition))
        End If
        Try
            Debug.WriteLine($"VLC_SeekSound  Player={ConvertPlayerIndexToText(Player)} ({Player})")
            Select Case Player
                Case ePlayer.VideoA : VideoA_Player.Time = nPosition
                Case ePlayer.VideoB : VideoB_Player.Time = nPosition
                Case Else : Return False
            End Select
            Return True
        Catch ex As Exception
            Debug.WriteLine($"VLC_SeekSound_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_SetVideoWindowVisibility(Player As ePlayer, Show As Boolean) As Boolean
        Return VLC_SetVideoWindowVisibility(Player, If(Show, Visibility.Visible, Visibility.Hidden))
    End Function
    Public Function VLC_SetVideoWindowVisibility(Player As ePlayer, Visible As Visibility) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_SetVideoWindowVisibility(Player, Visible))
        End If
        Try
            Select Case Player
                Case ePlayer.VideoA : MainWndw.VideoRendererA.Visibility = Visible
                Case ePlayer.VideoB : MainWndw.VideoRendererB.Visibility = Visible
                Case Else : Return False
            End Select
            Return True
        Catch ex As Exception
            Debug.WriteLine($"VLC_SetVideoWindowVisibility_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_GetVideoWindowVisibility(Player As ePlayer) As Visibility
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_GetVideoWindowVisibility(Player))
        End If
        Try
            Select Case Player
                Case ePlayer.VideoA : Return MainWndw.VideoRendererA.Visibility
                Case ePlayer.VideoB : Return MainWndw.VideoRendererB.Visibility
            End Select
            Return Visibility.Visible
        Catch ex As Exception
            Debug.WriteLine($"VLC_GetVideoWindowVisibility_Error  ex={ex}")
            Return Visibility.Visible
        End Try
    End Function



    Public Function VLC_SetOutputDevice(Engine As MediaPlayer, AudioDevice As Structures.AudioOutputDevice) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_SetOutputDevice(Engine, AudioDevice))
        End If
        Try
            Engine.SetOutputDevice(AudioDevice.DeviceIdentifier)
            Return True
        Catch ex As Exception
            Debug.WriteLine($"VLC_SetOutputDevice_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_SetOutputDevice(Player As ePlayer, AudioDevice As Structures.AudioOutputDevice) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_SetOutputDevice(Player, AudioDevice))
        End If
        Try
            Select Case Player
                Case ePlayer.VideoA : VLC_SetOutputDevice(VideoA_Player, AudioDevice)
                Case ePlayer.VideoB : VLC_SetOutputDevice(VideoB_Player, AudioDevice)
            End Select
            Return True
        Catch ex As Exception
            Debug.WriteLine($"VLC_SetOutputDevice_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_GetOutputDevices(Engine As MediaPlayer) As List(Of Structures.AudioOutputDevice)
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_GetOutputDevices(Engine))
        End If
        Return Engine.AudioOutputDeviceEnum.ToList
    End Function
    Public Function VLC_SetAudioVolume(Player As ePlayer, Volume As Integer) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_SetAudioVolume(Player, Volume))
        End If
        Try
            Debug.WriteLine($"VLC_SetAudioVolume  Player={ConvertPlayerIndexToText(Player)} ({Player})  Set Volume to {Volume}")
            Select Case Player
                Case ePlayer.VideoA
                    'If Volume > 0 Then VideoA_Player.Mute = False
                    VideoA_Player.Volume = Volume
                Case ePlayer.VideoB
                    'If Volume > 0 Then VideoB_Player.Mute = False
                    VideoB_Player.Volume = Volume
                Case Else : Return False
            End Select
            Return True
        Catch ex As Exception
            Debug.WriteLine($"VLC_SetAudioVolume_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_GetAudioVolume(Player As ePlayer) As Integer
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_GetAudioVolume(Player))
        End If
        Try
            Debug.WriteLine($"VLC_GetAudioVolume  Player={ConvertPlayerIndexToText(Player)} ({Player})")
            Dim VolumeValue As Integer = 0
            Select Case Player
                Case ePlayer.VideoA : VolumeValue = VideoA_Player.Volume
                Case ePlayer.VideoB : VolumeValue = VideoB_Player.Volume
            End Select
            Debug.WriteLine($"VLC_GetAudioVolume  Player={ConvertPlayerIndexToText(Player)} ({Player})  CurrentVolume={VolumeValue}")
            Return VolumeValue
        Catch ex As Exception
            Debug.WriteLine($"VLC_GetAudioVolume_Error  ex={ex}")
            Return 0
        End Try
    End Function
    Public Function VLC_GetAudioDelay(Player As ePlayer) As Long
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_GetAudioDelay(Player))
        End If
        Try
            Debug.WriteLine($"VLC_GetAudioDelay  Player={ConvertPlayerIndexToText(Player)} ({Player})")
            Select Case Player
                Case ePlayer.VideoA : Return VideoA_Player.AudioDelay
                Case ePlayer.VideoB : Return VideoB_Player.AudioDelay
            End Select
            Return 0
        Catch ex As Exception
            Debug.WriteLine($"VLC_GetAudioDelay_Error  ex={ex}")
            Return 0
        End Try
    End Function
    Public Function VLC_PlayerIsNothing(Player As ePlayer) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_PlayerIsNothing(Player))
        End If
        Try
            Select Case Player
                Case ePlayer.VideoA : Return IsNothing(VideoA_Player)
                Case ePlayer.VideoB : Return IsNothing(VideoB_Player)
            End Select
            Return False
        Catch ex As Exception
            Debug.WriteLine($"VLC_PlayerIsNothing_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_PlayerMediaIsNothing(Player As ePlayer) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_PlayerMediaIsNothing(Player))
        End If
        Try
            Select Case Player
                Case ePlayer.VideoA : Return IsNothing(VideoA_Player.Media)
                Case ePlayer.VideoB : Return IsNothing(VideoB_Player.Media)
            End Select
            Return False
        Catch ex As Exception
            Debug.WriteLine($"VLC_PlayerMediaIsNothing_Error  ex={ex}")
            Return False
        End Try
    End Function
    Public Function VLC_SetAudioDelay(Player As ePlayer, AudioDelay As Long) As Boolean
        If Not Windows.Application.Current.Dispatcher.CheckAccess() Then
            Return Windows.Application.Current.Dispatcher.Invoke(Function() VLC_SetAudioDelay(Player, AudioDelay))
        End If
        Try
            Debug.WriteLine($"VLC_SetAudioDelay  Player={ConvertPlayerIndexToText(Player)} ({Player})  AudioDelay={AudioDelay}")
            Select Case Player
                Case ePlayer.VideoA : Return VideoA_Player.SetAudioDelay(AudioDelay)
                Case ePlayer.VideoB : Return VideoB_Player.SetAudioDelay(AudioDelay)
            End Select
            Return False
        Catch ex As Exception
            Debug.WriteLine($"VLC_SetAudioDelay_Error  ex={ex}")
            Return 0
        End Try
    End Function
    Private Function Duration_ms_to_formatted(duration_ms As Long) As String
        Dim MyTimeMs As Long = duration_ms / 1000
        If MyTimeMs < 0 Then MyTimeMs = 0
        Dim Minutes As Short = MyTimeMs \ 60
        MyTimeMs -= (Minutes * 60)
        Return $"{Minutes}:{MyTimeMs.ToString.PadLeft(2, "0")}"
    End Function

    Private Sub LibVLCMainControl_Log(sender As Object, e As LogEventArgs) Handles LibVLCMainControlA.Log
        Debug.WriteLine($"LibVLCMainControl_Log  e={e.FormattedLog}")
    End Sub


#End Region



#End Region


End Module
