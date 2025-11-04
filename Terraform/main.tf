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
