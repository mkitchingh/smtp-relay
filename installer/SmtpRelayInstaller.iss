[Setup]
AppName=SMTP Relay
AppVersion=1.0
DefaultDirName={pf}\SMTP Relay
DefaultGroupName=SMTP Relay
OutputBaseFilename=SmtpRelaySetup
Compression=lzma
SolidCompression=yes

[Files]
Source: "output\service\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "output\gui\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\SMTP Relay Config"; Filename: "{app}\SmtpRelay.GUI.exe"

[Run]
Filename: "{app}\SmtpRelay.GUI.exe"; Description: "Launch SMTP Relay Config"; Flags: postinstall skipifsilent

[UninstallRun]
Filename: "sc.exe"; Parameters: "stop SMTPRelay"; RunOnceId: "StopService"
Filename: "sc.exe"; Parameters: "delete SMTPRelay"; RunOnceId: "DeleteService"