IF  "%1"=="" (	
	SET _VERSION_="0.0"
) ELSE (
	SET _VERSION_=%1
)

IF  "%2"=="" (	
	SET _FULLVERSION_=%_VERSION_%
) ELSE (
	SET _SEMANTIC_=%2
	SET _FULLVERSION_=%_VERSION_%-%_SEMANTIC_%
)

IF EXIST _TempNugets del /q /s _TempNugets\WindowsAppStudio*

IF NOT EXIST _TempNugets md _TempNugets

powershell.exe -ExecutionPolicy Bypass -Command .\UpdateVersionFiles.ps1 %_VERSION_% %_SEMANTIC_%

call build.bat waslibs.sln

.nuget\nuget.exe pack src\AppStudio.DataProviders\AppStudio.DataProviders.nuspec -OutPutDirectory _TempNugets -Version %_FULLVERSION_% -Prop Configuration=Release
.nuget\nuget.exe pack src\AppStudio.Common\AppStudio.Common.nuspec -OutPutDirectory _TempNugets -Version %_FULLVERSION_% -Prop Configuration=Release

.nuget\nuget.exe restore -Source %cd%\_TempNugets waslibs.Controls.sln
call build.bat waslibs.controls.sln

.nuget\nuget.exe pack src\AppStudio.Controls\AppStudio.Controls.nuspec -OutPutDirectory _TempNugets -Version %_FULLVERSION_%  -Prop Configuration=Release 

