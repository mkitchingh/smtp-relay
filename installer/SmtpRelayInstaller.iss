[Setup]
AppName=SMTP Relay
AppVersion=1.0
DefaultDirName={pf}\SMTP Relay
DefaultGroupName=SMTP Relay
OutputBaseFilename=SmtpRelaySetup
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin

[Files]
Source: "installer\output\service\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "installer\output\gui\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\SMTP Relay Config"; Filename: "{app}\SmtpRelay.GUI.exe"
Name: "{group}\Uninstall SMTP Relay"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\SmtpRelay.GUI.exe"; Flags: postinstall nowait skipifsilent

[UninstallRun]
Filename: "sc.exe"; Parameters: "stop SMTPRelay"; Flags: runhidden
Filename: "sc.exe"; Parameters: "delete SMTPRelay"; Flags: runhidden