Imports System.ServiceProcess

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WinLog
  Inherits System.ServiceProcess.ServiceBase

  'UserService esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  ' Il punto di ingresso principale del processo
  <MTAThread()> _
  <System.Diagnostics.DebuggerNonUserCode()> _
  Shared Sub Main()
    Dim ServicesToRun() As System.ServiceProcess.ServiceBase

    ' All'interno di uno stesso processo è possibile eseguire più servizi di Windows NT.
    ' Per aggiungere un servizio al processo, modificare la riga che segue in modo
    ' da creare un secondo oggetto servizio. Ad esempio,
    '
    '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New WinLog, New MySecondUserService}
    '
    ServicesToRun = New System.ServiceProcess.ServiceBase() {New WinLog}

    System.ServiceProcess.ServiceBase.Run(ServicesToRun)
  End Sub

  'Richiesto da Progettazione componenti
  Private components As System.ComponentModel.IContainer
  'Private EventLog1 As System.Diagnostics.EventLog

  ' NOTA: la procedura che segue è richiesta da Progettazione componenti
  ' Può essere modificata in Progettazione componenti.  
  ' Non modificarla nell'editor del codice.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.components = New System.ComponentModel.Container()
    Me.EventLog1 = New System.Diagnostics.EventLog()
    Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
    CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).BeginInit()
    '
    'WinLog
    '
    Me.ServiceName = "WinLog"
    CType(Me.EventLog1, System.ComponentModel.ISupportInitialize).EndInit()

  End Sub
  Friend WithEvents EventLog1 As System.Diagnostics.EventLog

  Public Sub New()
    ' Chiamata richiesta dalla finestra di progettazione.
    'InitializeComponent()
    ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().

    MyBase.New()
    InitializeComponent()
    Me.EventLog1 = New System.Diagnostics.EventLog
    If Not System.Diagnostics.EventLog.SourceExists("mgt") Then
      System.Diagnostics.EventLog.CreateEventSource("mgt",
      "WinLogTrace")
    End If
    EventLog1.Source = "mgt"
    EventLog1.Log = "WinLogTrace"

  End Sub
  Friend WithEvents Timer1 As System.Windows.Forms.Timer



End Class
