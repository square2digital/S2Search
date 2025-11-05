# #############################################################
# Script to deploy infrastructure and code for S2Search
# - the script is designed to be run as required
# ##############################################################

#######################
# Simple deployment
#######################
# cls; cd "E:\github\S2Search\Infrastructure"; .\s2search-infra-deploy.ps1 -deployInfra $true

# Steps
# 1 - run Terraform to create AKS cluster and supporting infra
# 2 - copy files to new assets folder in blob storage
# 3 - run the APIs to create and deploy the index to Azure Search
# 4 - run Helm deployment script to deploy S2 Search components to AKS

param (
    [bool]$deployInfra = $false
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

if ($deployInfra) {

    Write-Color -Text "Changing to Terraform directory" -Color DarkYellow
    cd "E:\github\S2Search\Terraform"

    Write-Color -Text "run terraform validate" -Color DarkYellow
    terraform validate
}
