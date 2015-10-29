IF  "%1"=="" (	
	GOTO HELL
) ELSE (
	SET _FULLVERSION_=%1
)

IF EXIST _TempNugets del /q /s _TempNugets\WindowsAppStudio*
IF EXIST _TempNugetsSymbols del /q /s _TempNugetsSymbols\WindowsAppStudio*

IF NOT EXIST _TempNugets md _TempNugets
IF NOT EXIST _TempNugetsSymbols md _TempNugetsSymbols


.nuget\nuget.exe pack src\AppStudio.DataProviders\AppStudio.DataProviders.nuspec -OutPutDirectory _TempNugets -Version %_FULLVERSION_% -Prop Configuration=Release
.nuget\nuget.exe pack src\AppStudio.Common\AppStudio.Common.nuspec -OutPutDirectory _TempNugets -Version %_FULLVERSION_% -Prop Configuration=Release
.nuget\nuget.exe pack src\AppStudio.Uwp\AppStudio.Uwp.nuspec -OutPutDirectory _TempNugets -Version %_FULLVERSION_% -Prop Configuration=Release

.nuget\nuget.exe pack src\AppStudio.DataProviders\AppStudio.DataProviders.nuspec -OutPutDirectory _TempNugetsSymbols -Symbols -Version %_FULLVERSION_% -Prop Configuration=Release
.nuget\nuget.exe pack src\AppStudio.Common\AppStudio.Common.nuspec -OutPutDirectory _TempNugetsSymbols -Symbols -Version %_FULLVERSION_% -Prop Configuration=Release
.nuget\nuget.exe pack src\AppStudio.Uwp\AppStudio.Uwp.nuspec -OutPutDirectory _TempNugetsSymbols -Symbols -Version %_FULLVERSION_% -Prop Configuration=Release

GOTO END

:HELL
ECHO VERSION NOT FOUND
EXIT -1

:END
ECHO PROCESS FINISHED