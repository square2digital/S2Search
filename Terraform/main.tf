provider "azurerm" {
  features {}

  client_id       = "serviceprinciple.client_id"
  client_secret   = "serviceprinciple.client_secret"
  tenant_id       = "serviceprinciple.tenant_id"
  subscription_id = "serviceprinciple.subscription_id"

}
