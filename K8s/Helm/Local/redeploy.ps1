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
$searchCredentialsInstanceEndpoint = (az search service show --resource-group $defaultResourceGroup --name s2-search-dev | ConvertFrom-Json).name
$storageConnectionString = (az storage account show-connection-string --resource-group $defaultResourceGroup --name $storageAccountName --output tsv)

$databasePassword = "";
$databaseConnectionString = "Host=s2search-postgresql;Port=5432;Database=s2searchdb;Username=s2search;Password=$databasePassword";
$redisConnectionString = "s2search-redis-master:6379";

cd "E:\github\S2Search\K8s\Helm\Local"; 

Write-Color -Text "defaultResourceGroup - $defaultResourceGroup" -Color Blue
Write-Color -Text "storageAccountName - $storageAccountName" -Color Blue
Write-Color -Text "searchCredentialsQueryKey - $searchCredentialsQueryKey" -Color Blue
Write-Color -Text "searchCredentialsInstanceEndpoint - $searchCredentialsInstanceEndpoint" -Color Blue
Write-Color -Text "storageConnectionString - $storageConnectionString" -Color Blue

helm upgrade --install s2search . -n s2search `
    --set-string postgresql.auth.password=$databasePassword `
    --set-string postgresql.auth.connectionString="$databaseConnectionString" `
    --set-string ConnectionStrings.databaseConnectionString="$databaseConnectionString" `
    --set-string ConnectionStrings.azureStorageConnectionString=$storageConnectionString `
    --set-string ConnectionStrings.redisConnectionString=$redisConnectionString `
    --set-string Search.searchCredentialsQueryKey=$searchCredentialsQueryKey `
    --set-string Search.SearchCredentialsInstanceEndpoint=$searchCredentialsInstanceEndpoint `
    --set-string feedfunctions.azureStorage.connectionString=$storageConnectionString `
    --set-string searchinsights.azureStorage.connectionString=$storageConnectionString;
docker image prune -f;
