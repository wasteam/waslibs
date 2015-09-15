
IF  "%1"=="" (	
	GOTO HELL
) ELSE (
	SET _TAG_=%1
)

IF  NOT "%2"=="" (	
	SET _TAG_=%1.%2
)

git remote set-url origin https://%3:%4@github.com/wasteam/waslibs.git
git tag v%_TAG_%
git push --tag



GOTO END

:HELL
ECHO VERSION NOT FOUND
EXIT -1

:END
ECHO PROCESS FINISHED
