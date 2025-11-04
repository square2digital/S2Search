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

variable "search_service_name" {
  type        = string
  description = "Name of the Azure Search service"
  sensitive   = false
}

variable "search_service_sku" {
  type        = string
  description = "SKU for the Azure Search service"
  sensitive   = false
}

variable "search_service_replicas" {
  type        = number
  description = "Number of replicas for the Azure Search service"
  sensitive   = false
}

variable "search_service_partitions" {
  type        = number
  description = "Number of partitions for the Azure Search service"
  sensitive   = false
}
