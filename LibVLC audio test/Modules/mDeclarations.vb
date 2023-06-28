Imports LibVLCSharp.Shared
Imports System.IO

Module mDeclarations
    Public Enum ePlayer
        VideoA = 0
        VideoB = 1
        App = 15
    End Enum
    Structure structDeviceSettings
        Dim DeviceName As String
        Dim Volume As Short
    End Structure

    Public MainWndw As MainWindow

    Public _Systemdata_Path As String = Path.Combine(My.Application.Info.DirectoryPath, "_Systemdata")

    Public Output_Devices As List(Of cOutputDevices)
    Public OutputDevicesAudio As List(Of Structures.AudioOutputDevice)
    Public DeviceSettings_VideoA As structDeviceSettings
    Public DeviceSettings_VideoB As structDeviceSettings
    Public Max_Volume_VideoA As Short = 100
    Public Max_Volume_VideoB As Short = 100

    Public Loading As Boolean = True

    Public FilesA As New List(Of String)
    Public FilesB As New List(Of String)

End Module
