IF  "%1"=="" (	
	GOTO HELL
) ELSE (
	SET _FULLVERSION_=%1
)

.nuget\nuget.exe push _TempNugets\WindowsAppStudio.DataProviders.%_FULLVERSION_%.nupkg %2 -Source %3
.nuget\nuget.exe push _TempNugets\WindowsAppStudio.Common.%_FULLVERSION_%.nupkg %2 -Source %3
.nuget\nuget.exe push _TempNugets\WindowsAppStudio.Uwp.%_FULLVERSION_%.nupkg %2 -Source %3


.nuget\nuget.exe push _TempNugetsSymbols\WindowsAppStudio.DataProviders.%_FULLVERSION_%.symbols.nupkg %2 -Source %4
.nuget\nuget.exe push _TempNugetsSymbols\WindowsAppStudio.Common.%_FULLVERSION_%.symbols.nupkg %2 -Source %4
.nuget\nuget.exe push _TempNugetsSymbols\WindowsAppStudio.Uwp.%_FULLVERSION_%.symbols.nupkg %2 -Source %4


GOTO END

:HELL
ECHO VERSION NOT FOUND
EXIT -1

:END
ECHO PROCESS FINISHED