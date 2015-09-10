IF  "%1"=="" (	
	GOTO HELL
) ELSE (
	SET _VERSION_=%1
)

IF  "%2"=="" (	
	SET _FULLVERSION_=%_VERSION_%
) ELSE (
	SET _FULLVERSION_=%_VERSION_%-%2%
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

IF "%3"=="" (
	  ECHO NUGET Key not found. Not publishing
) ELSE (
	.nuget\nuget.exe push _TempNugets\WindowsAppStudio.DataProviders.%_FULLVERSION_%.nupkg %3 -Source %4
	.nuget\nuget.exe push _TempNugets\WindowsAppStudio.Common.%_FULLVERSION_%.nupkg %3 -Source %4
	.nuget\nuget.exe push _TempNugets\WindowsAppStudio.Controls.%_FULLVERSION_%.nupkg %3 -Source %4
)

GOTO END

:HELL
ECHO VERSION NOT FOUND
EXIT

:END

git tag v%_FULLVERSION_%
git push --tag
ECHO PROCESS FINISHED
