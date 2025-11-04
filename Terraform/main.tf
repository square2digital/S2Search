provider "azurerm" {
  features {}

  client_id       = var.serviceprinciple_client_id
  client_secret   = var.serviceprinciple_client_secret
  tenant_id       = var.tenant_id
  subscription_id = var.subscription_id

}

variable "serviceprinciple_client_id" {
  type        = string
  description = "Client ID for the service principal"
  sensitive   = true
}
variable "serviceprinciple_client_secret" {
  type        = string
  description = "Client secret for the service principal"
  sensitive   = true
}
variable "tenant_id" {
  type        = string
  description = "Azure tenant ID"
  sensitive   = true
}
variable "subscription_id" {
  type        = string
  description = "Azure subscription ID"
  sensitive   = true
}
