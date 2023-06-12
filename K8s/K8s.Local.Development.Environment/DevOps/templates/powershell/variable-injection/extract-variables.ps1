# This script will identify files that contain a placeholder variable and extract them to a text file

$foundPlaceholders = $false
$placeholderPattern = '\$env:(\w+)'

$files = Get-ChildItem -Path "$($PSScriptRoot)\*" -Include "*.json", "*.config", "*.yaml", "*.yml"
$fileCount = 0
$variablesList = New-Object 'System.Collections.Generic.List[String]'

#Loop through the config files
foreach ($file in $files) {
    $fullname = $file.FullName
    Write-Host "Checking for variables to extract in: $fullName"
    $fileCount++
   
    $variableMatches = Select-String -Path $fullName -Pattern $placeholderPattern

    if ($variableMatches) {
        $foundPlaceholders = $true

        #display each match
        foreach ($match in $variableMatches) {
            #set to upper case then replace placeholder to lower for list comparison
            $variableMatch = $match.Matches[0].ToString().ToUpper().Replace('$ENV', '$env') 
            Write-Host "Found variable: $variableMatch"
            
            if (!$variablesList.Contains($variableMatch)) {
                Write-Host "Adding $variableMatch to list"
                $variablesList.Add($variableMatch)
            }
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
    Write-Host "No files found in working directory $($PSScriptRoot)"
    exit 1
}

if ($variablesList.Count -eq 0) {
    Write-Error "No variables found for extraction"
    exit 1
}

Write-Host "Total Variables Found: "$variablesList.Count""
foreach ($variableItem in $variablesList) {
    Write-Host $variableItem
}

if ($variablesList.Count -gt 0) {
    Write-Host "Writing Extracted Variables to file: $($PSScriptRoot)\VariablesToValidate.txt"
    Set-Content -Path "$($PSScriptRoot)\VariablesToValidate.txt" -Value $variablesList
}

Write-Host "Extraction Complete"
Write-Host ""