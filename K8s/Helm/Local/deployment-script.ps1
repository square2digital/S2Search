# #############################################################
# Script to deploy the S2 Search app to a local K8s Cluster
# - the script is designed to be run as required
# - you can choose to install the full platform or select specific components
# ##############################################################
                                      
# execution commands
# build all services
# .\deployment-script.ps1

# local path
# E:\github\S2Search\K8s\Helm\Local

#######################
# debug the elastic UI
#######################
# build everything
# cls; cd "E:\github\S2Search\K8s\Helm\Local"; .\deployment-script.ps1 -includeSearchUI $true -includePostgres $true -includeSearchAPI $true -includeFunctions $true

param (
    [bool]$includeSearchUI = $false,
    [bool]$includePostgres = $false,    
    [bool]$includeSearchAPI = $false,
    [bool]$includeFunctions = $false
)

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

Output-Parameters -param $includeSearchUI -name "includeSearchUI"
Output-Parameters -param $includePostgres -name "includePostgres"
Output-Parameters -param $includeSearchAPI -name "includeSearchAPI"
Output-Parameters -param $includeFunctions -name "includeFunctions"

$S2SearchAsciiArt = @"
  ██████   ██████ ▓█████ ▄▄▄       ██▀███   ▄████▄   ██░ ██     ██ ▄█▀  ██████ 
▒██    ▒ ▒██    ▒ ▓█   ▀▒████▄    ▓██ ▒ ██▒▒██▀ ▀█  ▓██░ ██▒    ██▄█▒ ▒██    ▒ 
░ ▓██▄   ░ ▓██▄   ▒███  ▒██  ▀█▄  ▓██ ░▄█ ▒▒▓█    ▄ ▒██▀▀██░   ▓███▄░ ░ ▓██▄   
  ▒   ██▒  ▒   ██▒▒▓█  ▄░██▄▄▄▄██ ▒██▀▀█▄  ▒▓▓▄ ▄██▒░▓█ ░██    ▓██ █▄   ▒   ██▒
▒██████▒▒▒██████▒▒░▒████▒▓█   ▓██▒░██▓ ▒██▒▒ ▓███▀ ░░▓█▒░██▓   ▒██▒ █▄▒██████▒▒
▒ ▒▓▒ ▒ ░▒ ▒▓▒ ▒ ░░░ ▒░ ░▒▒   ▓▒█░░ ▒▓ ░▒▓░░ ░▒ ▒  ░ ▒ ░░▒░▒   ▒ ▒▒ ▓▒▒ ▒▓▒ ▒ ░
░ ░▒  ░ ░░ ░▒  ░ ░ ░ ░  ░ ▒   ▒▒ ░  ░▒ ░ ▒░  ░  ▒    ▒ ░▒░ ░   ░ ░▒ ▒░░ ░▒  ░ ░
░  ░  ░  ░  ░  ░     ░    ░   ▒     ░░   ░ ░         ░  ░░ ░   ░ ░░ ░ ░  ░  ░  
      ░        ░     ░  ░     ░  ░   ░     ░ ░       ░  ░  ░   ░  ░         ░  
                                           ░                                   
"@

Write-Color -Text "$S2SearchAsciiArt" -Color Green

############
# Variables
############

# the PatToken is for the "Azure DevOps Artifacts Credentials Provider" which allows the docker images
# when built to pull down dependacies from the DevOps artifacts repo "square2digital"
$PatToken = "4quc53ontolu6jwvy4ktkj2o5z2mhojgpykrzba6mh477wc6zhcq"
$DeploymentRoot = "E:\github\S2Search"

$ApplicationPathSearchUI = "$DeploymentRoot\UIs\SearchUIs\AzureCognitiveServices\S2Search.Search.NextJS.ReactUI\"
Write-Color -Text "The ApplicationPathSearchUI is -> $ApplicationPathSearchUI" -Color Blue

Write-Color -Text "################################" -Color Green
Write-Color -Text "Building Docker Images"           -Color Green
Write-Color -Text "################################" -Color Green

if ($includeSearchUI) {
    Set-Location $ApplicationPathSearchUI
    npm install
    Write-Color -Text "Building Docker Image - S2 Search UI (NextJS) at location $ApplicationPathSearchUI" -Color Magenta    
    Write-Color -Text "docker build --pull --rm -f Dockerfile -t s2searchui:dev . --build-arg NODE_ENV=production" -Color Yellow
    docker build --pull --rm -f "Dockerfile" -t s2searchui:dev . --build-arg NODE_ENV=production
}

Write-Color -Text "################################" -Color Green
Write-Color -Text "Process Complete" -Color Green
Write-Color -Text "################################" -Color Green