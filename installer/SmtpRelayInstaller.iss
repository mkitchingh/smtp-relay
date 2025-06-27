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
Source: "installer\output\service\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs
Source: "installer\output\gui\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\SMTP Relay Config"; Filename: "{app}\SmtpRelay.GUI.exe"

[Run]
Filename: "{app}\SmtpRelay.GUI.exe"; Description: "Launch SMTP Relay Config"; Flags: postinstall nowait skipifsilent