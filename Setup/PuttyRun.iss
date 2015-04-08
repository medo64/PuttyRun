#define AppName        GetStringFileInfo('..\Binaries\PuttyRun.exe', 'ProductName')
#define AppVersion     GetStringFileInfo('..\Binaries\PuttyRun.exe', 'ProductVersion')
#define AppFileVersion GetStringFileInfo('..\Binaries\PuttyRun.exe', 'FileVersion')
#define AppCompany     GetStringFileInfo('..\Binaries\PuttyRun.exe', 'CompanyName')
#define AppCopyright   GetStringFileInfo('..\Binaries\PuttyRun.exe', 'LegalCopyright')
#define AppBase        LowerCase(StringChange(AppName, ' ', ''))
#define AppSetupFile   AppBase + StringChange(AppVersion, '.', '')

#define AppVersionEx   StringChange(AppVersion, '0.00', '')
#if "" != HgNode
#  define AppVersionEx AppVersionEx + " (" + HgNode + ")"
#endif


[Setup]
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppCompany}
AppPublisherURL=http://jmedved.com/{#AppBase}/
AppCopyright={#AppCopyright}
VersionInfoProductVersion={#AppVersion}
VersionInfoProductTextVersion={#AppVersionEx}
VersionInfoVersion={#AppFileVersion}
DefaultDirName={pf}\{#AppCompany}\{#AppName}
OutputBaseFilename={#AppSetupFile}
OutputDir=..\Releases
SourceDir=..\Binaries
AppId=JosipMedved_PuttyRun
CloseApplications="yes"
RestartApplications="no"
UninstallDisplayIcon={app}\PuttyRun.exe
AlwaysShowComponentsList=no
ArchitecturesInstallIn64BitMode=x64
DisableProgramGroupPage=yes
MergeDuplicateFiles=yes
MinVersion=0,5.1
PrivilegesRequired=admin
ShowLanguageDialog=no
SolidCompression=yes
ChangesAssociations=no
DisableWelcomePage=yes
LicenseFile=..\Setup\License.rtf


[Messages]
SetupAppTitle=Setup {#AppName} {#AppVersionEx}
SetupWindowTitle=Setup {#AppName} {#AppVersionEx}
BeveledLabel=jmedved.com

[Dirs]
Name: "{userappdata}\Josip Medved\PuttyRun";  Flags: uninsalwaysuninstall

[Files]
Source: "PuttyRun.exe";   DestDir: "{app}";                      Flags: ignoreversion;
Source: "PuttyRun.pdb";   DestDir: "{app}";                      Flags: ignoreversion;
Source: "ReadMe.txt";  DestDir: "{app}";  Attribs: readonly;  Flags: overwritereadonly uninsremovereadonly;

[Icons]
Name: "{userstartmenu}\PuttyRun";  Filename: "{app}\PuttyRun.exe"

[Registry]
Root: HKLM;  Subkey: "Software\Josip Medved\PuttyRun";                 ValueType: none;    ValueName: "Installed";                                              Flags: deletevalue uninsdeletevalue
Root: HKCU;  Subkey: "Software\Josip Medved\PuttyRun";                 ValueType: dword;   ValueName: "Installed";  ValueData: "1";                             Flags: uninsdeletekey
Root: HKCU;  Subkey: "Software\Josip Medved";                                                                                                                   Flags: uninsdeletekeyifempty
Root: HKCU;  Subkey: "Software\Microsoft\Windows\CurrentVersion\Run";  ValueType: string;  ValueName: "PuttyRun";   ValueData: """{app}\PuttyRun.exe"" /hide";  Flags: uninsdeletevalue

[Run]
Description: "View ReadMe.txt";         Filename: "{app}\ReadMe.txt";                         Flags: postinstall runasoriginaluser shellexec nowait skipifsilent unchecked
Description: "Launch application now";  Filename: "{app}\PuttyRun.exe";   Parameters: "/setup";  Flags: postinstall nowait skipifsilent runasoriginaluser shellexec

[Code]

procedure InitializeWizard;
begin
  WizardForm.LicenseAcceptedRadio.Checked := True;
end;
