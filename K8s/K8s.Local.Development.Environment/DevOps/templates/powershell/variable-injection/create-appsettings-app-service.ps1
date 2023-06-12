# Create $appServiceSettings variable to deploy to an App Service

$folder = Split-Path $PSCommandPath -Parent

#Render the service configuration file with the appsettings content
$appServiceSettings = (Get-Content -Path "$folder/appsettings.AppService.json" -Raw) 

$appServiceSettings = $ExecutionContext.InvokeCommand.ExpandString($appServiceSettings)

$appServiceSettings = $appServiceSettings.Replace("`r", " ")
$appServiceSettings = $appServiceSettings.Replace("`n", " ")

Write-Host "Updating AppServiceSettings environment variable"
Write-Host "##vso[task.setvariable variable=AppServiceSettings]$appServiceSettings"