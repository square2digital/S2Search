# #############################################################
# Script to deploy infrastructure and codebase for S2Search
# - the script is designed to be run as required
# ##############################################################

#######################
# Simple deployment
#######################
# full script execution
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $true -deployInfra $true -uploadAssets $true -provisionSearch $true -HelmDeployment $true

# infrastructure only - no helm deployment
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $true -deployInfra $true -uploadAssets $true -provisionSearch $true -HelmDeployment $false

# Helm Deployment only - no infrastructure changes
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $false -deployInfra $false -uploadAssets $false -provisionSearch $false -HelmDeployment $true

# Destroy only
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $true -deployInfra $false -uploadAssets $false -provisionSearch $false -HelmDeployment $false

# segmented execution - test search
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $false -deployInfra $false -uploadAssets $false -provisionSearch $true -HelmDeployment $false

# Steps
# 1 - run Terraform to create AKS cluster and supporting infra
# 2 - copy files to new assets folder in blob storage
# 3 - run the APIs to create and deploy the index to Azure Search
# 4 - Deploy indexes and data to Azure Search

param (
    [bool]$destroyInfra = $false,    
    [bool]$deployInfra = $false,    
    [bool]$uploadAssets = $false,
    [bool]$provisionSearch = $false,
    [bool]$HelmDeployment = $false,
    [string]$databasePassword = "",    
    [string]$databaseConnectionString = "",
    [string]$redisConnectionString = ""
)

function Write-Color([String[]]$Text, [ConsoleColor[]]$Color) {
    for ($i = 0; $i -lt $Text.Length; $i++) {
        Write-Host $Text[$i] -Foreground $Color[$i] -NoNewLine
    }
    Write-Host
}

$S2SearchAsciiArt = @"
        ___      _       ____          
   ____|__ \    (_)___  / __/________ _
  / ___/_/ /   / / __ \/ /_/ ___/ __ `/
 (__  ) __/   / / / / / __/ /  / /_/ / 
/____/____/  /_/_/ /_/_/ /_/   \__,_/  

"@

Write-Color -Text "$S2SearchAsciiArt" -Color DarkBlue

$tfOutput = ""

Write-Color -Text "Changing to Terraform directory" -Color DarkYellow
Set-Location "E:\github\S2Search\Terraform"

# Pause for user confirmation
Write-Color -Text "" -Color White
Write-Color -Text "Press any key to continue with deployment, or ESC or n to exit..." -Color Yellow
$key = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

if ($key.VirtualKeyCode -eq 27 -or $key.Character -eq 'n') {
    Write-Color -Text "" -Color White
    Write-Color -Text "Deployment cancelled by user." -Color Red
    Write-Color -Text "Exiting script..." -Color Yellow
    exit 0
}

Write-Color -Text "Continuing with deployment..." -Color Green
Write-Color -Text "" -Color White

# 1 - run Terraform to provision infrastructure

if ($destroyInfra) {
    
    Write-Color -Text "###################################" -Color DarkBlue
    Write-Color -Text "Terraform destroy infrastructure" -Color DarkBlue
    Write-Color -Text "###################################" -Color DarkBlue

    Write-Color -Text "running terraform destroy" -Color DarkYellow
    terraform destroy -var-file="values.tfvars" -var-file="secrets.tfvars" -auto-approve
}

if ($deployInfra) {

    Write-Color -Text "###################################" -Color DarkBlue
    Write-Color -Text "Terraform create infrastructure" -Color DarkBlue
    Write-Color -Text "###################################" -Color DarkBlue

    Write-Color -Text "running terraform validate" -Color DarkYellow
    terraform validate

    Write-Color -Text "running terraform plan" -Color DarkYellow
    terraform plan -var-file="values.tfvars" -var-file="secrets.tfvars"

    Write-Color -Text "running terraform apply" -Color DarkYellow
    terraform apply -var-file="values.tfvars" -var-file="secrets.tfvars" -auto-approve

    $tfOutput = terraform output -json | ConvertFrom-Json

    $storageAccountName = $tfOutput.storage_account_name.value

    # Use these values in further automation
    Write-Host "Storage Account: $storageAccountName"
}

# copy files to new assets folder in blob storage
if ($uploadAssets) {

    Write-Color -Text "##################################################" -Color DarkBlue
    Write-Color -Text "copy files to new assets folder in blob storage" -Color DarkBlue
    Write-Color -Text "##################################################" -Color DarkBlue

    Write-Color -Text "getting terraform outputs" -Color DarkYellow
    $tfOutput = terraform output -json | ConvertFrom-Json    

    # output all vars    
    <#
        foreach ($key in $tfOutput.PSObject.Properties.Name) {
        $value = $tfOutput.$key.value
        Write-Host "$key = $value"
    }
    #>

    # Set variables
    $resourceGroup = $tfOutput.default_resource_group_name.value
    $storageAccount = $tfOutput.storage_account_name.value
    $containerName = "assets"
    $localFolderPath = "C:\Users\square2\OneDrive - Square2 Digital\Square2 Digital\Development\Storage\assets"

    # Get the storage account key
    $accountKey = az storage account keys list `
        --resource-group $resourceGroup `
        --account-name $storageAccount `
        --query "[0].value" `
        --output tsv

    # Upload files using az storage blob sync (much faster than individual uploads)
    Write-Color -Text "Syncing files to storage container '$containerName' using az storage blob sync..." -Color DarkYellow
    
    $syncStartTime = Get-Date
    
    az storage blob sync `
        --account-name $storageAccount `
        --account-key $accountKey `
        --container $containerName `
        --source $localFolderPath `
        --delete-destination false `
        --exclude-pattern "*.tmp;*.log;Thumbs.db" `
        --output table
    
    $syncEndTime = Get-Date
    $syncDuration = ($syncEndTime - $syncStartTime).TotalSeconds

    Write-Color -Text "Asset Upload complete - time $([math]::Round($syncDuration, 2)) seconds" -Color Green
}

# Deploy indexes and data to Azure Search
if ($provisionSearch) {

    Write-Color -Text "##################################################" -Color DarkBlue
    Write-Color -Text "Deploy indexes and data to Azure Search" -Color DarkBlue
    Write-Color -Text "##################################################" -Color DarkBlue    

    Write-Color -Text "getting terraform outputs" -Color DarkYellow
    $tfOutput = terraform output -json | ConvertFrom-Json    

    $searchServiceName = $tfOutput.search_service_name.value
    $resourceGroup = $tfOutput.default_resource_group_name.value

    # Get the admin API key
    $apiKey = az search admin-key show `
        --service-name $searchServiceName `
        --resource-group $resourceGroup `
        --query "primaryKey" `
        --output tsv
        
    # Define the API endpoint and headers
    $uri = "$($tfOutput.search_service_url.value)/synonymmaps?api-version=2025-08-01-preview"
    $headers = @{
        "api-key"      = $apiKey
        "Content-Type" = "application/json"
    }

    ######################
    ## Create Synonym Map
    ######################
    Write-Color -Text "Create Synonym Map" -Color DarkYellow

    $body = @{
        name          = "vehicles"
        format        = "solr"
        synonyms      = "beema, bimma => BMW`nVW => Volkswagen`ngas => Petrol`nChevy => Chevrolet`nsedan => Saloon"
        encryptionKey = $null
    } | ConvertTo-Json -Depth 3

    # Send the POST request
    $response = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $body
    
    # Output the response
    Write-Color -Text $response -Color Green

    ######################
    ## Create Index
    ######################
    Write-Color -Text "Create Index" -Color DarkYellow
    $indexName = "s2-demo-vehicles"
    
    $jsonContent = Get-Content -Raw -Path "E:/github/S2Search/Infrastructure/assets/index-schema.json"
    $jsonContent = $jsonContent -replace '##index_name##', $indexName
    $body = $jsonContent
    #Write-Color -Text $body -Color DarkMagenta
    
    $uri = "$($tfOutput.search_service_url.value)/indexes?api-version=2025-08-01-preview"

    # Send request
    $response = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $body
    
    # Output the response
    Write-Color -Text $response -Color Green

    ######################
    ## Import Data
    ######################        
    Write-Color -Text "Import Data" -Color DarkYellow
    $jsonContent = Get-Content -Raw -Path "E:/github/S2Search/Infrastructure/assets/vehicle-data.json"

    $jsonContent = $jsonContent -replace 'https://s2storagedev.blob.core.windows.net/assets', $tfOutput.storage_container_assets_url.value
    #Write-Color -Text $jsonContent -Color DarkMagenta

    $uri = "$($tfOutput.search_service_url.value)/indexes/$indexName/docs/index?api-version=2025-08-01-preview"

    # Send request
    $response = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $jsonContent
    
    # Output the response
    Write-Color -Text $response -Color Green
}

if ($HelmDeployment) {
    
    Write-Color -Text "###################" -Color DarkBlue
    Write-Color -Text "Helm Deployment"     -Color DarkBlue
    Write-Color -Text "###################" -Color DarkBlue

    $tfOutput = terraform output -json | ConvertFrom-Json

    $aksClusterName = $tfOutput.aks_cluster_name.value

    # resource groups
    $defaultResourceGroup = $tfOutput.default_resource_group_name.value
    $k8sResourceGroup = $tfOutput.k8s_resource_group_name.value

    az aks get-credentials `
        --name $aksClusterName `
        --resource-group $k8sResourceGroup `
        --overwrite-existing    

    # Use environment variable for namespace if available, otherwise use default
    $S2Namespace = "s2search"

    Set-Location "E:\github\S2Search\K8s\Helm\Local"

    Write-Color -Text "helm uninstall s2search . -n $S2Namespace" -Color DarkYellow
    helm uninstall s2search . -n $S2Namespace

    Write-Color -Text "kubectl delete namespace $S2Namespace" -Color DarkYellow
    kubectl delete namespace $S2Namespace

    Write-Color -Text "kubectl create namespace $S2Namespace" -Color DarkYellow
    kubectl create namespace $S2Namespace

    # Get GitHub credentials from environment variables
    # Load environment variables from .env file
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

    $githubUsername = $env:GITHUB_USERNAME
    $githubToken = $env:GITHUB_TOKEN

    Write-Color -Text "githubUsername = $githubUsername" -Color Magenta
    Write-Color -Text "githubToken = $githubToken" -Color Magenta

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

    helm dependency update .

    ###########################
    ## Get the Search details
    ###########################
    $SearchCredentialsQueryKey = az search query-key list --resource-group $defaultResourceGroup --service-name s2-search-dev --output tsv --query "[0].key"
    $searchServiceName = (az search service show --resource-group $defaultResourceGroup --name s2-search-dev | ConvertFrom-Json).name
    $SearchCredentialsInstanceEndpoint = $tfOutput.search_service_connection_info.endpoint_url.value
    $azureStorageAccountName = $tfOutput.storage_account_name.value
    $redisConnectionString = "s2search-redis-master:6379";

    $databasePassword = "";
    $databaseConnectionString = "Host=s2search-postgresql;Port=5432;Database=s2searchdb;Username=s2search;Password=$databasePassword";


    ###########################
    ## Get the Storage details
    ###########################
    $storageConnectionString = (az storage account show-connection-string --resource-group $defaultResourceGroup --name $storageAccountName --output tsv)
    Write-Host "Storage Connection String: $storageConnectionString"

    Write-Color -Text "databasePassword - $databasePassword" -Color Blue
    Write-Color -Text "databaseConnectionString - $databaseConnectionString" -Color Blue
    Write-Color -Text "azureStorageConnectionString - $StorageConnectionString" -Color Blue
    Write-Color -Text "redisConnectionString - $redisConnectionString" -Color Blue
    Write-Color -Text "SearchCredentialsQueryKey: - $SearchCredentialsQueryKey" -Color Blue
    Write-Color -Text "SearchCredentialsInstanceEndpoint - $SearchCredentialsInstanceEndpoint" -Color Blue
    Write-Color -Text "AzureStorageAccountName - $azureStorageAccountName" -Color Blue

    helm upgrade --install s2search . -n $S2Namespace `
        --set-string postgresql.auth.password=$databasePassword `
        --set-string postgresql.auth.connectionString="$databaseConnectionString" `
        --set-string ConnectionStrings.databaseConnectionString="$databaseConnectionString" `
        --set-string ConnectionStrings.azureStorageConnectionString=$StorageConnectionString `
        --set-string ConnectionStrings.redisConnectionString=$redisConnectionString `
        --set-string Search.SearchCredentialsQueryKey=$SearchCredentialsQueryKey `
        --set-string Search.SearchCredentialsInstanceEndpoint=$SearchCredentialsInstanceEndpoint `
        --set-string feedfunctions.azureStorage.connectionString=$StorageConnectionString `
        --set-string searchinsights.azureStorage.connectionString=$StorageConnectionString;
}

Write-Color -Text "###################################" -Color DarkBlue
Write-Color -Text "Terraform output" -Color DarkBlue
Write-Color -Text "###################################" -Color DarkBlue

Write-Color -Text "running terraform output" -Color DarkYellow
terraform output

Write-Color -Text "################################" -Color Green
Write-Color -Text "Process Complete"                 -Color Green
Write-Color -Text "################################" -Color Green