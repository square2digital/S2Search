terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.6"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.1"
    }
  }
  required_version = ">= 1.8"
}

provider "azurerm" {
  features {}

  client_id       = var.serviceprinciple_client_id
  client_secret   = var.serviceprinciple_client_secret
  tenant_id       = var.tenant_id
  subscription_id = var.subscription_id
}

resource "azurerm_resource_group" "s2search_test" {
  name     = "s2search-terraform-test-rg"
  location = var.location

  tags = {
    Environment = "terraform-managed"
    Project     = "S2Search"
    CreatedBy   = "terraform"
  }
}

# Generate a random suffix for globally unique storage account name
resource "random_string" "storage_suffix" {
  length  = 3
  special = false
  upper   = false
  lower   = false
  numeric = true
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

  # Security and networking
  public_network_access_enabled = true

  tags = {
    Environment = "terraform-managed"
    Project     = "S2Search"
    Service     = "search"
  }
}

resource "azurerm_storage_account" "s2search_storage" {
  name                     = "${var.storage_account_name}${random_string.storage_suffix.result}"
  resource_group_name      = azurerm_resource_group.s2search_test.name
  location                 = azurerm_resource_group.s2search_test.location
  account_tier             = var.account_tier
  account_replication_type = var.account_replication_type
  account_kind             = var.account_kind
  access_tier              = var.access_tier
  min_tls_version          = var.min_tls_version

  # Security best practices
  allow_nested_items_to_be_public = false
  shared_access_key_enabled       = true
  public_network_access_enabled   = true

  # Enable blob versioning and change feed for better data management
  blob_properties {
    versioning_enabled            = true
    change_feed_enabled           = true
    change_feed_retention_in_days = 7

    delete_retention_policy {
      days = 7
    }

    container_delete_retention_policy {
      days = 7
    }
  }

  tags = {
    Environment = "terraform-managed"
    Project     = "S2Search"
    Service     = "storage"
  }
}

resource "azurerm_storage_container" "s2search_blob" {
  name                  = "assets"
  storage_account_name  = azurerm_storage_account.s2search_storage.name
  container_access_type = "private"
}

resource "azurerm_storage_container" "s2search_feed_services" {
  name                  = "feed-services"
  storage_account_name  = azurerm_storage_account.s2search_storage.name
  container_access_type = "private"
}

# Storage Queues for async processing
resource "azurerm_storage_queue" "feed_processing" {
  name                 = "feed-process"
  storage_account_name = azurerm_storage_account.s2search_storage.name
}

resource "azurerm_storage_queue" "search_indexing" {
  name                 = "feed-validate"
  storage_account_name = azurerm_storage_account.s2search_storage.name
}

resource "azurerm_storage_queue" "cache_invalidation" {
  name                 = "feed-extract"
  storage_account_name = azurerm_storage_account.s2search_storage.name
}
