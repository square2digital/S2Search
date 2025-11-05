# #############################################################
# Script to deploy infrastructure and code for S2Search
# - the script is designed to be run as required
# ##############################################################

#######################
# Simple deployment
#######################
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -destroyInfra $false -deployInfra $false -uploadAssets $true

# Steps
# 1 - run Terraform to create AKS cluster and supporting infra
# 2 - copy files to new assets folder in blob storage
# 3 - run the APIs to create and deploy the index to Azure Search
# 4 - run Helm deployment script to deploy S2 Search components to AKS

param (
    [bool]$deployInfra = $false,
    [bool]$destroyInfra = $false,
    [bool]$uploadAssets = $false
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

# 1 - run Terraform to create AKS cluster and supporting infra
if ($destroyInfra) {
    Write-Color -Text "running terraform destroy" -Color DarkYellow
    terraform destroy -var-file="values.tfvars" -var-file="secrets.tfvars"    
}

if ($deployInfra) {
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

if ($uploadAssets) {

    Write-Color -Text "getting terraform outputs" -Color DarkYellow
    $tfOutput = terraform output -json | ConvertFrom-Json

    # output all vars    
    <#     foreach ($key in $tfOutput.PSObject.Properties.Name) {
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

    # Upload files recursively
    Get-ChildItem -Path $localFolderPath -Recurse -File | ForEach-Object {
        $filePath = $_.FullName
        $relativePath = $_.FullName.Substring($localFolderPath.Length + 1) -replace '\\', '/'

        az storage blob upload `
            --account-name $storageAccount `
            --account-key $accountKey `
            --container-name $containerName `
            --name $relativePath `
            --file $filePath `
            --overwrite
    }
}


# 2 - copy files to new assets folder in blob storage
