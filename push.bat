IF  "%1"=="" (	
	GOTO HELL
) ELSE (
	SET _FULLVERSION_=%1
)

.nuget\nuget.exe push _TempNugets\WindowsAppStudio.DataProviders.%_FULLVERSION_%.nupkg %2 -Source %3
.nuget\nuget.exe push _TempNugets\WindowsAppStudio.Common.%_FULLVERSION_%.nupkg %2 -Source %3
.nuget\nuget.exe push _TempNugets\WindowsAppStudio.Uwp.%_FULLVERSION_%.nupkg %2 -Source %3

REM git tag v%_FULLVERSION_%
REM git push --tag


GOTO END

:HELL
ECHO VERSION NOT FOUND
EXIT -1

:END
ECHO PROCESS FINISHED