##-----------------------------------------------------------------------
## <copyright file="ApplyVersionToAssemblies.ps1">(c) http://TfsBuildExtensions.codeplex.com/. This source is subject to the Microsoft Permissive License. See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx. All other rights reserved.</copyright>
##-----------------------------------------------------------------------
# Look for a 0.0.0.0 pattern in the build number. 
# If found use it to version the assemblies.
#
# For example, if the 'Build number format' build process parameter 
# $(BuildDefinitionName)_$(Year:yyyy).$(Month).$(DayOfMonth)$(Rev:.r)
# then your build numbers come out like this:
# "Build HelloWorld_2013.07.19.1"
# This script would then apply version 2013.07.19.1 to your assemblies.
	
# Enable -Verbose option
[CmdletBinding()]
# Disable parameter
# Convenience option so you can debug this script or disable it in 
# your build definition without having to remove it from
# the 'Post-build script path' build process parameter.
param(
    [Parameter(Mandatory = $False, Position=1)]
    [string]$packageIdentityName= "Microsoft.WindowsAppStudioInstaller",
    [Parameter(Mandatory = $False, Position=2)]
    [String]$appDisplayName = "Windows App Studio Installer",
    [Parameter(Mandatory = $False, Position=1)]
    [string]$publisherInfo = "CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US",
    [Parameter(Mandatory = $False, Position=2)]
    [string]$publisherDisplayName = "Microsoft Corporation",
    [switch]$Disable
)
if ($PSBoundParameters.ContainsKey('Disable'))
{
	Write-Host "Script disabled; no actions will be taken on the files."
}
	
$PackageIdentityNameRegex = 'Identity[ ]+Name=".+"[ ]+Publisher';
$ProductionPackageIdentityName = "Identity Name=""$packageIdentityName"" Publisher";

$AppDisplayNameRegex = '<DisplayName>.+</DisplayName>';
$ProductionAppDisplayName = "<DisplayName>$appDisplayName</DisplayName>";

$PublisherRegex = 'Publisher=".+"[ ]+Version';
$ProductionPublisherInfo = "Publisher=""$publisherInfo"" Version";

$PublisherDisplayNameRegex = "<PublisherDisplayName>.+</PublisherDisplayName>";
$ProductionPublisherDisplayName = "<PublisherDisplayName>$publisherDisplayName</PublisherDisplayName>";
	
	
# Apply the version to the assembly property files
$files = gci .\ -recurse -include "Package.appxmanifest" 

if($files)
{
	Write-Host "Will apply $publisherInfo to application manifest."
	
	foreach ($file in $files) {
		if(-not $Disable)
		{
			$filecontent = Get-Content($file)
			attrib $file -r
            $packageIdentityNameReplaced = $filecontent -replace $PackageIdentityNameRegex, $ProductionPackageIdentityName
            $appDisplayNameReplaced = $packageIdentityNameReplaced -replace $AppDisplayNameRegex, $ProductionAppDisplayName
			$publisherReplaced =  $appDisplayNameReplaced -replace $PublisherRegex, $ProductionPublisherInfo 
            $publisherReplaced -replace $PublisherDisplayNameRegex, $ProductionPublisherDisplayName | Out-File $file utf8
			Write-Host "Publisher info updated in ""$file"""
            Write-Host "Package Display Identity Name = ""$packageIdentityName"""
            Write-Host "App Display Name = ""$appDisplayName"""
            Write-Host "Publisher = ""$publisherInfo"""
            Write-Host "Publisher Display Name = ""$publisherDisplayName"""
		}
	}
}
else
{
	Write-Warning "Found no files to update application manifest."
}

