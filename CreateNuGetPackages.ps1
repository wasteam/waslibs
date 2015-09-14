[CmdletBinding()] 
Param(
	[Parameter(Mandatory=$True,Position=1)]
	[string]$MayorVersion,
	[Parameter(Mandatory=$True,Position=2)]
	[string]$MinorVersion,
	[Parameter(Mandatory=$False,Position=3)]
	[String]$Patch,
	[Parameter(Mandatory=$False,Position=4)]
	[string]$Semantic = "",
    [Parameter(Mandatory=$False,Position=5)]
	[string]$Revision = "",
	[Parameter(Mandatory=$False, Position=6)]
    [string]$TfsBuildNumber="", #Must follow the pattern buildName_YYYYMMDD.r
	[Parameter(Mandatory=$False,Position=7)]
	[string]$PackagesFeedKey ="",
	[Parameter(Mandatory=$False,Position=8)]
	[string]$PackagesFeed=""
)


if($Patch -ne "" -and $TfsBuildNumber -ne ""){
	Write-Error "Is not allowed to specify the parameter BuildVersion and BuildName at the same time"
}

if($Patch -eq "" -and $TfsBuildNumber -eq ""){
	Write-Error "You must specify the parameter BuildVersion or BuildName"
}
if($TfsBuildNumber -ne ""){
	if($Revision -ne "" -or $Semantic -ne "") {
		Write-Error "Semantic and/or Revision can't be specified if you set the BuildName parameter"
	}
	else{
		Write-Host "Infering version from BuildName $TfsBuildNumber"
		$TfsBuildNumberRegEx = "_\d\d\d\d(\d\d\d\d)\.(\d+)"
		if($TfsBuildNumber -match $TfsBuildNumberRegEx){
			$BuildMonthDay = $matches[1]
			$BuildRevision = $matches[2]
			Write-Host "Build Month and Day: $BuildMonthDay"
			Write-Host "Build Revision: $BuildRevision"		
			
			$NewVersion = $MayorVersion + "." + $MinorVersion + "." + $BuildMonthDay
			$Semantic = "" #Ensure No Semantic Version for CI Builds
			$Revision = $BuildRevision
		}
		else{
			Write-Error "Build format does not match the expected pattern (buildName_YYYYMMDD.r)"
		}
	}
}
else{
	$NewVersion = $MayorVersion + "." + $MinorVersion + "." + $Patch 
}

if($NewVersion -and $NewVersion -ne ""){
	$FullVersion = $NewVersion
	if($Semantic -ne "") {
		$FullVersion = $FullVersion + "-" + $Semantic
	}
	
	if($Revision -ne ""){
		$FullVersion = $FullVersion + $Revision
	}

	Write-Host "New Version: $NewVersion"
	Write-Host "Full Version: $FullVersion"
    Invoke-Command -ScriptBlock  { .\UpdateVersionFiles.ps1 $NewVersion $Semantic $Revision }
	
	Write-Host "Buiding waslibs.sln"
	Invoke-Command { .\build.bat  } 
	
	Write-Host "Creating nuget packages..."
	Invoke-Command { .\pack.bat $FullVersion }
	
	if($PackagesFeedKey -eq ""){
		Write-Warning "NuGet feed key not present. Not publishing."
	}
	else{
		Invoke-Command { .\push.bat $FullVersion $PackagesFeedKey $PackagesFeed }
	}
}
else{
	Write-Error "New version for packages can't be determined."
}