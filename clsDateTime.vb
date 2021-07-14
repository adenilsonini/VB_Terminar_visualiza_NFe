Imports System
Imports System.Net
Imports System.Net.Sockets


Public Class clsDateTime
  Public Shared Function GetNetworkTime() As DateTime
        Const ntpServer As String = "time.windows.com"
        Dim ntpData = New Byte(47) {}
        ntpData(0) = &H1B
        Dim addresses = Dns.GetHostEntry(ntpServer).AddressList
        Dim ipEndPoint = New IPEndPoint(addresses(0), 123)

        Using socket = New Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            socket.Connect(ipEndPoint)
            socket.ReceiveTimeout = 3000
            socket.Send(ntpData)
            socket.Receive(ntpData)
            socket.Close()
        End Using

        Const serverReplyTime As Byte = 40
        Dim intPart As ULong = BitConverter.ToUInt32(ntpData, serverReplyTime)
        Dim fractPart As ULong = BitConverter.ToUInt32(ntpData, serverReplyTime + 4)
        intPart = SwapEndianness(intPart)
        fractPart = SwapEndianness(fractPart)
        Dim milliseconds = (intPart * 1000) + ((fractPart * 1000) / &H100000000L)
        Dim networkDateTime = (New DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds(CLng(milliseconds))
        Return networkDateTime.ToLocalTime()
    End Function

    Private Shared Function SwapEndianness(ByVal x As ULong) As UInteger
        Return (((x And &HFF) << 24) + ((x And &HFF00) << 8) + ((x And &HFF0000) >> 8) + ((x And &HFF000000) >> 24))
    End Function
  
End Class


