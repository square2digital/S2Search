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
