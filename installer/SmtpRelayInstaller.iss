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
Source: "..\src\SmtpRelay\bin\Release\net6.0-windows\SmtpRelay.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\src\SmtpRelay.GUI\bin\Release\net6.0-windows\SmtpRelay.GUI.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\SMTP Relay GUI"; Filename: "{app}\SmtpRelay.GUI.exe"
Name: "{group}\Uninstall SMTP Relay"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\SmtpRelay.exe"; Parameters: "--install"; StatusMsg: "Installing service..."; Flags: runhidden waituntilterminated

[UninstallRun]
Filename: "{app}\SmtpRelay.exe"; Parameters: "--uninstall"; StatusMsg: "Uninstalling service..."; Flags: runhidden waituntilterminated
