call "%ProgramFiles(x86)%\Microsoft Visual Studio 14.0\Common7\Tools\VsMSBuildCmd.bat"
.nuget\nuget.exe restore waslibs.sln
msbuild /p:Configuration=Release waslibs.sln
REM git checkout .