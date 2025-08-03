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
# - Build Command
#############################################################

# build everything - exclude redundant ConfigAPI & includeCRAPI
# cls; cd "E:\devops\K8s.Local.Cluster.Setup\local"; .\deployment-script.ps1 -includeElasticUI $true -includeSearchUI $true -includeAdminUI $true -includeConfigAPI $false -includeSearchAPI $true -includeElasticAPI $true -includeCRAPI $false -includeRedis $true -includeSftpGo $true -includeElastic $true -deleteAllImages $false -includeAdminAPI $true -deleteS2Namespace $true

#############################################################
# - replace with your path to use the example Build Command below 
#############################################################

# build nothing
# cls; cd "E:\devops\K8s.Local.Cluster.Setup\local"; .\deployment-script.ps1 -includeElasticUI $false -includeSearchUI $false -includeAdminUI $false -includeConfigAPI $false -includeSearchAPI $false -includeElasticAPI $false -includeCRAPI $false -includeRedis $false -includeSftpGo $false -includeElastic $false -deleteAllImages $false -includeAdminAPI $false -deleteS2Namespace $false

# build specifc service only - Elastic UI & API in this case
# cls; cd "E:\devops\K8s.Local.Cluster.Setup\local"; .\deployment-script.ps1 -includeElasticUI $true -includeSearchUI $true -includeAdminUI $false -includeConfigAPI $false -includeSearchAPI $false -includeElasticAPI $false -includeCRAPI $false -includeRedis $false -includeSftpGo $false -includeElastic $false -deleteAllImages $false -includeAdminAPI $true -deleteS2Namespace $false

# build just Admin UI
# cls; cd "E:\devops\K8s.Local.Cluster.Setup\local"; .\deployment-script.ps1 -includeElasticUI $false -includeSearchUI $false -includeAdminUI $true -includeConfigAPI $false -includeSearchAPI $false -includeElasticAPI $false -includeCRAPI $false -includeRedis $false -includeSftpGo $false -includeElastic $false -deleteAllImages $false -includeAdminAPI $false -deleteS2Namespace $false

#######################
# debug the elastic UI
#######################
# build everything
# cls; cd "E:\devops\K8s.Local.Cluster.Setup\local"; .\deployment-script.ps1 -includeElasticUI $true -includeSearchUI $true -includeAdminUI $true -includeConfigAPI $true -includeSearchAPI $true -includeElasticAPI $true -includeCRAPI $true -includeRedis $true -includeSftpGo $true -includeElastic $true -deleteAllImages $true -includeAdminAPI $true -deleteS2Namespace $true

##############################
## remove redundant APIs - superceeded by the AdminAPI
## ConfigAPI
## CRAPI
##############################
# cls; cd "E:\devops\K8s.Local.Cluster.Setup\local"; .\deployment-script.ps1 -includeElasticUI $true -includeSearchUI $true -includeAdminUI $true -includeConfigAPI $false -includeSearchAPI $true -includeElasticAPI $true -includeCRAPI $false -includeRedis $true -includeSftpGo $false -includeElastic $true -deleteAllImages $false -includeAdminAPI $true -deleteS2Namespace $true

param (
    [bool]$includeElasticUI = $false,
    [bool]$includeSearchUI = $false,
    [bool]$includeAdminUI = $false,
    
    [bool]$includeSearchAPI = $false,
    [bool]$includeElasticAPI = $false,
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
    param(
        [string]$ApplicationName,
        [string]$Endpoint,
        [int]$TimeoutMs
    )

    $DelayMs = 2500
    $Response = $null

    do {
        Write-Progress -Activity "Testing $ApplicationName endpoint" -SecondsRemaining ($TimeoutMs / 1000)
        Start-Sleep -Milliseconds $DelayMs
        $TimeoutMs -= $DelayMs

        try {
            $Response = Invoke-WebRequest -Uri $Endpoint

            if ($Response.StatusCode -eq 200) {
                Write-Host -ForegroundColor Green "$ApplicationName on URL $Endpoint responded with a 200"
            }
        }
        catch [System.Net.WebException] {
            $_.Exception.Response
        }
    }
    until ($Response.StatusCode -eq 200 -or $TimeoutMs -le 0)

    if ($TimeoutMs -le 0) {
        throw "Testing $ApplicationName with endpoint - $Endpoint timed out"
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

function Get-Environment-Logo {

    return @'

 _                          _   _   __ _____      
| |                        | | | | / /|  _  |     
| |      ___    ___   __ _ | | | |/ /  \ V /  ___ 
| |     / _ \  / __| / _` || | |    \  / _ \ / __|
| |____| (_) || (__ | (_| || | | |\  \| |_| |\__ \
\_____/ \___/  \___| \__,_||_| \_| \_/\_____/|___/                                                                                                    

'@

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
Output-Parameters -param $deleteS2Namespace -name "deleteS2Namespace"

# #############################################################
# Ensure that the current context is set to the Azure K8s Cluster
# if not throw an exception and immediately exit script
# #############################################################

$context = Invoke-Expression -Command "kubectl config current-context"
$azureAksContext = "aks-k8s-westeu"
$gitlabPassword = "glpat-58mqCnQM_zDE67ymwZc5"
$gitlabEndpoint = "registry.gitlab.com"
$letsEncryptCertPath = "C:\Certbot\live\s2search.co.uk-0001"

############################################################################################
# this is a fail safe to ensure changes are only made to the AKS cluster - not local docker
############################################################################################
if ($context -ne $azureAksContext) {
    Invoke-Expression -Command "kubectl config use-context $azureAksContext"    
    throw "The K8s context is not $azureAksContext but is actually $context - the context must be set as $azureAksContext - the context has now been updated to $azureAksContext"
}

$logoString = Get-Environment-Logo
Write-Host $logoString

############
# Variables
############

# the PatToken is for the "Azure DevOps Artifacts Credentials Provider" which allows the docker images
# when built to pull down dependacies from the DevOps artifacts repo "square2digital"
$PatToken = "2f2luh3n7klqqnw5ofulaarwlyj7rtrhmya7uvfzmraxbyia5aia"
$DeploymentRoot = "E:\devops"

Write-Color -Text "PatToken is -> $PatToken" -Color Magenta
Write-Color -Text "DeploymentRoot is -> $DeploymentRoot" -Color Magenta

$ApplicationPathCustomerAPIDockerFile = "$DeploymentRoot\S2Search.CustomerResource.API\src\api\CustomerResource\"
Write-Color -Text "The ApplicationPathCustomerAPIDockerFile is -> $ApplicationPathCustomerAPIDockerFile" -Color Blue

$ApplicationPathCustomerAPIContext = "$DeploymentRoot\S2Search.CustomerResource.API"
Write-Color -Text "The ApplicationPathCustomerAPIContext is -> $ApplicationPathCustomerAPIContext" -Color Blue

$ApplicationPathClientDockerFile = "$DeploymentRoot\S2Search.ClientConfiguration.API\src\api\ClientConfiguration\"
Write-Color -Text "The ApplicationPathClientDockerFile is -> $ApplicationPathClientDockerFile" -Color Blue

$ApplicationPathClientConfigContext = "$DeploymentRoot\S2Search.ClientConfiguration.API"
Write-Color -Text "The ApplicationPathClientConfigContext is -> $ApplicationPathClientConfigContext" -Color Blue

$ApplicationPathSearchAPI = "$DeploymentRoot\S2Search.Search.API\Search\"
Write-Color -Text "The ApplicationPathSearchAPI is -> $ApplicationPathSearchAPI" -Color Blue

$ApplicationPathSearchAPIContext = "$DeploymentRoot\S2Search.Search.API"
Write-Color -Text "The ApplicationPathSearchAPIContext is -> $ApplicationPathSearchAPIContext" -Color Blue

$ApplicationPathElasticAPI = "$DeploymentRoot\S2Search.Elastic.API\Search\"
Write-Color -Text "The ApplicationPathElasticAPI is -> $ApplicationPathElasticAPI" -Color Blue

$ApplicationPathElasticAPIContext = "$DeploymentRoot\S2Search.Elastic.API"
Write-Color -Text "The ApplicationPathElasticAPIContext is -> $ApplicationPathElasticAPIContext" -Color Blue

$ApplicationPathElasticUI = "$DeploymentRoot\S2Search.Elastic.NextJS.ReactUI\S2Search"
Write-Color -Text "The ApplicationPathElasticUI is -> $ApplicationPathElasticUI" -Color Blue

$ApplicationPathSearchUI = "$DeploymentRoot\S2Search.Search.NextJS.ReactUI\S2Search"
Write-Color -Text "The ApplicationPathSearchUI is -> $ApplicationPathSearchUI" -Color Blue

$ApplicationPathAdminUI = "$DeploymentRoot\S2Search.Admin.NextJS.ReactUI\s2admin"
Write-Color -Text "The ApplicationPathAdminUI is -> $ApplicationPathAdminUI" -Color Blue

$ApplicationPathAdminAPIFile = "$DeploymentRoot\S2Search.Admin.API\Admin"
Write-Color -Text "The ApplicationPathClientDockerFile is -> $ApplicationPathClientDockerFile" -Color Blue

$ApplicationPathAdminAPIContext = "$DeploymentRoot\S2Search.Admin.API"
Write-Color -Text "The ApplicationPathAdminAPIContext is -> $ApplicationPathAdminAPIContext" -Color Blue

#$S2Namespace = "s2"
$S2Namespace = "ingress-nginx"
$IngressNginxNamespace = "ingress-nginx"


$S2Namespace = "s2"

if ($deleteS2Namespace) {
    
    # delete S2 Namespace
    Write-Color -Text "Before $S2Namespace Delete" -Color Yellow
    kubectl get all -n $S2Namespace

    Write-Color -Text "Delete the $S2Namespace namespace including all resources" -Color Yellow
    kubectl delete namespace $S2Namespace

    Write-Color -Text "###########################################################" -Color Yellow
    Write-Color -Text "Waiting 15 seconds after $S2Namespace namespace has been deleted - stand by..." -Color Yellow
    Write-Color -Text "###########################################################" -Color Yellow
    Start-Sleep -Milliseconds 15000

    Write-Color -Text "After $S2Namespace Delete" -Color Yellow
    kubectl get all -n 
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

    docker volume prune -f
}

Write-Color -Text "################################" -Color DarkCyan
Write-Color -Text "Create $S2Namespace Namespace" -Color DarkCyan
Write-Color -Text "################################" -Color DarkCyan
kubectl create namespace $S2Namespace

Write-Color -Text "####################################" -Color DarkCyan
Write-Color -Text "k8s secret for gitlab authentication" -Color DarkCyan
Write-Color -Text "####################################" -Color DarkCyan
kubectl create secret docker-registry gitlab-registry-secret --docker-server=$gitlabEndpoint --docker-username=$S2Namespace --docker-password=$gitlabPassword --namespace $S2Namespace

# redis cluster
#$RedisPath = "$DeploymentRoot\K8s.Local.Cluster.Setup\local\redis"
#$RedisSentinelPath = "$RedisPath\sentinel"

#redis single instance
$RedisPath = "$DeploymentRoot\K8s.Local.Cluster.Setup\local\redis\SingleInstance"
Write-Color -Text "The RedisPath is -> $RedisPath" -Color Blue

#Elastic Cluster
$ElasticSearchPath = "$DeploymentRoot\K8s.Local.Cluster.Setup\local\ElasticSearch"
Write-Color -Text "The elastic path is -> $ElasticSearchPath" -Color Blue

#sftpgo + MySQL
$SFTPGoPath = "$DeploymentRoot\K8s.Local.Cluster.Setup\local\SFTPGo"
$MySQLPath = "$DeploymentRoot\K8s.Local.Cluster.Setup\local\MySql"

Write-Color -Text "The SFTPGoPath is -> $SFTPGoPath" -Color Blue
Write-Color -Text "The MySQLPath is -> $MySQLPath" -Color Blue

# UI tests
$S2SearchUIURL = "https://demo.s2search.co.uk/vehicletest"
$S2AdminURL = "https://admin.s2search.co.uk/api/probe/ready"

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

    kubectl apply -f ./redis-configmap.yaml --namespace $S2Namespace
    kubectl apply -f ./redis-statefulset.yaml --namespace $S2Namespace

    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Redis Cluster deployed sucessfully" -Color Yellow
    Write-Color -Text "################################" -Color Yellow
}

if ($includeSftpGo) {

    Write-Color -Text "################################" -Color Green
    Write-Color -Text "MySQL Database" -Color Green
    Write-Color -Text "################################" -Color Green

    Set-Location $MySQLPath

    kubectl delete pvc mysql-pv-claim --namespace $S2Namespace
    kubectl delete pv mysql-pv-volume --namespace $S2Namespace
    kubectl apply -f mysql-deploy.yml --namespace $S2Namespace
    kubectl apply -f mysql-pv.yml --namespace $S2Namespace

    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Setup SFTPGo " -Color Yellow
    Write-Color -Text "################################" -Color Yellow
    
    Set-Location $SFTPGoPath

    kubectl apply -f ./sftpgo-config.yml --namespace $S2Namespace
    kubectl apply -f ./sftpgo-deploy.yml --namespace $S2Namespace
    
    # disable for now while we get it workinbg
    #kubectl apply -f ./sftpgo-loadbalancer.yml --namespace $S2Namespace
}

if ($includeElastic) {

    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Setup Elastic" -Color Yellow
    Write-Color -Text "################################" -Color Yellow

    Set-Location $ElasticSearchPath

    kubectl apply -f ./elastic-statefulset.yml --namespace $S2Namespace
    kubectl apply -f ./elastic-service.yml --namespace $S2Namespace
    kubectl apply -f ./elastic-loadbalancer.yml --namespace $S2Namespace
    
    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Setup Kibana" -Color Yellow
    Write-Color -Text "################################" -Color Yellow    

    kubectl apply -f ./kibana-deployment.yml --namespace $S2Namespace
    kubectl apply -f ./kibana-loadbalancer.yml --namespace $S2Namespace
}

Write-Color -Text "################################" -Color Green
Write-Color -Text "Starting Docker Images" -Color Green
Write-Color -Text "################################" -Color Green

if ($includeElasticUI) {
    Set-Location $ApplicationPathElasticUI
    npm install
    Write-Color -Text "Building Docker Image - S2 Elastic UI (NextJS) at location $ApplicationPathElasticUI" -Color Magenta    
    Write-Color -Text "docker build --pull --rm -f Dockerfile -t s2elasticui:dev . --build-arg NODE_ENV=production" -Color Yellow
    docker build --pull --rm -f "Dockerfile" -t s2elasticui:dev -t registry.gitlab.com/dp-dev0/s2search/s2elasticui:latest . --build-arg NODE_ENV=production
    Write-Color -Text "docker push registry.gitlab.com/dp-dev0/s2search/s2elasticui:latest" -Color Green
    docker push registry.gitlab.com/dp-dev0/s2search/s2elasticui:latest

    Write-Color -Text "Deleting Deployment 's2elasticui-deployment'" -Color Magenta
    kubectl delete deployment s2elasticui-deployment --namespace $S2Namespace

    Set-Location "$DeploymentRoot\K8s.Local.Cluster.Setup\local\S2ElasticUI"
    kubectl apply -f deployment.yml --namespace $S2Namespace
}

if ($includeSearchUI) {
    Set-Location $ApplicationPathSearchUI
    npm install
    Write-Color -Text "Building Docker Image - S2 Search UI (NextJS) at location $ApplicationPathSearchUI" -Color Magenta    
    Write-Color -Text "docker build --pull --rm -f Dockerfile -t s2searchui:dev . --build-arg NODE_ENV=production" -Color Yellow
    docker build --pull --rm -f "Dockerfile" -t s2searchui:dev -t registry.gitlab.com/dp-dev0/s2search/s2searchui:latest . --build-arg NODE_ENV=production
    Write-Color -Text "docker push registry.gitlab.com/dp-dev0/s2search/s2searchui:latest" -Color Green
    docker push registry.gitlab.com/dp-dev0/s2search/s2searchui:latest

    Write-Color -Text "Deleting Deployment 's2searchui-deployment'" -Color Magenta
    kubectl delete deployment s2searchui-deployment --namespace $S2Namespace

    Set-Location "$DeploymentRoot\K8s.Local.Cluster.Setup\local\S2SearchUI"
    kubectl apply -f deployment.yml --namespace $S2Namespace    
    kubectl apply -f service-clusterip.yml --namespace $S2Namespace
}

if ($includeAdminUI) {
    Set-Location $ApplicationPathAdminUI
    npm install
    Write-Color -Text "Building Docker Image - S2 Admin UI  (ReactJS) at location $ApplicationPathAdminUI" -Color Magenta
    Write-Color -Text "docker build --pull --rm -f Dockerfile -t s2adminui:dev ." -Color Yellow
    docker build --pull --rm -f "Dockerfile" -t s2adminui:dev -t registry.gitlab.com/dp-dev0/s2search/s2adminui:latest .
    Write-Color -Text "docker push registry.gitlab.com/dp-dev0/s2search/s2adminui:latest" -Color Green
    docker push registry.gitlab.com/dp-dev0/s2search/s2adminui:latest

    Write-Color -Text "Deleting Deployment 's2adminui-deployment'" -Color Magenta
    kubectl delete deployment s2adminui-deployment --namespace $S2Namespace

    Set-Location "$DeploymentRoot\K8s.Local.Cluster.Setup\local\S2AdminUI"
    kubectl apply -f deployment.yml --namespace $S2Namespace            
    kubectl apply -f service-clusterip.yml --namespace $S2Namespace    
}

if ($includeSearchAPI) {
    Set-Location $ApplicationPathSearchAPI
    Write-Color -Text "Building Docker Image - Search API at location $ApplicationPathSearchAPI and context $ApplicationPathSearchAPIContext" -Color Magenta
    Write-Color -Text "docker build --pull --rm -f Dockerfile.local -t s2searchapi:dev $ApplicationPathSearchAPIContext --build-arg PAT=$PatToken" -Color Yellow
    docker build --pull --rm -f "Dockerfile.local" -t s2searchapi:dev -t registry.gitlab.com/dp-dev0/s2search/s2searchapi:latest $ApplicationPathSearchAPIContext --build-arg PAT=$PatToken
    Write-Color -Text "docker push registry.gitlab.com/dp-dev0/s2search/s2searchapi:latest" -Color Green
    docker push registry.gitlab.com/dp-dev0/s2search/s2searchapi:latest

    Write-Color -Text "Deleting Deployment 's2searchapi-deployment'" -Color Magenta
    kubectl delete deployment s2searchapi-deployment --namespace $S2Namespace

    Set-Location "$DeploymentRoot\K8s.Local.Cluster.Setup\local\S2SearchAPI"
    kubectl apply -f .\ConfigMaps\configmap-localk8s.yml --namespace $S2Namespace
    kubectl apply -f deployment.yml --namespace $S2Namespace
    kubectl apply -f service-clusterip.yml --namespace $S2Namespace    
}

if ($includeElasticAPI) {
    Set-Location $ApplicationPathElasticAPI
    Write-Color -Text "Building Docker Image - Elastic API at location $ApplicationPathElasticAPI and context $ApplicationPathElasticAPIContext" -Color Magenta
    Write-Color -Text "docker build --pull --rm -f Dockerfile.local -t s2elasticapi:dev $ApplicationPathElasticAPIContext --build-arg PAT=$PatToken" -Color Yellow
    docker build --pull --rm -f "Dockerfile.local" -t s2elasticapi:dev -t registry.gitlab.com/dp-dev0/s2search/s2elasticapi:latest $ApplicationPathElasticAPIContext --build-arg PAT=$PatToken
    Write-Color -Text "docker push registry.gitlab.com/dp-dev0/s2search/s2elasticapi:latest" -Color Green    
    docker push registry.gitlab.com/dp-dev0/s2search/s2elasticapi:latest

    Write-Color -Text "Deleting Deployment 's2elasticapi-deployment'" -Color Magenta
    kubectl delete deployment s2elasticapi-deployment --namespace $S2Namespace

    Set-Location "$DeploymentRoot\K8s.Local.Cluster.Setup\local\S2ElasticAPI"
    kubectl apply -f .\ConfigMaps\configmap-localk8s.yml --namespace $S2Namespace
    kubectl apply -f deployment.yml --namespace $S2Namespace
    kubectl apply -f service-clusterip.yml --namespace $S2Namespace    
}

if ($includeAdminAPI) {
    Set-Location $ApplicationPathAdminAPIFile 
    Write-Color -Text "Building Docker Image - Admin API at location $ApplicationPathAdminAPIFile and context $ApplicationPathAdminAPIContext" -Color Magenta
    Write-Color -Text "docker build --pull --rm -f Dockerfile.local -t s2adminapi:dev $ApplicationPathAdminAPIContext --build-arg PAT=$PatToken" -Color Yellow
    docker build --pull --rm -f "Dockerfile.local" -t s2adminapi:dev -t registry.gitlab.com/dp-dev0/s2search/s2adminapi:latest $ApplicationPathAdminAPIContext --build-arg PAT=$PatToken
    Write-Color -Text "docker push registry.gitlab.com/dp-dev0/s2search/s2adminapi:latest" -Color Green
    docker push registry.gitlab.com/dp-dev0/s2search/s2adminapi:latest

    Write-Color -Text "Deleting Deployment 's2adminapi-deployment'" -Color Magenta
    kubectl delete deployment s2adminapi-deployment --namespace $S2Namespace

    Set-Location "$DeploymentRoot\K8s.Local.Cluster.Setup\local\S2AdminAPI"
    kubectl apply -f deployment.yml --namespace $S2Namespace    
    kubectl apply -f service-clusterip.yml --namespace $S2Namespace
}

Write-Color -Text "################################" -Color Magenta
Write-Color -Text "Docker Images build sucessfully" -Color Magenta
Write-Color -Text "################################" -Color Magenta

Write-Color -Text "################################" -Color Blue
Write-Color -Text "Create nginx-ingress" -Color Blue
Write-Color -Text "################################" -Color Blue

kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/controller-v1.8.2/deploy/static/provider/cloud/deploy.yaml

Write-Color -Text "###########################################################" -Color Yellow
Write-Color -Text "Waiting 25 seconds for the Ingress to become active - stand by..." -Color Yellow
Write-Color -Text "###########################################################" -Color Yellow
Start-Sleep -Milliseconds 25000

Write-Color -Text "################################" -Color Blue
Write-Color -Text "Create tls Secret" -Color Blue
Write-Color -Text "################################" -Color Blue

kubectl delete secret s2search-ssl-cert --namespace $S2Namespace
kubectl create secret tls s2search-ssl-cert --cert="$letsEncryptCertPath\cert.pem" --key="$letsEncryptCertPath\privkey.pem" --namespace $S2Namespace
kubectl apply -f "$DeploymentRoot\K8s.Local.Cluster.Setup\local\Kubernetes\ingress-controller\nginx\customers-ingress\s2search-ingress.yml" --namespace $S2Namespace

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
    Test-Application -ApplicationName "Azure Search UI" -Endpoint $S2SearchUIURL -TimeoutMs 10000
}

if ($includeAdminUI) {
    Write-Color -Text "################################" -Color Yellow
    Write-Color -Text "Test Admin UI Page" -Color Yellow
    Write-Color -Text "endpoint $S2AdminURL" -Color Yellow
    Write-Color -Text "################################" -Color Yellow
    Test-Application -ApplicationName "Admin UI" -Endpoint $S2AdminURL -TimeoutMs 10000
}

Write-Color -Text "################################" -Color Green
Write-Color -Text "Process Complete" -Color Green
Write-Color -Text "################################" -Color Green