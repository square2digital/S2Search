# =================================================================
# Azure Search Service Outputs
# =================================================================

# Admin keys are not available through Terraform for security reasons
# Use the Azure CLI commands below to retrieve them after deployment
output "search_service_admin_key_command" {
  value       = "az search admin-key show --resource-group ${azurerm_resource_group.s2search_test.name} --service-name ${azurerm_search_service.s2search_instance.name}"
  description = "Azure CLI command to retrieve admin keys for the search service"
}

output "search_service_query_key_command" {
  value       = "az search query-key list --resource-group ${azurerm_resource_group.s2search_test.name} --service-name ${azurerm_search_service.s2search_instance.name}"
  description = "Azure CLI command to retrieve query keys for the search service"
}

output "search_service_id" {
  value       = azurerm_search_service.s2search_instance.id
  description = "The ID of the Azure Search service"
}

output "search_service_name" {
  value       = azurerm_search_service.s2search_instance.name
  description = "The name of the Azure Search service"
}

output "search_service_url" {
  value       = "https://${azurerm_search_service.s2search_instance.name}.search.windows.net"
  description = "The URL endpoint for the Azure Search service"
}

output "search_service_endpoint" {
  value       = azurerm_search_service.s2search_instance.name
  description = "The search service endpoint name (without https://)"
}

output "search_service_location" {
  value       = azurerm_search_service.s2search_instance.location
  description = "The location of the Azure Search service"
}

output "search_service_sku" {
  value       = azurerm_search_service.s2search_instance.sku
  description = "The SKU tier of the Azure Search service"
}

output "search_service_replica_count" {
  value       = azurerm_search_service.s2search_instance.replica_count
  description = "The number of replicas in the Azure Search service"
}

output "search_service_partition_count" {
  value       = azurerm_search_service.s2search_instance.partition_count
  description = "The number of partitions in the Azure Search service"
}

output "search_service_query_keys" {
  value       = "Use Azure CLI: az search query-key list --resource-group ${azurerm_resource_group.s2search_test.name} --service-name ${azurerm_search_service.s2search_instance.name}"
  description = "Command to retrieve query keys for the Azure Search service"
}

output "search_service_public_network_access_enabled" {
  value       = azurerm_search_service.s2search_instance.public_network_access_enabled
  description = "Whether public network access is enabled for the Azure Search service"
}

# =================================================================
# Composite Outputs for Application Configuration
# =================================================================

output "search_service_connection_info" {
  value = {
    service_name = azurerm_search_service.s2search_instance.name
    endpoint_url = "https://${azurerm_search_service.s2search_instance.name}.search.windows.net"
    location     = azurerm_search_service.s2search_instance.location
    sku          = azurerm_search_service.s2search_instance.sku
  }
  description = "Complete connection information for the Azure Search service"
}

# =================================================================
# Resource Group Outputs
# =================================================================

output "resource_group_name" {
  value       = azurerm_resource_group.s2search_test.name
  description = "The name of the resource group"
}

output "resource_group_id" {
  value       = azurerm_resource_group.s2search_test.id
  description = "The ID of the resource group"
}

output "resource_group_location" {
  value       = azurerm_resource_group.s2search_test.location
  description = "The location of the resource group"
}

# =================================================================
# Azure Storage Account Outputs
# =================================================================

output "storage_account_name" {
  value       = azurerm_storage_account.s2search_storage.name
  description = "The name of the storage account"
}

output "storage_account_id" {
  value       = azurerm_storage_account.s2search_storage.id
  description = "The ID of the storage account"
}

output "storage_account_primary_endpoint" {
  value       = azurerm_storage_account.s2search_storage.primary_blob_endpoint
  description = "The primary blob endpoint of the storage account"
}

output "storage_account_primary_key" {
  value       = azurerm_storage_account.s2search_storage.primary_access_key
  description = "The primary access key for the storage account"
  sensitive   = true
}

output "storage_account_connection_string" {
  value       = azurerm_storage_account.s2search_storage.primary_connection_string
  description = "The primary connection string for the storage account"
  sensitive   = true
}

# =================================================================
# Storage Container Outputs
# =================================================================

output "storage_container_assets_url" {
  value       = "${azurerm_storage_account.s2search_storage.primary_blob_endpoint}${azurerm_storage_container.s2search_blob.name}"
  description = "The URL of the assets container"
}

output "storage_container_feed_services_url" {
  value       = "${azurerm_storage_account.s2search_storage.primary_blob_endpoint}${azurerm_storage_container.s2search_feed_services.name}"
  description = "The URL of the feed-services container"
}

# =================================================================
# AKS Cluster Outputs
# =================================================================

output "aks_cluster_name" {
  value       = azurerm_kubernetes_cluster.s2search_aks.name
  description = "The name of the AKS cluster"
}

output "aks_cluster_id" {
  value       = azurerm_kubernetes_cluster.s2search_aks.id
  description = "The ID of the AKS cluster"
}

output "aks_cluster_fqdn" {
  value       = azurerm_kubernetes_cluster.s2search_aks.fqdn
  description = "The FQDN of the AKS cluster"
}

output "aks_cluster_endpoint" {
  value       = azurerm_kubernetes_cluster.s2search_aks.kube_config.0.host
  description = "The endpoint of the AKS cluster"
  sensitive   = true
}

output "aks_cluster_ca_certificate" {
  value       = azurerm_kubernetes_cluster.s2search_aks.kube_config.0.cluster_ca_certificate
  description = "The cluster CA certificate"
  sensitive   = true
}

output "aks_get_credentials_command" {
  value       = "az aks get-credentials --resource-group ${azurerm_resource_group.s2search_test.name} --name ${azurerm_kubernetes_cluster.s2search_aks.name}"
  description = "Azure CLI command to get AKS credentials"
}

# =================================================================
# Storage Queue Outputs
# =================================================================

output "storage_queue_names" {
  value = [
    azurerm_storage_queue.feed_processing.name,
    azurerm_storage_queue.search_indexing.name,
    azurerm_storage_queue.cache_invalidation.name
  ]
  description = "Names of all storage queues"
}
