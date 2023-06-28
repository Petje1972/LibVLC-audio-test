Imports System.IO
Module mIO
    Public Function GetOutputDevice2Use() As Boolean
        Dim FilePointer As Short = FreeFile()
        Try
            Dim FileName As String = Path.Combine(_Systemdata_Path, "deviceID.ini")
            If Not File.Exists(FileName) Then Return False

            FileOpen(FilePointer, FileName, OpenMode.Input, OpenAccess.Read, OpenShare.Shared)
            Dim _1stLeft As String = LineInput(FilePointer)
            Dim _1stRight As String = LineInput(FilePointer)

            Debug.WriteLine($"GetOutputDevice2Use  _1stLeft={_1stLeft}")
            Debug.WriteLine($"GetOutputDevice2Use _1stRight={_1stRight}")

            Dim SplStr_A As String() = _1stLeft.Split("|")
            DeviceSettings_VideoA.DeviceName = SplStr_A(0)
            DeviceSettings_VideoA.Volume = If(IsNumeric(SplStr_A(1)), CShort(SplStr_A(1)), 100)

            Dim SplStr_B As String() = _1stRight.Split("|")
            DeviceSettings_VideoB.DeviceName = SplStr_B(0)
            DeviceSettings_VideoB.Volume = If(IsNumeric(SplStr_B(1)), CShort(SplStr_B(1)), 100)

            Return True
        Catch ex As Exception
            Debug.WriteLine($"GetOutputDevice2Use_Error  ex={ex}")
            Return False
        Finally
            FileClose(FilePointer)
        End Try
    End Function
    Public Function RandomizeList(ListIn As List(Of String)) As List(Of String)
        Try
            Threading.Thread.Sleep(1)

            Randomize()

            Dim BaseData As New List(Of String)
            BaseData.AddRange(ListIn)

            Dim ListOut As New List(Of String)

            Do Until BaseData.Count = 0
                Dim Index As Integer = CInt(Int(BaseData.Count * Rnd()))
                ListOut.Add(BaseData(Index))
                BaseData.RemoveAt(Index)
            Loop
            Return ListOut

        Catch ex As Exception
            Debug.WriteLine($"RandomizeList_Error  ex={ex}")
            Return ListIn
        End Try
    End Function


End Module
