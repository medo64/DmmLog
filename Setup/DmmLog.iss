#define AppName        GetStringFileInfo('..\Binaries\DmmLog.exe', 'ProductName')
#define AppVersion     GetStringFileInfo('..\Binaries\DmmLog.exe', 'ProductVersion')
#define AppFileVersion GetStringFileInfo('..\Binaries\DmmLog.exe', 'FileVersion')
#define AppCompany     GetStringFileInfo('..\Binaries\DmmLog.exe', 'CompanyName')
#define AppCopyright   GetStringFileInfo('..\Binaries\DmmLog.exe', 'LegalCopyright')
#define AppBase        LowerCase(StringChange(AppName, ' ', ''))
#define AppSetupFile   AppBase + StringChange(AppVersion, '.', '')

[Setup]
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppCompany}
AppPublisherURL=http://www.jmedved.com/{#AppBase}/
AppCopyright={#AppCopyright}
VersionInfoProductVersion={#AppVersion}
VersionInfoProductTextVersion={#AppVersion}
VersionInfoVersion={#AppFileVersion}
DefaultDirName={pf}\{#AppCompany}\{#AppName}
OutputBaseFilename={#AppSetupFile}
OutputDir=..\Releases
SourceDir=..\Binaries
AppId=JosipMedved_DmmLog
CloseApplications="yes"
RestartApplications="no"
UninstallDisplayIcon={app}\DmmLog.exe
AlwaysShowComponentsList=no
ArchitecturesInstallIn64BitMode=x64
DisableProgramGroupPage=yes
MergeDuplicateFiles=yes
MinVersion=0,6.01.7200
PrivilegesRequired=admin
ShowLanguageDialog=no
SolidCompression=yes
ChangesAssociations=yes
DisableWelcomePage=yes
LicenseFile=..\Setup\License.txt


[Files]
Source: "DmmLog.exe";               DestDir: "{app}";                      Flags: ignoreversion;
Source: "DmmLogBaseDriver.dll";     DestDir: "{app}";                      Flags: ignoreversion;
Source: "DmmLogAgilentDriver.dll";  DestDir: "{app}";                      Flags: ignoreversion;
Source: "ReadMe.txt";               DestDir: "{app}";  Attribs: readonly;  Flags: overwritereadonly uninsremovereadonly;


[Icons]
Name: "{userstartmenu}\DMM Log";  Filename: "{app}\DmmLog.exe"


[Registry]
Root: HKCU;  Subkey: "Software\Josip Medved";          ValueType: none;                                                                        Flags: uninsdeletekeyifempty;
Root: HKCU;  Subkey: "Software\Josip Medved\DMM Log";  ValueType: none;    ValueName: "Installed";                                             Flags: deletevalue uninsdeletevalue;
Root: HKLM;  Subkey: "Software\Josip Medved\DMM Log";  ValueType: dword;   ValueName: "Installed";  ValueData: "1";                            Flags: uninsdeletekey;

Root: HKCR;  Subkey: ".dmmlog";                        ValueType: string;  ValueName: "";           ValueData: "DmmLogFile";                   Flags: uninsdeletevalue
Root: HKCR;  Subkey: "DmmLogFile";                     ValueType: string;  ValueName: "";           ValueData: "DMM Log File";                 Flags: uninsdeletekey
Root: HKCR;  Subkey: "DmmLogFile\shell\open\command";  ValueType: string;  ValueName: "";           ValueData: """{app}\DmmLog.exe"" ""%1""";


[Run]
Filename: "{app}\DmmLog.exe";  Flags: postinstall nowait skipifsilent runasoriginaluser unchecked;            Description: "Launch application now";
Filename: "{app}\ReadMe.txt";  Flags: postinstall runasoriginaluser shellexec nowait skipifsilent unchecked;  Description: "View ReadMe.txt";
