[Setup]
AppName=SMTP Relay
AppVersion=1.0
DefaultDirName={pf}\SMTP Relay
DefaultGroupName=SMTP Relay
OutputBaseFilename=SmtpRelaySetup
OutputDir=.
Compression=lzma
SolidCompression=yes

[Files]
Source: "output\service\*"; DestDir: "{app}\service"; Flags: ignoreversion recursesubdirs
Source: "output\gui\*"; DestDir: "{app}\gui"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\SMTP Relay Config"; Filename: "{app}\gui\SmtpRelay.GUI.exe"

[Run]
Filename: "{app}\gui\SmtpRelay.GUI.exe"; Description: "Launch Config"; Flags: nowait postinstall skipifsilent

[UninstallRun]
Filename: "sc.exe"; Parameters: "stop SMTPRelayService"; RunOnceId: "StopService"
Filename: "sc.exe"; Parameters: "delete SMTPRelayService"; RunOnceId: "DeleteService"
