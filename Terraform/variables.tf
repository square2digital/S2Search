variable "location" {
  type        = string
  description = "Azure region for resource deployment"
  sensitive   = false
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
