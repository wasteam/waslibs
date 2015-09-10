IF  "%1"=="" (	
	GOTO HELL
) ELSE (
	SET _VERSION_=%1
)

SET _SEMANTIC_="%2"
IF  "%_SEMANTIC_%"=="" (	
	SET _FULLVERSION_=%_VERSION_%
) ELSE (
	SET _FULLVERSION_=%_VERSION_%-%_SEMANTIC_%
)

powershell.exe -ExecutionPolicy Bypass -Command .\UpdateVersionFiles.ps1 %_VERSION_% %_SEMANTIC_%

.nuget\nuget.exe restore waslibs.sln
msbuild /p:Configuration=Release waslibs.sln

call pack.bat %_FULLVERSION_%

IF "%3"=="" (
	  ECHO NUGET Key not found. Not publishing
) ELSE (
	call push.bat %_FULLVERSION_% %3 %4
)

GOTO END

:HELL
ECHO VERSION NOT FOUND
EXIT

:END
git checkout .
git tag v%_FULLVERSION_%
git push --tag
ECHO PROCESS FINISHED
