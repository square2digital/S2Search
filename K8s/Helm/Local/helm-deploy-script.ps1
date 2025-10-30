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
# build everything (credentials loaded from .env file)
# cls; cd "E:\github\S2Search\K8s\Helm\Local"; .\helm-deploy-script.ps1 -includeSearchUI $true -includePostgres $true -includeSearchAPI $true -includeFunctions $true
# 
# Or with explicit credentials (not recommended for public repos):
# cls; cd "E:\github\S2Search\K8s\Helm\Local"; .\helm-deploy-script.ps1 -includeSearchUI $true -includePostgres $true -includeSearchAPI $true -includeFunctions $true -githubUsername "your-username" -githubToken "your-token"

param (
    [bool]$includeSearchUI = $false,
    [bool]$includePostgres = $false,    
    [bool]$includeSearchAPI = $false,
    [bool]$includeFunctions = $false,
    [string]$githubUsername = "",
    [string]$githubToken = ""
)

# Supported colour values - Black DarkBlue DarkGreen DarkCyan DarkRed DarkMagenta DarkYellow Gray DarkGray Blue Green Cyan Red Magenta Yellow White
function Write-Color([String[]]$Text, [ConsoleColor[]]$Color) {
    for ($i = 0; $i -lt $Text.Length; $i++) {
        Write-Host $Text[$i] -Foreground $Color[$i] -NoNewLine
    }
    Write-Host
}

function Load-EnvFile([string]$envFilePath) {
    if (Test-Path $envFilePath) {
        Write-Color -Text "Loading environment variables from $envFilePath" -Color Green
        Get-Content $envFilePath | ForEach-Object {
            if ($_ -match "^\s*([^#][^=]*)\s*=\s*(.*)\s*$") {
                $name = $matches[1].Trim()
                $value = $matches[2].Trim()
                
                # Remove quotes if present
                if ($value -match '^"(.*)"$' -or $value -match "^'(.*)'$") {
                    $value = $matches[1]
                }
                
                Set-Item -Path "env:$name" -Value $value
                Write-Color -Text "Loaded: $name" -Color DarkGray
            }
        }
    }
    else {
        Write-Color -Text "Environment file not found: $envFilePath" -Color Yellow
        Write-Color -Text "You can create one from .env.example or provide credentials via parameters" -Color Yellow
    }
}

function Output-Parameters([bool]$param, [String]$name) {
    if ($param) {
        Write-Color -Text "Parameter $name is $param" -Color Green    
    }
    else {
        Write-Color -Text "Parameter $name is $param" -Color Red
    }
}

# Load environment variables from .env file
Load-EnvFile -envFilePath ".env"

# Use environment variables if parameters are not provided
if ([string]::IsNullOrEmpty($githubUsername) -and $env:GITHUB_USERNAME) {
    $githubUsername = $env:GITHUB_USERNAME
    Write-Color -Text "Using GitHub username from environment variable" -Color Green
}

if ([string]::IsNullOrEmpty($githubToken) -and $env:GITHUB_TOKEN) {
    $githubToken = $env:GITHUB_TOKEN
    Write-Color -Text "Using GitHub token from environment variable" -Color Green
}

Output-Parameters -param $includeSearchUI -name "includeSearchUI"
Output-Parameters -param $includePostgres -name "includePostgres"
Output-Parameters -param $includeSearchAPI -name "includeSearchAPI"
Output-Parameters -param $includeFunctions -name "includeFunctions"

# Check if GitHub credentials are provided
if ([string]::IsNullOrEmpty($githubUsername) -or [string]::IsNullOrEmpty($githubToken)) {
    Write-Color -Text "Warning: GitHub credentials not provided. You may need to create the ghcr-secret manually." -Color Yellow
    Write-Color -Text "To create the secret manually, run:" -Color Yellow
    Write-Color -Text "kubectl create secret docker-registry ghcr-secret --docker-server=ghcr.io --docker-username=<your-github-username> --docker-password=<your-github-token> -n s2search" -Color Yellow
}
else {
    Write-Color -Text "GitHub credentials provided. Will create ghcr-secret." -Color Green
}

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

Write-Color -Text "$S2SearchAsciiArt" -Color DarkBlue

############
# Variables
############

# the PatToken is for the "Azure DevOps Artifacts Credentials Provider" which allows the docker images
# when built to pull down dependacies from the DevOps artifacts repo "square2digital"
#$PatToken = "4quc53ontolu6jwvy4ktkj2o5z2mhojgpykrzba6mh477wc6zhcq"
#$DeploymentRoot = "E:\github\S2Search"

# Use environment variable for namespace if available, otherwise use default
$S2Namespace = if ($env:S2_NAMESPACE) { $env:S2_NAMESPACE } else { "s2search" }

Write-Color -Text "################################" -Color DarkBlue
Write-Color -Text "Helm Deployment"                  -Color DarkBlue
Write-Color -Text "################################" -Color DarkBlue

kubectl delete namespace $S2Namespace -IgnoreNotFound

# Add namespace (optional but recommended)
kubectl create namespace $S2Namespace

# Create GitHub Container Registry secret if credentials are provided
if (-not [string]::IsNullOrEmpty($githubUsername) -and -not [string]::IsNullOrEmpty($githubToken)) {
    Write-Color -Text "Creating GitHub Container Registry secret..." -Color Yellow
    
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

# Install chart with default values
helm install s2search . -n $S2Namespace

Write-Color -Text "################################" -Color Green
Write-Color -Text "Process Complete"                 -Color Green
Write-Color -Text "################################" -Color Green