IF  "%1"=="" (	
	GOTO HELL
) ELSE (
	SET _FULLVERSION_=%1
)

IF "%2"=="" (
	  ECHO NUGET Key not found. Not publishing
) ELSE (
	.nuget\nuget.exe push _TempNugets\WindowsAppStudio.DataProviders.%_FULLVERSION_%.nupkg %2 -Source %3
	.nuget\nuget.exe push _TempNugets\WindowsAppStudio.Common.%_FULLVERSION_%.nupkg %2 -Source %3
	.nuget\nuget.exe push _TempNugets\WindowsAppStudio.Controls.%_FULLVERSION_%.nupkg %2 -Source %3
	
)

GOTO END

:HELL
ECHO VERSION NOT FOUND
EXIT

:END
ECHO PROCESS FINISHED