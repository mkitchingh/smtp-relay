[Setup]
AppName=SMTP Relay
AppVersion=1.0
DefaultDirName={pf}\SMTP Relay
OutputDir=.
OutputBaseFilename=SmtpRelaySetup
Compression=lzma
SolidCompression=yes

[Files]
Source: "output\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\SMTP Relay Config"; Filename: "{app}\SmtpRelay.GUI.exe"

[Run]
Filename: "{app}\SmtpRelayService.exe"; Parameters: "--install"; StatusMsg: "Installing service..."

[UninstallRun]
Filename: "{app}\SmtpRelayService.exe"; Parameters: "--uninstall"; RunOnceId: "RemoveService"
