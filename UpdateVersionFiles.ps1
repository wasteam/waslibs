# Enable -Verbose option
[CmdletBinding()] 
Param(
	[Parameter(Mandatory=$True,Position=1)]
	[string]$NewVersion,
	[Parameter(Mandatory=$False,Position=2)]
	[string]$Semantic = ""

)

# Regular expression pattern to find the version in the build number 
# and then apply it to the assemblies
$VersionRegex = "\d+\.\d+\.\d+\.\d+"
$InformationalVersionRegex = "\d+\.\d+\.\d+\-(\w+)"


$ScriptPath = $null
try
{
    $ScriptPath = (Get-Variable MyInvocation).Value.MyCommand.Path
    $ScriptDir = Split-Path -Parent $ScriptPath
    $ScriptDir = Join-Path $ScriptDir "src"
}
catch {}

if (!$ScriptPath)
{
    Write-Error "Current path not found!"
	exit 1
}

if($Semantic -eq ""){
	$NewInformationalVersion = $NewVersion
}
else{
	$NewInformationalVersion = $NewVersion  + "-" + $Semantic
}

$NewVersion=$NewVersion + ".0"

Write-Host "Version: $NewVersion"
Write-Host "Informational Version: $NewInformationalVersion"
Write-Host "ScriptDir: " $ScriptDir

# Apply the version to the assembly property files
$files = gci $ScriptDir -recurse -include "*Properties*","My Project" | 
    ?{ $_.PSIsContainer } | 
    foreach { gci -Path $_.FullName -Recurse -include AssemblyInfo.* }

if($files)
{
    Write-Host "Will apply $NewVersion to $($files.count) files."

    foreach ($file in $files) {
        $filecontent = Get-Content($file)
        attrib $file -r
        $filecontent -replace $VersionRegex, $NewVersion | %{ $_ -replace $InformationalVersionRegex, $NewInformationalVersion } | Out-File $file

        Write-Host "$file.FullName - version applied"
    }
}
else
{
    Write-Warning "Found no files."
}


$projectJsonRegex = "WindowsAppStudio\.Common.*\d+\.\d+\.\d+\-(\w+)"
$projectJsonFile = Join-Path $ScriptDir "AppStudio.Controls\project.json"
$newReference = 'WindowsAppStudio.Common": "' + $NewInformationalVersion
$jsonContent = Get-Content($projectJsonFile)
attrib $projectJsonFile -r
$jsonContent -replace $projectJsonRegex, $newReference| Out-File $projectJsonFile




