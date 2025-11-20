# #############################################################
# Script to deploy infrastructure and codebase for S2Search
# - the script is designed to be run as required
# ##############################################################

#######################
# Simple deployment
#######################
# full script execution
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $true -deployInfra $true -uploadAssets $true -provisionSearch $true -helmDeployment $true

# infrastructure only - no helm deployment
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $true -deployInfra $true -uploadAssets $true -provisionSearch $true -helmDeployment $false

# Helm Deployment only - no infrastructure changes
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $false -deployInfra $false -uploadAssets $false -provisionSearch $false -helmDeployment $true

# Destroy only
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $true -deployInfra $false -uploadAssets $false -provisionSearch $false -helmDeployment $false

# segmented execution - test search
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $false -deployInfra $false -uploadAssets $false -provisionSearch $true -helmDeployment $false

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
    [bool]$helmDeployment = $false
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
if ($destroyInfra -or $deployInfra) {
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
}

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

if ($helmDeployment) {

    #$context = $tfOutput.aks_kube_config_context.value
    $context = "s2search-aks-dev";
    cd "E:\github\S2Search\Infrastructure"
    ./s2search-helm-deploy.ps1 -deleteAllImages $true -context $context

}

Write-Color -Text "###################################" -Color DarkBlue
Write-Color -Text "Terraform output" -Color DarkBlue
Write-Color -Text "###################################" -Color DarkBlue

Write-Color -Text "running terraform output" -Color DarkYellow
terraform output

Write-Color -Text "################################" -Color Green
Write-Color -Text "Process Complete"                 -Color Green
Write-Color -Text "################################" -Color Green