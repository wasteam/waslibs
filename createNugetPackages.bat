IF  "%1"=="" (	
	SET _VERSION_="0.0"
) ELSE (
	SET _VERSION_=%1
)

REM call build.bat

IF NOT EXIST _TempNugets md _TempNugets
.nuget\nuget.exe pack src\AppStudio.DataProviders\AppStudio.DataProviders.nuspec -OutPutDirectory _TempNugets -Version %_VERSION_% -Prop Configuration=Release
.nuget\nuget.exe pack src\AppStudio.Common\AppStudio.Common.nuspec -OutPutDirectory _TempNugets -Version %_VERSION_% -Prop Configuration=Release
.nuget\nuget.exe pack src\AppStudio.Controls\AppStudio.Controls.nuspec -OutPutDirectory _TempNugets -Version %_VERSION_%  -Prop Configuration=Release 

