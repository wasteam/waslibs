
IF  "%1"=="" (	
	GOTO HELL
) ELSE (
	SET _FULLVERSION_=%1
)

git remote set-url origin https://%2:%3@github.com/wasteam/waslibs.git
git tag v%_FULLVERSION_%
git push --tag



GOTO END

:HELL
ECHO VERSION NOT FOUND
EXIT -1

:END
ECHO PROCESS FINISHED
