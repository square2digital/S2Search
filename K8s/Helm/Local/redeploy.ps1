# cls; cd "E:\github\S2Search\K8s\Helm\Local"; .\redeploy.ps1
cls;

kubectl config use-context rancher-desktop
# kubectl config use-context s2search-aks-dev

cd "E:\github\S2Search\Terraform"

#terraform output -json | ConvertFrom-Json
$tfOutput = terraform output -json | ConvertFrom-Json

$defaultResourceGroup = $tfOutput.default_resource_group_name.value
$storageAccountName = $tfOutput.storage_account_name.value

$searchCredentialsQueryKey = az search query-key list --resource-group $defaultResourceGroup --service-name s2-search-dev --output tsv --query "[0].key"
$searchServiceName = (az search service show --resource-group $defaultResourceGroup --name s2-search-dev | ConvertFrom-Json).name
$searchCredentialsInstanceEndpoint = "https://$searchServiceName.search.windows.net"
$storageConnectionString = (az storage account show-connection-string --resource-group $defaultResourceGroup --name $storageAccountName --output tsv)

$databasePassword = "";
$databaseConnectionString = "Host=s2search-postgresql;Port=5432;Database=s2searchdb;Username=s2search;Password=$databasePassword";
$redisConnectionString = "s2search-redis-master:6379";

cd "E:\github\S2Search\K8s\Helm\Local"; 

Write-Host "defaultResourceGroup - $defaultResourceGroup"
Write-Host "storageAccountName - $storageAccountName"
Write-Host "searchCredentialsQueryKey - $searchCredentialsQueryKey"
Write-Host "searchServiceName - $searchServiceName"
Write-Host "searchCredentialsInstanceEndpoint - $searchCredentialsInstanceEndpoint"
Write-Host "storageConnectionString - $storageConnectionString"

helm upgrade --install s2search . -n s2search `
    --set-string postgresql.auth.password=$databasePassword `
    --set-string postgresql.auth.connectionString="$databaseConnectionString" `
    --set-string ConnectionStrings.databaseConnectionString="$databaseConnectionString" `
    --set-string ConnectionStrings.azureStorageConnectionString=$storageConnectionString `
    --set-string ConnectionStrings.redisConnectionString=$redisConnectionString `
    --set-string feedfunctions.azureStorage.connectionString=$storageConnectionString `
    --set-string searchinsights.azureStorage.connectionString=$storageConnectionString `
    --set-string Search.searchCredentialsQueryKey=$searchCredentialsQueryKey `
    --set-string Search.searchCredentialsInstanceEndpoint=$searchCredentialsInstanceEndpoint `
    --set-string Storage.accountName=$storageAccountName;

docker image prune -f;
