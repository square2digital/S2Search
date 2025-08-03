#################
# execute script
#################
# clear; cd "C:\git\s2-search-Infrastructure\kubernetes\azure\helm"; .\k8s-helm-namespace-deployment.ps1 -includeDelete $false -includeInstall $false -namespace "s2" -cluster "s2-searchdemolabk8scluster"
# pass the flags -includeDelete $false -includeInstall $true to delete everything and re-install or to run just one of the actions

# delete examples
# clear; cd "C:\git\s2-search-Infrastructure\kubernetes\azure\helm"; .\k8s-helm-namespace-deployment.ps1 -includeDelete $true -includeInstall $false -namespace "s2" -cluster "s2-search"

#################
# WARNING
#################
# Running this script too many times with flag -includeDelete true will result in Let's Encrypt rate-limiting the provisioning of new certificates, resulting in the application using the Kubernetes Ingress Controller's fake certificate.
# In this state, the system will experience failures due to SSL errors. It takes 6 hours for the certificates to be available again for re-provisioning.

param (
    [bool]$includeDelete = $true,
    [bool]$includeInstall = $true,
    [string]$namespace = "",
    [string]$cluster = ""
)

# suppoted colour values - Black DarkBlue DarkGreen DarkCyan DarkRed DarkMagenta DarkYellow Gray DarkGray Blue Green Cyan Red Magenta Yellow White
function Write-Color([String[]]$Text, [ConsoleColor[]]$Color) {
    for ($i = 0; $i -lt $Text.Length; $i++) {
        Write-Host $Text[$i] -Foreground $Color[$i] -NoNewLine
    }
    Write-Host
}

if ([string]::IsNullOrEmpty($namespace)) {
    Write-Color -Text "The parameter 'namespace' has not been passed - this needs to be the environment namespace" -Color Red
    exit
}

if ([string]::IsNullOrEmpty($cluster)) {
    Write-Color -Text "The parameter 'cluster' has not been passed - this needs to be the k8s cluster" -Color Red
    exit
}

$input = Read-Host "deployment ready for namespace = '$namespace' cluster = '$cluster' - Do you want to continue? (y/n)"

if ($input -eq 'y') {
    Write-Color -Text "Continuing..." -Color Green
}
elseif ($input -eq 'n') {
    Write-Color -Text "Exiting..." -Color Red
    exit
}
else {
    Write-Color -Text "Invalid input, exiting..." -Color Red
    exit
}

Write-Color -Text "Settings K8s context to $cluster" -Color DarkCyan
kubectl config use-context $cluster

# #############################################################
# Ensure that the current context is set to the Azure Cluster "s2-search"
# if not throw an exception and immediately exit script
# #############################################################

$azureContext = Invoke-Expression -Command "kubectl config current-context"
$localContext = "docker-desktop"

if ($context -eq $localContext) {
    Invoke-Expression -Command "kubectl config use-context docker-desktop"
    throw "The K8s context is not $azureContext but is actually $localContext - the context must be set as $azureContext - the context has now been updated to $azureContext"
}

# Variables
$releaseName = "s2-search-$namespace"
$DeploymentRoot = "E:\devops\K8s.Local.Cluster.Setup\Kubernetes\Azure\Helm\"
$certManagerVersion = "v1.15.3"
$releaseNameCertManager = "cert-manager-$namespace"

# Ingress & Cert Manager
$releaseNameCertManager = "cert-manager-$namespace"

# TLS Certs
$tlsIssuerPrivateKeyPath = "C:\git\s2-search-Infrastructure\kubernetes\azure\blob-storage\ci-k8s\certs\private.key.pem"
$PrivateKeyName = "my-issuer-private-key"

Set-Location $DeploymentRoot

if ($includeDelete) {

    Write-Color -Text "##########################################################" -Color DarkRed
    Write-Color -Text "Uninstall helm install $releaseName and dependencies"       -Color DarkRed
    Write-Color -Text "##########################################################" -Color DarkRed
    helm uninstall $releaseName --namespace $namespace
    helm uninstall $releaseNameElasticSearch --namespace $namespace
    helm uninstall $releaseNameNeo4j --namespace $namespace
    helm uninstall $releaseNameCertManager --namespace $namespace

    Write-Color -Text "################################" -Color DarkRed
    Write-Color -Text "Deleting Secrets"                 -Color DarkRed
    Write-Color -Text "################################" -Color DarkRed
    kubectl delete secret $PrivateKeyName --namespace $namespace
    kubectl delete namespace $namespace

    Write-Color -Text "######################################################" -Color DarkRed
    Write-Color -Text "View Helm releases in this namespace - $namespace"      -Color DarkRed
    Write-Color -Text "######################################################" -Color DarkRed
    helm ls -n $namespace
}

Write-Color -Text "##########################################" -Color DarkYellow
Write-Color -Text "Check if the namespace '$namespace' exists" -Color DarkYellow
Write-Color -Text "##########################################" -Color DarkYellow

$namespaceExists = kubectl get namespace $namespace --ignore-not-found

if (-not $namespaceExists) {
    kubectl create namespace $namespace
    Write-Color -Text "Namespace '$namespace' created" -Color Green
}
else {
    Write-Color "Namespace '$namespace' already exists." -Color Yellow
}

############################################################
## Installation of s2-search ##############################
############################################################

if ($includeInstall) {
    Write-Color -Text "################################" -Color DarkCyan
    Write-Color -Text "Install cert-manager CRDs "       -Color DarkCyan
    Write-Color -Text "################################" -Color DarkCyan
    kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/$certManagerVersion/cert-manager.crds.yaml
    helm repo add jetstack "https://charts.jetstack.io"
    helm repo update jetstack

    Write-Color -Text "################################" -Color DarkCyan
    Write-Color -Text "Install cert-manager"             -Color DarkCyan
    Write-Color -Text "################################" -Color DarkCyan
    helm upgrade --install $releaseNameCertManager jetstack/cert-manager `
        --wait `
        --debug `
        --create-namespace `
        --namespace $namespace `
        --version $certManagerVersion `
        --set installCRDs=false

    Write-Color -Text "################################" -Color DarkCyan
    Write-Color -Text "Install Ingress SSL Secrets"      -Color DarkCyan
    Write-Color -Text "################################" -Color DarkCyan
    kubectl create secret generic $PrivateKeyName --from-file=$tlsIssuerPrivateKeyPath `
        --namespace $namespace

    Write-Color -Text "###################" -Color Green
    Write-Color -Text "Install S2 Search "     -Color Green
    Write-Color -Text "###################" -Color Green
    helm upgrade --install $releaseName . -f values.yaml `
        --namespace $namespace `
        --set subDomain=$namespace `
        --wait `
        --debug

    Write-Color -Text "#################################################" -Color Green
    Write-Color -Text "View Helm releases in this namespace - $namespace" -Color Green
    Write-Color -Text "#################################################" -Color Green
    helm ls -n $namespace
}

Write-Color -Text "################################"  -Color Green
Write-Color -Text "Process Complete"                  -Color Green
Write-Color -Text  "################################" -Color Green

Write-Color -Text "################################" -Color Yellow
Write-Color -Text "WARNING - Lets Enctypt Rate Limit Risk" -Color Yellow
Write-Color -Text "################################" -Color Yellow
Write-Color -Text "Running this script too many times with flag -includeDelete true will result in Let's Encrypt rate-limiting the provisioning of new certificates, resulting in the application using the Kubernetes Ingress Controller's fake certificate." -Color Red
Write-Color -Text "In this state, the system will experience failures due to SSL errors. It takes 6 hours for the certificates to be available again for re-provisioning." -Color Red