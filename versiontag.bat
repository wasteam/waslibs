
IF  "%1"=="" (	
	GOTO HELL
) ELSE (
	SET _TAG_=v%1
)

IF NOT "%3"=="" (
	SET _TAG_=%_TAG_%-%3%2
)

IF "%3"=="" (
	IF NOT "%2"== "" SET _TAG_=%_TAG_%.%2
)

git config user.email "%4@outlook.com"
git config user.name "%4"
git remote set-url origin https://%4:%5@github.com/wasteam/waslibs.git
git tag -a %_TAG_% -m "Version built: %_TAG_%"
git push origin %_TAG_%

GOTO END

:HELL
ECHO VERSION NOT FOUND
EXIT -1

:END
ECHO PROCESS FINISHED