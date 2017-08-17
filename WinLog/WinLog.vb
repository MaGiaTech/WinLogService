Imports System.Runtime.InteropServices
Imports System.Net.NetworkInformation

Public Class WinLog
  Declare Auto Function SetServiceStatus Lib "advapi32.dll" (ByVal handle As IntPtr, ByRef serviceStatus As ServiceStatus) As Boolean

  Public Enum ServiceState
    SERVICE_STOPPED = 1
    SERVICE_START_PENDING = 2
    SERVICE_STOP_PENDING = 3
    SERVICE_RUNNING = 4
    SERVICE_CONTINUE_PENDING = 5
    SERVICE_PAUSE_PENDING = 6
    SERVICE_PAUSED = 7
  End Enum

  <StructLayout(LayoutKind.Sequential)>
  Public Structure ServiceStatus
    Public dwServiceType As Long
    Public dwCurrentState As ServiceState
    Public dwControlsAccepted As Long
    Public dwWin32ExitCode As Long
    Public dwServiceSpecificExitCode As Long
    Public dwCheckPoint As Long
    Public dwWaitHint As Long
  End Structure

  Dim eventId As Integer

  Protected Overrides Sub OnStart(ByVal args() As String)
    ' Update the service state to Start Pending.  
    Dim serviceStatus As ServiceStatus = New ServiceStatus()
    serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING
    serviceStatus.dwWaitHint = 100000
    SetServiceStatus(Me.ServiceHandle, serviceStatus)

    ' Update the service state to Running.  
    serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING
    SetServiceStatus(Me.ServiceHandle, serviceStatus)


    ' Inserire qui il codice necessario per avviare il proprio servizio. Il metodo deve effettuare
    ' le impostazioni necessarie per il funzionamento del servizio.
    EventLog1.WriteEntry("IN PARTENZA")

    ' Set up a timer to trigger every minute.  
    Dim timer As System.Timers.Timer = New System.Timers.Timer()
    timer.Interval = 60000 ' 60 seconds  
    AddHandler timer.Elapsed, AddressOf Me.OnTimer
    timer.Start()
  End Sub

  Protected Overrides Sub OnStop()

    ' Update the service state to Stop Pending.  
    Dim serviceStatus As ServiceStatus = New ServiceStatus()
    serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING
    serviceStatus.dwWaitHint = 100000
    SetServiceStatus(Me.ServiceHandle, serviceStatus)

    ' Update the service state to Running.  
    serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING
    SetServiceStatus(Me.ServiceHandle, serviceStatus)

    ' Inserire qui il codice delle procedure di chiusura necessarie per arrestare il proprio servizio.
    EventLog1.WriteEntry("IN ARRESTO.")
  End Sub

  Private Sub OnTimer(sender As Object, e As Timers.ElapsedEventArgs)
    ' TODO: Insert monitoring activities here.  
    'EventLog1.WriteEntry("MONITORAGGIO SISTEMA", EventLogEntryType.Information, eventId)
    'eventId = eventId + 1
    GetTcpConnections()
  End Sub

  Protected Overrides Sub OnContinue()
    EventLog1.WriteEntry("IN ESECUZIONE.")
  End Sub

  Private Sub GetTcpConnections()
    Dim properties As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties()
    Dim connections As TcpConnectionInformation() = properties.GetActiveTcpConnections()

    Dim StatoConnessioni As String
    StatoConnessioni = ""

    Dim t As TcpConnectionInformation
    For Each t In connections
      StatoConnessioni = StatoConnessioni & "Stato:" & t.State.ToString & " -- Endpoint Locale:" & t.LocalEndPoint.Address.ToString & " -- Endpoint Remoto: " & t.RemoteEndPoint.Address.ToString & vbCrLf
      'Console.Write("Local endpoint: {0} ", t.LocalEndPoint.Address)
      'Console.Write("Remote endpoint: {0} ", t.RemoteEndPoint.Address)
      'Console.WriteLine("{0}", t.State)
    Next t

    EventLog1.WriteEntry(StatoConnessioni, EventLogEntryType.Information, eventId)
    eventId = eventId + 1

  End Sub 'GetTcpConnections

End Class