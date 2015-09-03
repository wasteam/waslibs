IF  "%1"=="" (	
	SET _VERSION_="0.0"
) ELSE (
	SET _VERSION_=%1
)

IF EXIST _TempNugets rd /q /s _TempNugets

md _TempNugets

call build.bat waslibs.sln

.nuget\nuget.exe pack src\AppStudio.DataProviders\AppStudio.DataProviders.nuspec -OutPutDirectory _TempNugets -Version %_VERSION_% -Prop Configuration=Release
.nuget\nuget.exe pack src\AppStudio.Common\AppStudio.Common.nuspec -OutPutDirectory _TempNugets -Version %_VERSION_% -Prop Configuration=Release

.nuget\nuget update waslibs.controls.sln -RepositoryPath _TempNugets

call build.bat waslibs.controls.sln
.nuget\nuget.exe pack src\AppStudio.Controls\AppStudio.Controls.nuspec -OutPutDirectory _TempNugets -Version %_VERSION_%  -Prop Configuration=Release 

