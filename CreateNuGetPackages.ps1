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
	[string]$PackagesFeed="",
	[Parameter(Mandatory=$False,Position=9)]
	[string]$GitUser = "",
	[Parameter(Mandatory=$False,Position=10)]
	[string]$GitAccessToken = ""
)

if($error){
	$error.Clear()
}

if($Patch -ne "" -and $TfsBuildNumber -ne ""){
	throw "Is not allowed to specify the parameter TfsBuildNumber and Patch at the same time"
}

if($Patch -eq "" -and $TfsBuildNumber -eq ""){
	throw "You must specify the parameter TfsBuildNumber or Patch"
}
if($TfsBuildNumber -ne ""){
	if($Revision -ne "" -or $Semantic -ne "") {
		throw "Semantic and/or Revision can't be specified if you set the BuildName parameter"
	}
	else{
		Write-Host "Infering version from BuildName $TfsBuildNumber"
		$TfsBuildNumberRegEx = "_(\d\d\d\d\d\d\d\d)\.(\d+)"
		
		if($TfsBuildNumber -match $TfsBuildNumberRegEx){
			$buildDate = [DateTime]::ParseExact($matches[1], "yyyyMMdd", $null)
			$BuildMonthDay = $buildDate.ToString("Mdd")
			$BuildRevision = $matches[2]
			Write-Host "Build Month and Day: $BuildMonthDay"
			Write-Host "Build Revision: $BuildRevision"		
			
			$NewVersion = $MayorVersion + "." + $MinorVersion + "." + $BuildMonthDay
			$Semantic = "" #Ensure No Semantic Version for CI Builds
			$Revision = $BuildRevision
		}
		else{
			throw "Build format does not match the expected pattern (buildName_YYYYMMDD.r)"
		}
	}
}
else{
	$NewVersion = $MayorVersion + "." + $MinorVersion + "." + $Patch 
}

if(!$error -and $NewVersion -and $NewVersion -ne ""){
	$PackageVersion = $NewVersion
	if($Semantic -ne "") {
		$PackageVersion = $PackageVersion + "-" + $Semantic
	}
	

	Write-Host "New Version: $NewVersion"
	Write-Host "Revision: $Revision"
	Write-Host "Package Version: $PackageVersion"
    Invoke-Command -ScriptBlock  { .\UpdateVersionFiles.ps1 $NewVersion $Semantic $Revision }
	
	Write-Host "Buiding waslibs.sln"
	Invoke-Command { .\build.bat  } 
	
	Write-Host "Creating nuget packages..."
	Invoke-Command { .\pack.bat $PackageVersion }
	
	if($PackagesFeedKey -eq ""){
		Write-Warning "NuGet feed key not present. Not publishing."
	}
	else{
		Invoke-Command { .\push.bat $PackageVersion $PackagesFeedKey $PackagesFeed }
		
		if($GitUser -ne "" -and $GitAccessToken -ne ""){
			#Write-Host "Invoke-Command { .\versiontag.bat $PackageVersion $Revision $GitUser $GitAccessToken } -ErrorAction Ignore"
			#Invoke-Command { .\versiontag.bat $PackageVersion $Revision $GitUser $GitAccessToken } -ErrorAction Ignore
		}
	}
}
else{
	throw "New version for packages can't be determined."
}