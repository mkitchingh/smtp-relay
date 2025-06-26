[Setup]
AppName=SMTP Relay
AppVersion=1.0
DefaultDirName={pf}\SMTP Relay
DefaultGroupName=SMTP Relay
OutputDir=.
OutputBaseFilename=SmtpRelaySetup
Compression=lzma
SolidCompression=yes

[Files]
Source: "output\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\SMTP Relay Config"; Filename: "{app}\SmtpRelay.GUI.exe"
Name: "{group}\Uninstall SMTP Relay"; Filename: "{uninstallexe}"

[Run]
Filename: "sc.exe"; Parameters: "create SmtpRelay binPath= "{app}\SmtpRelay.exe" start= auto"; Flags: runhidden
Filename: "sc.exe"; Parameters: "start SmtpRelay"; Flags: runhidden

[UninstallRun]
Filename: "sc.exe"; Parameters: "stop SmtpRelay"; Flags: runhidden
Filename: "sc.exe"; Parameters: "delete SmtpRelay"; Flags: runhidden
