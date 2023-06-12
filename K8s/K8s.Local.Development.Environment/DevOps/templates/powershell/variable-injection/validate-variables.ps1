# this script will validate that the DevOps Environment settings have been correctly configured ready for the appsettings to be replaced

$variablesFromFile = Get-Content -Path "$($PSScriptRoot)\VariablesToValidate.txt"

$validationFailed = $false

Write-Host "Validating Environment Variables"

foreach ($variable in $variablesFromFile) {
    $variableValue = $ExecutionContext.InvokeCommand.ExpandString($variable)
    #Write-Host "Name: $variable | Value: $variableValue"
    #Write-Host "Name: $variable"
    #Write-Host "Value: $variableValue"
	
    if ([string]::IsNullOrEmpty($variableValue)) {
        Write-Error "$variable - is a required Environment Variable"
        $validationFailed = $true
    } 
    else {
        Write-Host "Name: $variable | Value: $variableValue"
    }
}

if ($validationFailed) {
    Write-Error "One or more validation checks has failed"
    Write-Error "Validation Failed"
    exit 1
}

Write-Host "Validation Complete"