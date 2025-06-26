
[Setup]
AppName=SMTP Relay
AppVersion=1.0
DefaultDirName={pf}\SMTP Relay
OutputDir=.
OutputBaseFilename=SmtpRelaySetup
Compression=lzma
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: "..\output\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\SMTP Relay Config"; Filename: "{app}\SmtpRelay.GUI.exe"; IconFilename: "{app}\email.ico"
Name: "{group}\Uninstall SMTP Relay"; Filename: "{uninstallexe}"

[Run]
Filename: "sc.exe"; Parameters: "create SmtpRelay binPath= '{app}\SmtpRelay.exe' start= auto"; Flags: runhidden runascurrentuser
Filename: "sc.exe"; Parameters: "start SmtpRelay"; Flags: runhidden runascurrentuser
Filename: "{app}\SmtpRelay.GUI.exe"; Description: "Launch SMTP Relay Config now"; Flags: postinstall skipifsilent

[UninstallRun]
Filename: "sc.exe"; Parameters: "stop SmtpRelay"; Flags: runhidden
Filename: "sc.exe"; Parameters: "delete SmtpRelay"; Flags: runhidden
