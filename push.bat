IF  "%1"=="" (	
	GOTO HELL
) ELSE (
	SET _FULLVERSION_=%1
)

.nuget\nuget.exe push _TempNugets\WindowsAppStudio.DataProviders.%_FULLVERSION_%.nupkg %2 -Source %3
.nuget\nuget.exe push _TempNugets\WindowsAppStudio.Common.%_FULLVERSION_%.nupkg %2 -Source %3
.nuget\nuget.exe push _TempNugets\WindowsAppStudio.Uwp.%_FULLVERSION_%.nupkg %2 -Source %3

git remote set-url origin https://ralarcon:0cbc2dc736f2af07d324eb9f47f4e81d13df3197@github.com/wasteam/waslibs.git
git tag v%_FULLVERSION_%
git push --tag


GOTO END

:HELL
ECHO VERSION NOT FOUND
EXIT -1

:END
ECHO PROCESS FINISHED