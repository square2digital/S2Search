# #############################################################
# Script to deploy the S2 Search app to a local K8s Cluster
# - the script is designed to be run as required
# - you can choose to install the full platform or select specific components
# ##############################################################
                                      
# execution commands
# build all services
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-helm-deploy.ps1 -deleteAllImages $true -context "s2search-aks-dev"
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-helm-deploy.ps1 -deleteAllImages $true -context "rancher-desktop"

# combined script
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-helm-deploy.ps1 -deleteAllImages $false -context "s2search-aks-dev"; cd "E:\github\S2Search\Infrastructure"; .\s2search-helm-deploy.ps1 -deleteAllImages $false -context "rancher-desktop";

# local path
# E:\github\S2Search\Infrastructure

# running command
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-helm-deploy.ps1

param (
    [bool]$deleteAllImages = $false,
    [string]$context = "rancher-desktop"
)

# Supported colour values - Black DarkBlue DarkGreen DarkCyan DarkRed DarkMagenta DarkYellow Gray DarkGray Blue Green Cyan Red Magenta Yellow White
function Write-Color([String[]]$Text, [ConsoleColor[]]$Color) {
    for ($i = 0; $i -lt $Text.Length; $i++) {
        Write-Host $Text[$i] -Foreground $Color[$i] -NoNewLine
    }
    Write-Host
}

function Output-Parameters([bool]$param, [String]$name) {
    if ($param) {
        Write-Color -Text "Parameter $name is $param" -Color Green    
    }
    else {
        Write-Color -Text "Parameter $name is $param" -Color Red
    }
}

$S2SearchAsciiArt = @"
        ___      __         __             __   ____      
   ____|__ \    / /_  ___  / /___ ___     / /__( __ )_____
  / ___/_/ /   / __ \/ _ \/ / __ `__ \   / //_/ __  / ___/
 (__  ) __/   / / / /  __/ / / / / / /  / ,< / /_/ (__  ) 
/____/____/  /_/ /_/\___/_/_/ /_/ /_/  /_/|_|\____/____/  
                                                                                        
"@

Write-Color -Text "$S2SearchAsciiArt" -Color DarkBlue

$S2Namespace = "s2search"

# Load .env file if it exists
if (Test-Path ".env") {
    Get-Content ".env" | ForEach-Object {
        if ($_ -match "^\s*([^#][^=]*)\s*=\s*(.*)\s*$") {
            $name = $matches[1].Trim()
            $value = $matches[2].Trim().Trim('"').Trim("'")
            Set-Item -Path "env:$name" -Value $value
        }
    }
    Write-Color -Text "Loaded credentials from .env file" -Color Green
}

# Use environment variables if parameters are not provided
Write-Color -Text "####################################################" -Color DarkBlue
Write-Color -Text "Confirming GitHub Credentials & Secrets from .env"    -Color DarkBlue
Write-Color -Text "####################################################" -Color DarkBlue
if ([string]::IsNullOrEmpty($githubUsername)) { $githubUsername = $env:GITHUB_USERNAME }
if ([string]::IsNullOrEmpty($githubToken)) { $githubToken = $env:GITHUB_TOKEN }
if ([string]::IsNullOrEmpty($databasePassword)) { $databasePassword = $env:DATABASE_PASSWORD }

# Check if GitHub credentials are provided
if ([string]::IsNullOrEmpty($githubUsername) -or [string]::IsNullOrEmpty($githubToken)) {
    Write-Color -Text "Warning: GitHub credentials not provided. You may need to create the ghcr-secret manually." -Color Red
    Write-Color -Text "To create the secret manually, run:" -Color Red
    Write-Color -Text "kubectl create secret docker-registry ghcr-secret --docker-server=ghcr.io --docker-username=$githubUsername --docker-password=$githubToken -n $S2Namespace" -Color Red
    exit
}
else {
    Write-Color -Text "GitHub credentials provided. Will create ghcr-secret." -Color Green
    Write-Color -Text "githubUsername = $githubUsername" -Color Yellow
    Write-Color -Text "githubToken = $githubToken" -Color Yellow
}

if ([string]::IsNullOrEmpty($databasePassword)) {
    Write-Color -Text "Error: The database password not provided. You may need to create the database secret manually." -Color Red
    exit
}
else {
    Write-Color -Text "Database password provided. Will create database secret." -Color Green
    Write-Color -Text "databasePassword = $databasePassword" -Color Yellow
}

cd "E:\github\S2Search\K8s\Helm"

Write-Color -Text "################################" -Color DarkBlue
Write-Color -Text "Helm Deployment"                  -Color DarkBlue
Write-Color -Text "################################" -Color DarkBlue

az aks get-credentials --resource-group s2search-aks-rg --name s2search-aks-dev --overwrite-existing

Write-Color -Text "selected K8s Context $context" -Color DarkYellow
kubectl config use-context $context
kubectl config get-contexts

Write-Color -Text "helm uninstall s2search -n $S2Namespace" -Color DarkYellow
helm uninstall s2search -n $S2Namespace

Write-Color -Text "kubectl delete namespace $S2Namespace" -Color DarkYellow
kubectl delete namespace $S2Namespace

Write-Color -Text "kubectl create namespace $S2Namespace" -Color DarkYellow
kubectl create namespace $S2Namespace

if ($deleteAllImages) {
    
    Write-Color -Text "################################" -Color DarkBlue
    Write-Color -Text "Delete Images"                    -Color DarkBlue
    Write-Color -Text "################################" -Color DarkBlue

    Write-Color -Text "ghcr.io/square2digital/s2search-ui" -Color DarkYellow
    docker rmi ghcr.io/square2digital/s2search-ui:latest -f

    Write-Color -Text "ghcr.io/square2digital/s2search-backend-api" -Color DarkYellow
    docker rmi ghcr.io/square2digital/s2search-backend-api:latest -f
}

# Create GitHub Container Registry secret if credentials are provided
if (-not [string]::IsNullOrEmpty($githubUsername) -and -not [string]::IsNullOrEmpty($githubToken)) {
    Write-Color -Text "Creating GitHub Container Registry secret..." -Color DarkYellow
    
    # Delete existing secret if it exists (ignore errors)
    kubectl delete secret ghcr-secret -n $S2Namespace 2>$null
    
    # Create the secret
    kubectl create secret docker-registry ghcr-secret `
        --docker-server=ghcr.io `
        --docker-username=$githubUsername `
        --docker-password=$githubToken `
        -n $S2Namespace
    
    if ($LASTEXITCODE -eq 0) {
        Write-Color -Text "GitHub Container Registry secret created successfully!" -Color Green
    }
    else {
        Write-Color -Text "Failed to create GitHub Container Registry secret!" -Color Red
        exit 1
    }
}

docker image prune -f

helm dependency update .

cd "E:\github\S2Search\Terraform"

#terraform output -json | ConvertFrom-Json
$tfOutput = terraform output -json | ConvertFrom-Json

$defaultResourceGroup = $tfOutput.default_resource_group_name.value
$storageAccountName = $tfOutput.storage_account_name.value

Write-Color -Text "################################" -Color DarkBlue
Write-Color -Text "Helm Install"                     -Color DarkBlue
Write-Color -Text "################################" -Color DarkBlue

$searchCredentialsQueryKey = az search query-key list --resource-group $defaultResourceGroup --service-name s2-search-dev --output tsv --query "[0].key"
$searchServiceName = (az search service show --resource-group $defaultResourceGroup --name s2-search-dev | ConvertFrom-Json).name
$searchCredentialsInstanceEndpoint = "https://$searchServiceName.search.windows.net"
$storageConnectionString = (az storage account show-connection-string --resource-group $defaultResourceGroup --name $storageAccountName --output tsv)

$databaseConnectionString = "Host=s2search-postgresql;Port=5432;Database=s2searchdb;Username=s2search;Password=$databasePassword";
$redisConnectionString = "s2search-redis-master:6379";

Write-Color -Text "databasePassword - $databasePassword" -Color Blue
Write-Color -Text "databaseConnectionString - $databaseConnectionString" -Color Blue
Write-Color -Text "azureStorageConnectionString - $storageConnectionString" -Color Blue
Write-Color -Text "redisConnectionString - $redisConnectionString" -Color Blue
Write-Color -Text "SearchCredentialsQueryKey: - $searchCredentialsQueryKey" -Color Blue
Write-Color -Text "SearchCredentialsInstanceEndpoint - $searchCredentialsInstanceEndpoint" -Color Blue
Write-Color -Text "AzureStorageAccountName - $storageAccountName" -Color Blue

cd "E:\github\S2Search\K8s\Helm"; 

helm upgrade --install s2search . -n $S2Namespace `
    --set-string postgresql.auth.password=$databasePassword `
    --set-string postgresql.auth.connectionString="$databaseConnectionString" `
    --set-string connectionStrings.databaseConnectionString="$databaseConnectionString" `
    --set-string connectionStrings.azureStorageConnectionString=$storageConnectionString `
    --set-string connectionStrings.redisConnectionString=$redisConnectionString `
    --set-string feedfunctions.azureStorage.connectionString=$storageConnectionString `
    --set-string searchinsights.azureStorage.connectionString=$storageConnectionString `
    --set-string search.searchCredentialsQueryKey=$searchCredentialsQueryKey `
    --set-string search.searchCredentialsInstanceEndpoint=$searchCredentialsInstanceEndpoint `
    --set-string storage.accountName=$storageAccountName;

Write-Color -Text "###########################################" -Color DarkBlue
Write-Color -Text "Deploy SQL Scripts to Postgres - stand by "  -Color DarkBlue
Write-Color -Text "###########################################" -Color DarkBlue

Start-Sleep -Seconds 60

Write-Color -Text "Getting 01-sql_deploy.sql from configmap" -Color Cyan
kubectl get configmap s2search-postgres-init -n $S2Namespace -o jsonpath='{.data.01-sql_deploy\.sql}' > 01-sql_deploy.sql

Write-Color -Text "Getting 02-create_data.sql from configmap" -Color Cyan
kubectl get configmap s2search-postgres-init -n $S2Namespace -o jsonpath='{.data.02-create_data\.sql}' > 02-create_data.sql

Write-Color -Text "Getting 03-search-insights-data.sql from configmap" -Color Cyan
kubectl get configmap s2search-postgres-init -n $S2Namespace -o jsonpath='{.data.03-search-insights-data\.sql}' > 03-search-insights-data.sql

Write-Color -Text "copying SQL script 01-sql_deploy from configmap to pod" -Color Cyan
kubectl cp ./01-sql_deploy.sql s2search/s2search-postgresql-0:/tmp/01-sql_deploy.sql

Write-Color -Text "copying SQL script 02-create_data from configmap to pod" -Color Cyan
kubectl cp ./02-create_data.sql s2search/s2search-postgresql-0:/tmp/02-create_data.sql

Write-Color -Text "copying SQL script 02-create_data from configmap to pod" -Color Cyan
kubectl cp ./03-search-insights-data.sql s2search/s2search-postgresql-0:/tmp/03-search-insights-data.sql

Write-Color -Text "Executing SQL script 01-sql_deploy.sql in pod" -Color Cyan
kubectl exec -it s2search-postgresql-0 -n $S2Namespace -- sh -c "PGPASSWORD='$databasePassword' psql -U s2search -d s2searchdb -f /tmp/01-sql_deploy.sql"

Write-Color -Text "Executing SQL script 02-create_data.sql in pod" -Color Cyan
kubectl exec -it s2search-postgresql-0 -n $S2Namespace -- sh -c "PGPASSWORD='$databasePassword' psql -U s2search -d s2searchdb -f /tmp/02-create_data.sql"

Write-Color -Text "Executing SQL script 03-search-insights-data.sql in pod" -Color Cyan
kubectl exec -it s2search-postgresql-0 -n $S2Namespace -- sh -c "PGPASSWORD='$databasePassword' psql -U s2search -d s2searchdb -f /tmp/03-search-insights-data.sql"

Write-Color -Text "################################" -Color Green
Write-Color -Text "Process Complete"                 -Color Green
Write-Color -Text "################################" -Color Green

docker image prune -f;