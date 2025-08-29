# Create $appsettings variable to deploy as appsettings.Kubernetes.json

$folder = Split-Path $PSCommandPath -Parent

#Render the service configuration file with the appsettings content
$appSettings = (Get-Content -Path "$folder/appsettings.Kubernetes.json" -Raw) 

$appSettings = $ExecutionContext.InvokeCommand.ExpandString($appSettings)

$appSettings = $appSettings.Replace("`r", " ")
$appSettings = $appSettings.Replace("`n", " ")

Write-Host "Updating AppSettings environment variable"
Write-Host "##vso[task.setvariable variable=AppSettings]$appSettings"