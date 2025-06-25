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
Filename: "{app}\SmtpRelay.exe"; Parameters: "--install"; StatusMsg: "Installing service..."; Flags: runhidden

[UninstallRun]
Filename: "{app}\SmtpRelay.exe"; Parameters: "--uninstall"; StatusMsg: "Uninstalling service..."; Flags: runhidden
