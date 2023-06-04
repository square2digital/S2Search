# Inject variables into config files
#
# All non-secret variables will be automatically passed as environment variables by the DevOps release pipeline
#
# When variables are turned into environment vraiables, variable names become uppercase and . become underscores
# for example, the variable name any.variable becomes the variable name $ANY_VARIABLE
#
# secret variables must be explicitly passed as environment variables in the DevOps powershell task and should be
# named as per the above convention
#
# Placeholders in config files should include the environment specifier e.g. $env:ANY_VARIABLE

# list the files in the current directory
Write-Host "listing files in current directory - $($PSScriptRoot)\*"
Get-ChildItem -Path $PSScriptRoot

Write-Host "$($PSScriptRoot)\*"

# get the config files from the current working directory
$files = Get-ChildItem -Path "$($PSScriptRoot)\*" -Include "*.json", "*.config", "*.yaml", "*.yml"
$fileCount = 0

#Loop through the config files
foreach ($file in $files) {
    $fullname = $file.FullName
    Write-Host "Injecting settings into: $fullName"
    $fileCount++

    # Expand settings into the config file & save
    $content = Get-Content -Path $fullname -Raw
    $content = $ExecutionContext.InvokeCommand.ExpandString($content)
    Set-Content -Path $fullname $content
}

# return how many files were updated
if ($fileCount -eq 0) {
    Write-Host "No files found in working directory $(Get-Location)"
    exit 1
}