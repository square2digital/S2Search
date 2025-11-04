provider "azurerm" {
  features {}

  client_id       = var.serviceprinciple_client_id
  client_secret   = var.serviceprinciple_client_secret
  tenant_id       = var.tenant_id
  subscription_id = var.subscription_id

  resource_provider_registrations = "none"
}

resource "azurerm_resource_group" "s2search_test" {
  name     = "s2search-terraform-test-rg"
  location = var.location
}


resource "azurerm_search_service" "s2search_instance" {
  name                = var.search_service_name
  resource_group_name = azurerm_resource_group.s2search_test.name
  location            = azurerm_resource_group.s2search_test.location
  sku                 = var.search_service_sku
  replica_count       = var.search_service_replicas
  partition_count     = var.search_service_partitions

  local_authentication_enabled = true
  authentication_failure_mode  = "http403"
}
