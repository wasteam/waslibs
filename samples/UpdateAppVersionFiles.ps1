# Enable -Verbose option
[CmdletBinding()] 
Param(
	[Parameter(Mandatory=$True,Position=1)]
	[string]$NewVersion, #Expected Mayor.Minor.Patch
    [Parameter(Mandatory=$False,Position=2)]
	[string]$Revision = ""

)

# Regular expression pattern to find the version in the build number 
# and then apply it to the assemblies
$VersionRegex = "\d+\.\d+\.\d+\.\d+"

$ManifestVersionRegex = " Version=""\d+\.\d+\.\d+\.\d+"""

if($NewVersion + "." + $Revision -match $VersionRegex){
    $ScriptPath = $null
    try
    {
        $ScriptPath = (Get-Variable MyInvocation).Value.MyCommand.Path
        $ScriptDir = Split-Path -Parent $ScriptPath
    }
    catch {}

    if (!$ScriptPath)
    {
        Write-Error "Current path not found!"
        exit 1
    }


    if($Revision -ne ""){
        $AssemblyVersion=$NewVersion + "." + $Revision
    }

    $tempManifestVersion = $NewVersion + ".0"
    $ManifestVersion = " Version=""$tempManifestVersion"""


    Write-Host "Version: $AssemblyVersion"
    Write-Host "Manifest: $ManifestVersion"
    Write-Host "ScriptDir: " $ScriptDir

    # Apply the version to the assembly property files
    $assemblyInfoFiles = gci $ScriptDir -recurse -include "*Properties*","My Project" | 
        ?{ $_.PSIsContainer } | 
        foreach { gci -Path $_.FullName -Recurse -include AssemblyInfo.* }

    if($assemblyInfoFiles)
    {
        Write-Host "Will apply $AssemblyVersion to $($assemblyInfoFiles.count) Assembly Info Files."

        foreach ($file in $assemblyInfoFiles) {
            $filecontent = Get-Content($file)
            attrib $file -r
            $filecontent -replace $VersionRegex, $AssemblyVersion | Out-File $file utf8

            Write-Host "$file.FullName - version applied"
        }
    }
    else
    {
        Write-Warning "No Assembly Info Files found."
    }

    # Try Manifests
    $manifestFiles = gci .\ -recurse -include "Package.appxmanifest" 

    if($manifestFiles)
    {
        Write-Host "Will apply $ManifestVersion to $($manifestFiles.count) Manifests."

        foreach ($file in $manifestFiles) {
            $filecontent = Get-Content($file)
            attrib $file -r
            $filecontent -replace $ManifestVersionRegex, $ManifestVersion | Out-File $file utf8

            Write-Host "$file.FullName - version applied to Manifest"
        }
    }
    else
    {
        Write-Warning "No Manifest files found."
    }
}
else{
    throw "Version parameters specified do not match the expected version pattern."
}
