# #############################################################
# Script to deploy infrastructure and code for S2Search
# - the script is designed to be run as required
# ##############################################################

#######################
# Simple deployment
#######################
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -deployInfra $true -destroyInfra $true -uploadAssets $true -provisionSearch $true

# Steps
# 1 - run Terraform to create AKS cluster and supporting infra
# 2 - copy files to new assets folder in blob storage
# 3 - run the APIs to create and deploy the index to Azure Search
# 4 - Deploy indexes and data to Azure Search

param (
    [bool]$deployInfra = $false,
    [bool]$destroyInfra = $false,
    [bool]$uploadAssets = $false,
    [bool]$provisionSearch = $false  # New parameter for background upload
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
    $resourceGroup = $tfOutput.resource_group_name.value
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
    $resourceGroup = $tfOutput.resource_group_name.value

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

    $jsonContent = $jsonContent -replace 'https://s2storagedev.blob.core.windows.net', $tfOutput.search_service_url.value
    #Write-Color -Text $jsonContent -Color DarkMagenta

    $uri = "$($tfOutput.search_service_url.value)/indexes/$indexName/docs/index?api-version=2025-08-01-preview"

    # Send request
    $response = Invoke-RestMethod -Uri $uri -Method Post -Headers $headers -Body $jsonContent
    
    # Output the response
    Write-Color -Text $response -Color Green
}

Write-Color -Text "###################################" -Color DarkBlue
Write-Color -Text "Terraform output" -Color DarkBlue
Write-Color -Text "###################################" -Color DarkBlue

Write-Color -Text "running terraform output" -Color DarkYellow
terraform output