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

variable "storage_account_name" {
  type        = string
  description = "Name of the storage account"
  sensitive   = false
}

variable "account_tier" {
  type        = string
  description = "Tier of the storage account"
  sensitive   = false
}

variable "account_replication_type" {
  type        = string
  description = "Replication type of the storage account"
  sensitive   = false
}

variable "account_kind" {
  type        = string
  description = "Kind of the storage account"
  sensitive   = false
}

variable "access_tier" {
  type        = string
  description = "Access tier of the storage account"
  sensitive   = false
}

variable "min_tls_version" {
  type        = string
  description = "Minimum TLS version for the storage account"
  sensitive   = false
}
