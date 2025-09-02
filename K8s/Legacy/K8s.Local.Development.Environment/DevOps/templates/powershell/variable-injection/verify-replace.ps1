# verify that all variables have been injected into config files

# Placeholders in config files should include the environment specifier e.g $env:ANY_VARIABLE
# so this script will look for any "$env:" values remaining in the files

$foundPlaceholders = $false
$placeholderPattern = '$env:'

$files = Get-ChildItem -Path "$($PSScriptRoot)\*" -Include "*.json", "*.config", "*.yaml", "*.yml"
$fileCount = 0

#Loop through the config files
foreach ($file in $files) {
    $fullname = $file.FullName
    Write-Host "Verifying settings placeholders replaced in: $fullName"
    $fileCount++

    $variableMatches = Select-String -Path $fullName $placeholderPattern -SimpleMatch

    if ($variableMatches) {
        #Found some matches
        $foundPlaceholders = $true

        #display each match
        foreach ($match in $variableMatches) {
            Write-Host " Line $($match.LineNumber): $($match.Line.Trim())"
        }
    }
    else {        
        # no matches found
        Write-Host " no settings placeholders found"
    }

    Write-Host ""
}

# return how many files were updated
if ($fileCount -eq 0) {
    Write-Host "No files found in working directory $(Get-Location)"
    exit 1
}

# return how many files were updated
if ($foundPlaceholders) {
    Write-Error "Found settings placeholders that have not been replaced"
    exit 1
}

Write-Host "Verification Complete"
Write-Host ""