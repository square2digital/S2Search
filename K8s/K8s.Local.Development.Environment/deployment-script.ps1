# #############################################################
# Script to deploy the S2 Search app to a local K8s Cluster
# - the script is designed to be run as required
# - you can choose to install the full platform or select specific components
# -  
# #############################################################

# execution commands
# build all services
# .\deployment-script.ps1

#############################################################
# - replace with your path to use the example commands below 
#############################################################

# build nothing
# cls; cd "F:\github\Square2 Digital\S2Search\K8s\K8s.Local.Development.Environment"; .\deployment-script.ps1 -includeElasticUI $false -includeSearchUI $false -includeAdminUI $false -includeConfigAPI $false -includeSearchAPI $false -includeElasticAPI $false -includeCRAPI $false -includeRedis $false -includeSftpGo $false -includeElastic $false -deleteAllImages $false -includeAdminAPI $false -deleteS2Namespace $false

# build everything - exclude includeConfigAPI & includeCRAPI
# cls; cd "F:\github\Square2 Digital\S2Search\K8s\K8s.Local.Development.Environment"; .\deployment-script.ps1 -includeElasticUI $true -includeSearchUI $true -includeAdminUI $true -includeConfigAPI $false -includeSearchAPI $true -includeElasticAPI $true -includeCRAPI $false -includeRedis $true -includeSftpGo $true -includeElastic $true -deleteAllImages $false -includeAdminAPI $true -deleteS2Namespace $true

# build specifc service only - Elastic UI & API in this case
# cls; cd "F:\github\Square2 Digital\S2Search\K8s\K8s.Local.Development.Environment"; .\deployment-script.ps1 -includeElasticUI $true -includeSearchUI $true -includeAdminUI $false -includeConfigAPI $false -includeSearchAPI $false -includeElasticAPI $false -includeCRAPI $false -includeRedis $false -includeSftpGo $false -includeElastic $false -deleteAllImages $false -includeAdminAPI $true -deleteS2Namespace $false

#######################
# debug the elastic UI
#######################
# build everything
# cls; cd "F:\github\Square2 Digital\S2Search\K8s\K8s.Local.Development.Environment"; .\deployment-script.ps1 -includeElasticUI $true -includeSearchUI $true -includeAdminUI $true -includeConfigAPI $true -includeSearchAPI $true -includeElasticAPI $true -includeCRAPI $true -includeRedis $true -includeSftpGo $true -includeElastic $true -deleteAllImages $true -includeAdminAPI $true -deleteS2Namespace $true

##############################
## remove currently redundant services
## ConfigAPI
## CRAPI
## SftpGo
##############################
# cls; cd "F:\github\Square2 Digital\S2Search\K8s\K8s.Local.Development.Environment"; .\deployment-script.ps1 -includeElasticUI $true -includeSearchUI $true -includeAdminUI $true -includeConfigAPI $false -includeSearchAPI $true -includeElasticAPI $true -includeCRAPI $false -includeRedis $true -includeSftpGo $false -includeElastic $true -deleteAllImages $true -includeAdminAPI $true -deleteS2Namespace $true

param (
    [bool]$includeElasticUI = $false,
    [bool]$includeSearchUI = $false,
    [bool]$includeAdminUI = $false,
    
    [bool]$includeSearchAPI = $false,
    [bool]$includeElasticAPI = $true,
    [bool]$includeAdminAPI = $false,

    [bool]$includeConfigAPI = $false,
    [bool]$includeCRAPI = $false,

    [bool]$includeRedis = $false,
    [bool]$includeSftpGo = $false,
    [bool]$includeElastic = $false,    

    [bool]$deleteAllImages = $false,
    [bool]$deleteS2Namespace = $false
)

# Suppoted colour values - Black DarkBlue DarkGreen DarkCyan DarkRed DarkMagenta DarkYellow Gray DarkGray Blue Green Cyan Red Magenta Yellow White
function Write-Color([String[]]$Text, [ConsoleColor[]]$Color) {
    for ($i = 0; $i -lt $Text.Length; $i++) {
        Write-Host $Text[$i] -Foreground $Color[$i] -NoNewLine
    }
    Write-Host
}

function Test-Application {
    param([string]$ApplicationName, [string]$Endpoint, [int]$TimeoutMs )

    $DelayMs = 2500
    do {
        Write-Progress -Activity "tesing endpoint in Search UI" -SecondsRemaining ($TimeoutMs / 1000)
        Start-Sleep -Milliseconds $DelayMs
        $TimeoutMs -= $DelayMs
        $Response = try {
            (Invoke-WebRequest -Uri $Endpoint)
            if ($Response.StatusCode -eq 200) {
                Write-Color -Text "$ApplicationName on URL $Endpoint responsed with a 200" -Color Green
            }

        }
        catch [System.Net.WebException] {
            $_.Exception.Response
        }
    }
    until ($Response.StatusCode -eq 200 -or $TimeoutMs -lt 0)

    if ($TimeoutMs -lt 0) {
        Write-Error -Message "Testing $ApplicationName with endpoint - $Endpoint timed out" -ErrorAction Stop
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

## output the parameters for visual update
Output-Parameters -param $includeElasticUI -name "includeElasticUI"
Output-Parameters -param $includeSearchUI -name "includeSearchUI"
Output-Parameters -param $includeAdminUI -name "includeAdminUI"
Output-Parameters -param $includeConfigAPI -name "includeConfigAPI"
Output-Parameters -param $includeSearchAPI -name "includeSearchAPI"
Output-Parameters -param $includeElasticAPI -name "includeElasticAPI"
Output-Parameters -param $includeCRAPI -name "includeCRAPI"
Output-Parameters -param $includeRedis -name "includeRedis"
Output-Parameters -param $includeSftpGo -name "includeSftpGo"
Output-Parameters -param $includeElastic -name "includeElastic"
Output-Parameters -param $includeAdminAPI -name "includeAdminAPI"
Output-Parameters -param $deleteAllImages -name "deleteAllImages"

# #############################################################
# Ensure that the current context is set to docker-desktop
# if not throw an exception and immediately exit script
# #############################################################

$context = Invoke-Expression -Command "kubectl config current-context"
$localContext = "docker-desktop"

if ($context -ne $localContext) {
    Invoke-Expression -Command "kubectl config use-context docker-desktop"    
    throw "The K8s context is not $localContext but is actually $context - the context must be set as $localContext - the context has now been updated to $localContext"
}

############
# Variables
############

# the PatToken is for the "Azure DevOps Artifacts Credentials Provider" which allows the docker images
# when built to pull down dependacies from the DevOps artifacts repo "square2digital"
$PatToken = "4quc53ontolu6jwvy4ktkj2o5z2mhojgpykrzba6mh477wc6zhcq"
$DeploymentRoot = "F:\github\Square2 Digital\S2Search"

$ApplicationPathCustomerAPIDockerFile = "$DeploymentRoot\APIs\S2Search.CustomerResource.API\src\api\CustomerResource\"
Write-Color -Text "The ApplicationPathCustomerAPIDockerFile is -> $ApplicationPathCustomerAPIDockerFile" -Color Blue

$ApplicationPathCustomerAPIContext = "$DeploymentRoot\APIs\S2Search.CustomerResource.API"
Write-Color -Text "The ApplicationPathCustomerAPIContext is -> $ApplicationPathCustomerAPIContext" -Color Blue

$ApplicationPathClientDockerFile = "$DeploymentRoot\APIs\S2Search.ClientConfiguration.API\src\api\ClientConfiguration\"
Write-Color -Text "The ApplicationPathClientDockerFile is -> $ApplicationPathClientDockerFile" -Color Blue

$ApplicationPathClientConfigContext = "$DeploymentRoot\APIs\S2Search.ClientConfiguration.API"
Write-Color -Text "The ApplicationPathClientConfigContext is -> $ApplicationPathClientConfigContext" -Color Blue

$ApplicationPathSearchAPI = "$DeploymentRoot\SearchAPIs\AzureCognitiveServices\S2Search.Search.API\Search\"
Write-Color -Text "The ApplicationPathSearchAPI is -> $ApplicationPathSearchAPI" -Color Blue

$ApplicationPathSearchAPIContext = "$DeploymentRoot\SearchAPIs\AzureCognitiveServices\S2Search.Search.API"
Write-Color -Text "The ApplicationPathSearchAPIContext is -> $ApplicationPathSearchAPIContext" -Color Blue

$ApplicationPathElasticAPI = "$DeploymentRoot\SearchAPIs\ElasticSearch\S2Search.Elastic.API\Search\"
Write-Color -Text "The ApplicationPathElasticAPI is -> $ApplicationPathElasticAPI" -Color Blue

$ApplicationPathElasticAPIContext = "$DeploymentRoot\SearchAPIs\ElasticSearch\S2Search.Elastic.API"
Write-Color -Text "The ApplicationPathElasticAPIContext is -> $ApplicationPathElasticAPIContext" -Color Blue

$ApplicationPathElasticUI = "$DeploymentRoot\SearchUIs\ElasticSearch\S2Search.Elastic.NextJS.ReactUI\S2Search"
Write-Color -Text "The ApplicationPathElasticUI is -> $ApplicationPathElasticUI" -Color Blue

$ApplicationPathSearchUI = "$DeploymentRoot\SearchUIs\AzureCognitiveServices\S2Search.Search.NextJS.ReactUI\S2Search"
Write-Color -Text "The ApplicationPathSearchUI is -> $ApplicationPathSearchUI" -Color Blue

$ApplicationPathAdminUI = "$DeploymentRoot\AdminUI\S2Search.Admin.NextJS.ReactUI\s2admin"
Write-Color -Text "The ApplicationPathAdminUI is -> $ApplicationPathAdminUI" -Color Blue

$ApplicationPathAdminAPIFile = "$DeploymentRoot\APIs\S2Search.Admin.API\Admin"
Write-Color -Text "The ApplicationPathClientDockerFile is -> $ApplicationPathClientDockerFile" -Color Blue

$ApplicationPathAdminAPIContext = "$DeploymentRoot\APIs\S2Search.Admin.API"
Write-Color -Text "The ApplicationPathAdminAPIContext is -> $ApplicationPathAdminAPIContext" -Color Blue



$S2Namespace = "s2"

if ($deleteAllImages) {
    
    # delete S2 Namespace
    Write-Color -Text "Delete all resources in the $S2Namespace Namespace" -Color Yellow
    kubectl delete all --all -n $S2Namespace

    Write-Color -Text "###########################################################" -Color Yellow
    Write-Color -Text "Waiting 15 seconds after $S2Namespace namespace has been deleted - stand by..." -Color Yellow
    Write-Color -Text "###########################################################" -Color Yellow
    Start-Sleep -Milliseconds 15000
}

if ($deleteAllImages) {    

    Write-Color -Text "################################" -Color DarkMagenta
    Write-Color -Text "Deleteing Old Docker Images" -Color DarkMagenta
    Write-Color -Text "################################" -Color DarkMagenta

    docker rmi $(docker images 's2elasticui:dev' -q) -f
    docker rmi $(docker images 's2searchui:dev' -q) -f
    docker rmi $(docker images 's2adminui:dev' -q) -f
    docker rmi $(docker images 's2searchapi:dev' -q) -f
    docker rmi $(docker images 's2elasticapi:dev' -q) -f
    docker rmi $(docker images 's2clientconfigurationapi:dev' -q) -f
    docker rmi $(docker images 's2customerresourceapi:dev' -q) -f
    docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "s2elasticui") -f
    docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "s2searchui") -f
    docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "s2adminui") -f
    docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "s2searchapi") -f
    docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "s2elasticapi") -f
    docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "s2customerresourceapi") -f
    docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "s2clientconfigurationapi") -f
    
    ###################################################################################
    # dont delete the dependent images as we keep hitting docker repo pull rate limits
    ###################################################################################
    #docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "redis") -f
    #docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "kibana") -f
    #docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "elasticsearch") -f
    #docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "mysql") -f
    #docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}" | findstr "drakkan/sftpgo") -f
    #docker image prune -a -f

    docker volume prune -f
}

Write-Color -Text "################################" -Color DarkCyan
Write-Color -Text "Create $S2Namespace Namespace" -Color DarkCyan
Write-Color -Text "################################" -Color DarkCyan
kubectl create namespace $S2Namespace

# redis cluster
#$RedisPath = "$DeploymentRoot\K8s\K8s.Local.Development.Environment\redis"
#$RedisSentinelPath = "$RedisPath\sentinel"

#redis single instance
$RedisPath = "$DeploymentRoot\K8s\K8s.Local.Development.Environment\redis\SingleInstance"
Write-Color -Text "The RedisPath is -> $RedisPath" -Color Blue

#Elastic Cluster
$ElasticSearchPath = "$DeploymentRoot\K8s\K8s.Local.Development.Environment\ElasticSearch"
Write-Color -Text "The elastic path is -> $ElasticSearchPath" -Color Blue

#sftpgo + MySQL
$SFTPGoPath = "$DeploymentRoot\K8s\K8s.Local.Development.Environment\SFTPGo"
$MySQLPath = "$DeploymentRoot\K8s\K8s.Local.Development.Environment\MySql"

Write-Color -Text "The SFTPGoPath is -> $SFTPGoPath" -Color Blue
Write-Color -Text "The MySQLPath is -> $MySQLPath" -Color Blue

$S2ElasticUIURL = "http://localhost:2999/vehicletest"
$S2SearchUIURL = "http://localhost:3000/vehicletest"
$S2AdminURL = "http://localhost:3001/api/probe/ready"
$Timeout = 2500

Start-Sleep -Milliseconds $Timeout 

Write-Color -Text "################################" -Color Green
Write-Color -Text "Old Docker Images removed" -Color Green
Write-Color -Text "################################" -Color Green

Start-Sleep -Milliseconds $Timeout 

if ($includeRedis) {

    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Setup Redis Cluster" -Color Yellow
    Write-Color -Text "################################" -Color Yellow

    Set-Location $RedisPath

    kubectl apply -f ./redis-configmap.yaml --namespace=$S2Namespace
    kubectl apply -f ./redis-statefulset.yaml --namespace=$S2Namespace

    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Redis Cluster deployed sucessfully" -Color Yellow
    Write-Color -Text "################################" -Color Yellow
}

if ($includeSftpGo) {

    Write-Color -Text "################################" -Color Green
    Write-Color -Text "MySQL Database" -Color Green
    Write-Color -Text "################################" -Color Green

    Set-Location $MySQLPath

    kubectl apply -f mysql-deploy.yml --namespace=$S2Namespace
    kubectl apply -f mysql-pv.yml --namespace=$S2Namespace

    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Setup SFTPGo " -Color Yellow
    Write-Color -Text "################################" -Color Yellow
    
    Set-Location $SFTPGoPath

    kubectl apply -f ./sftpgo-config.yml --namespace=$S2Namespace
    kubectl apply -f ./sftpgo-deploy.yml --namespace=$S2Namespace
    kubectl apply -f ./sftpgo-service.yml --namespace=$S2Namespace
}

if ($includeElastic) {

    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Setup Elastic" -Color Yellow
    Write-Color -Text "################################" -Color Yellow

    Set-Location $ElasticSearchPath

    kubectl apply -f ./elastic-statefulset.yml --namespace=$S2Namespace
    kubectl apply -f ./elastic-service.yml --namespace=$S2Namespace
    kubectl apply -f ./elastic-loadbalancer.yml --namespace=$S2Namespace
    
    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Setup Kibana" -Color Yellow
    Write-Color -Text "################################" -Color Yellow    

    kubectl apply -f ./kibana-deployment.yml --namespace=$S2Namespace
    kubectl apply -f ./kibana-loadbalancer.yml --namespace=$S2Namespace
}

Write-Color -Text "################################" -Color Green
Write-Color -Text "Starting Docker Images" -Color Green
Write-Color -Text "################################" -Color Green

if ($includeElasticUI) {
    Set-Location $ApplicationPathElasticUI
    npm install
    Write-Color -Text "Building Docker Image - S2 Elastic UI (NextJS) at location $ApplicationPathElasticUI" -Color Magenta    
    Write-Color -Text "docker build --pull --rm -f Dockerfile -t s2elasticui:dev . --build-arg NODE_ENV=production" -Color Yellow
    docker build --pull --rm -f "Dockerfile" -t s2elasticui:dev . --build-arg NODE_ENV=production

    Write-Color -Text "Deleting Deployment 's2elasticui-deployment'" -Color Magenta
    kubectl delete deployment s2elasticui-deployment --namespace s2
}

if ($includeSearchUI) {
    Set-Location $ApplicationPathSearchUI
    npm install
    Write-Color -Text "Building Docker Image - S2 Search UI (NextJS) at location $ApplicationPathSearchUI" -Color Magenta    
    Write-Color -Text "docker build --pull --rm -f Dockerfile -t s2searchui:dev . --build-arg NODE_ENV=production" -Color Yellow
    docker build --pull --rm -f "Dockerfile" -t s2searchui:dev . --build-arg NODE_ENV=production

    Write-Color -Text "Deleting Deployment 's2searchui-deployment'" -Color Magenta
    kubectl delete deployment s2searchui-deployment --namespace s2
}

if ($includeAdminUI) {
    Set-Location $ApplicationPathAdminUI
    npm install
    Write-Color -Text "Building Docker Image - S2 Admin UI  (ReactJS) at location $ApplicationPathAdminUI" -Color Magenta
    Write-Color -Text "docker build --pull --rm -f Dockerfile -t s2adminui:dev ." -Color Yellow
    docker build --pull --rm -f "Dockerfile" -t s2adminui:dev .

    Write-Color -Text "Deleting Deployment 's2adminui-deployment'" -Color Magenta
    kubectl delete deployment s2adminui-deployment --namespace s2
}

if ($includeSearchAPI) {
    Set-Location $ApplicationPathSearchAPI
    Write-Color -Text "Building Docker Image - Search API at location $ApplicationPathSearchAPI and context $ApplicationPathSearchAPIContext" -Color Magenta
    Write-Color -Text "docker build --pull --rm -f Dockerfile.local -t s2searchapi:dev $ApplicationPathSearchAPIContext --build-arg PAT=$PatToken" -Color Yellow
    docker build --pull --rm -f "Dockerfile.local" -t s2searchapi:dev $ApplicationPathSearchAPIContext --build-arg PAT=$PatToken

    Write-Color -Text "Deleting Deployment 's2searchapi-deployment'" -Color Magenta
    kubectl delete deployment s2searchapi-deployment --namespace s2
}

if ($includeElasticAPI) {
    Set-Location $ApplicationPathElasticAPI
    Write-Color -Text "Building Docker Image - Elastic API at location $ApplicationPathElasticAPI and context $ApplicationPathElasticAPIContext" -Color Magenta
    Write-Color -Text "docker build --pull --rm -f Dockerfile.local -t s2elasticapi:dev $ApplicationPathElasticAPIContext --build-arg PAT=$PatToken" -Color Yellow
    docker build --pull --rm -f "Dockerfile.local" -t s2elasticapi:dev $ApplicationPathElasticAPIContext --build-arg PAT=$PatToken

    Write-Color -Text "Deleting Deployment 's2elasticapi-deployment'" -Color Magenta
    kubectl delete deployment s2elasticapi-deployment --namespace s2
}

if ($includeAdminAPI) {
    Set-Location $ApplicationPathAdminAPIFile 
    Write-Color -Text "Building Docker Image - Admin API at location $ApplicationPathAdminAPIFile and context $ApplicationPathAdminAPIContext" -Color Magenta
    Write-Color -Text "docker build --pull --rm -f Dockerfile.local -t s2adminapi:dev $ApplicationPathAdminAPIContext --build-arg PAT=$PatToken" -Color Yellow
    docker build --pull --rm -f "Dockerfile.local" -t s2adminapi:dev $ApplicationPathAdminAPIContext --build-arg PAT=$PatToken

    Write-Color -Text "Deleting Deployment 's2adminapi-deployment'" -Color Magenta
    kubectl delete deployment s2adminapi-deployment --namespace s2
}

Write-Color -Text "################################" -Color Magenta
Write-Color -Text "Docker Images build sucessfully" -Color Magenta
Write-Color -Text "################################" -Color Magenta

Write-Color -Text "################################" -Color Green
Write-Color -Text "Creating K8s Deployments to local cluster" -Color Green
Write-Color -Text "################################" -Color Green

# - 3 - deploy each of the services back to k8s
Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2SearchAPI"
kubectl apply -f .\ConfigMaps\configmap-localk8s.yml --namespace=$S2Namespace
kubectl apply -f deployment.yml --namespace=$S2Namespace

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2ElasticAPI"
kubectl apply -f .\ConfigMaps\configmap-localk8s.yml --namespace=$S2Namespace
kubectl apply -f deployment.yml --namespace=$S2Namespace

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2ElasticUI"
kubectl apply -f deployment.yml --namespace=$S2Namespace

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2SearchUI"
kubectl apply -f deployment.yml --namespace=$S2Namespace

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2AdminUI"
kubectl apply -f deployment.yml --namespace=$S2Namespace

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2AdminAPI"
kubectl apply -f deployment.yml --namespace=$S2Namespace

Write-Color -Text "################################" -Color Green
Write-Color -Text "Creating K8s Services to local cluster" -Color Green
Write-Color -Text "################################" -Color Green

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2SearchAPI"
kubectl apply -f service-loadbalancer.yml --namespace=$S2Namespace
kubectl apply -f service-clusterip.yml --namespace=$S2Namespace

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2ElasticAPI"
kubectl apply -f service-loadbalancer.yml --namespace=$S2Namespace
kubectl apply -f service-clusterip.yml --namespace=$S2Namespace

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2ElasticUI"
kubectl apply -f service-loadbalancer.yml --namespace=$S2Namespace

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2SearchUI"
kubectl apply -f service-loadbalancer.yml --namespace=$S2Namespace

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2AdminUI"
kubectl apply -f service-loadbalancer.yml --namespace=$S2Namespace

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment\S2AdminAPI"
kubectl apply -f service-loadbalancer.yml --namespace=$S2Namespace
kubectl apply -f service-clusterip.yml --namespace=$S2Namespace

if ($includeSearchUI -and $includeAdminUI) {
    Write-Color -Text "###########################################################" -Color Yellow
    Write-Color -Text "Waiting 15 seconds for Pods to become active - stand by..." -Color Yellow
    Write-Color -Text "###########################################################" -Color Yellow
    Start-Sleep -Milliseconds 15000
}

if ($includeSearchUI) {
    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Test Search UI Page" -Color Yellow
    Write-Color -Text "endpoint $S2SearchUIURL" -Color Yellow
    Write-Color -Text "################################" -Color Yellow
    Test-Application -Endpoint $S2SearchUIURL -TimeoutMs 10000
}

if ($includeAdminUI) {
    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Test Admin UI Page" -Color Yellow
    Write-Color -Text "endpoint $S2AdminURL" -Color Yellow
    Write-Color -Text "################################" -Color Yellow
    Test-Application -Endpoint $S2AdminURL -TimeoutMs 10000
}

if ($includeElasticUI) {
    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Test Elastic UI Page" -Color Yellow
    Write-Color -Text "endpoint $S2ElasticUIURL" -Color Yellow
    Write-Color -Text "################################" -Color Yellow
    Test-Application -Endpoint $S2ElasticUIURL -TimeoutMs 10000
}

Set-Location "$DeploymentRoot\K8s\K8s.Local.Development.Environment"

Write-Color -Text "################################" -Color Green
Write-Color -Text "Process Complete" -Color Green
Write-Color -Text "################################" -Color Green