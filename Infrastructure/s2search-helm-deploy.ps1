# #############################################################
# Script to deploy the S2 Search app to a local K8s Cluster
# - the script is designed to be run as required
# - you can choose to install the full platform or select specific components
# ##############################################################
                                      
# execution commands
# build all services
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-helm-deploy.ps1 -deleteAllImages $true -context "s2search-aks-dev"
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-helm-deploy.ps1 -deleteAllImages $true -context "rancher-desktop"

# local path
# E:\github\S2Search\Infrastructure

# running command
# - see one note - K8s & Helm for commands and examples

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
if ([string]::IsNullOrEmpty($githubUsername)) { $githubUsername = $env:GITHUB_USERNAME }
if ([string]::IsNullOrEmpty($githubToken)) { $githubToken = $env:GITHUB_TOKEN }
if ([string]::IsNullOrEmpty($databasePassword)) { $databasePassword = $env:DATABASE_PASSWORD }

# Check if GitHub credentials are provided
if ([string]::IsNullOrEmpty($githubUsername) -or [string]::IsNullOrEmpty($githubToken)) {
    Write-Color -Text "Warning: GitHub credentials not provided. You may need to create the ghcr-secret manually." -Color Red
    Write-Color -Text "To create the secret manually, run:" -Color Red
    Write-Color -Text "kubectl create secret docker-registry ghcr-secret --docker-server=ghcr.io --docker-username=<your-github-username> --docker-password=<your-github-token> -n s2search" -Color Red
    exit
}
else {
    Write-Color -Text "GitHub credentials provided. Will create ghcr-secret." -Color Green
}

if ([string]::IsNullOrEmpty($databasePassword)) {
    Write-Color -Text "Error: The database password not provided. You may need to create the database secret manually." -Color Red
    exit
}
else {
    Write-Color -Text "Database password provided. Will create database secret." -Color Green
}

$S2SearchAsciiArt = @"
        ___      __         __             __   ____      
   ____|__ \    / /_  ___  / /___ ___     / /__( __ )_____
  / ___/_/ /   / __ \/ _ \/ / __ `__ \   / //_/ __  / ___/
 (__  ) __/   / / / /  __/ / / / / / /  / ,< / /_/ (__  ) 
/____/____/  /_/ /_/\___/_/_/ /_/ /_/  /_/|_|\____/____/  
                                                                                        
"@

Write-Color -Text "$S2SearchAsciiArt" -Color DarkBlue

cd "E:\github\S2Search\K8s\Helm"

############
# Variables
############

# the PatToken is for the "Azure DevOps Artifacts Credentials Provider" which allows the docker images
# when built to pull down dependacies from the DevOps artifacts repo "square2digital"
#$PatToken = "4quc53ontolu6jwvy4ktkj2o5z2mhojgpykrzba6mh477wc6zhcq"
#$DeploymentRoot = "E:\github\S2Search"

# Use environment variable for namespace if available, otherwise use default
$S2Namespace = "s2search"

Write-Color -Text "################################" -Color DarkBlue
Write-Color -Text "Helm Deployment"                  -Color DarkBlue
Write-Color -Text "################################" -Color DarkBlue

Write-Color -Text "selected K8s Context $context" -Color DarkYellow
kubectl config use-context $context
kubectl config get-contexts

Write-Color -Text "helm uninstall s2search . -n $S2Namespace" -Color DarkYellow
helm uninstall s2search . -n $S2Namespace

Write-Color -Text "kubectl delete namespace $S2Namespace" -Color DarkYellow
kubectl delete namespace $S2Namespace

Write-Color -Text "kubectl create namespace $S2Namespace" -Color DarkYellow
kubectl create namespace $S2Namespace

if ($deleteAllImages) {
    
    Write-Color -Text "################################" -Color DarkBlue
    Write-Color -Text "Delete Images"                    -Color DarkBlue
    Write-Color -Text "################################" -Color DarkBlue

    # delete S2 Namespace
    #Write-Color -Text "Delete all resources in the $S2Namespace Namespace" -Color Yellow
    #kubectl delete all --all -n $S2Namespace

    Write-Color -Text "ghcr.io/square2digital/s2search-ui" -Color DarkYellow
    docker rmi ghcr.io/square2digital/s2search-ui:latest

    Write-Color -Text "ghcr.io/square2digital/s2search-backend-api" -Color DarkYellow
    docker rmi ghcr.io/square2digital/s2search-backend-api:latest
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
        Write-Color -Text "GitHub Container Registry secret created successfully!" -Color Blue
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

###########################
## Get the Search details
###########################
$searchCredentialsQueryKey = az search query-key list --resource-group s2search-rg --service-name s2-search-dev --output tsv --query "[0].key"
$searchServiceName = (az search service show --resource-group s2search-rg --name s2-search-dev | ConvertFrom-Json).name
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

helm upgrade --install s2search . -n s2search `
    --set-string postgresql.auth.password=$databasePassword `
    --set-string postgresql.auth.connectionString="$databaseConnectionString" `
    --set-string ConnectionStrings.databaseConnectionString="$databaseConnectionString" `
    --set-string ConnectionStrings.azureStorageConnectionString=$storageConnectionString `
    --set-string ConnectionStrings.redisConnectionString=$redisConnectionString `
    --set-string feedfunctions.azureStorage.connectionString=$storageConnectionString `
    --set-string searchinsights.azureStorage.connectionString=$storageConnectionString `
    --set-string Search.searchCredentialsQueryKey=$searchCredentialsQueryKey `
    --set-string Search.searchCredentialsInstanceEndpoint=$searchCredentialsInstanceEndpoint `
    --set-string Storage.accountName=$storageAccountName;

docker image prune -f;

Write-Color -Text "################################" -Color Green
Write-Color -Text "Process Complete"                 -Color Green
Write-Color -Text "################################" -Color Green