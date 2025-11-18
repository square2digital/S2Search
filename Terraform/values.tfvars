# terraform globals
tags_environment = "terraform-managed"
tags_project     = "S2Search"
tags_service     = "storage"

# azure globals
#resource_group_name         = "s2 search"
location                    = "northeurope"
default_resource_group_name = "s2search-rg"
k8s_resource_group_name     = "s2search-aks-rg"

# s2search search instance
search_service_name       = "s2-search-dev"
search_service_sku        = "free"
search_service_replicas   = 1
search_service_partitions = 1

# s2search storage account
storage_account_name     = "s2storage"
account_tier             = "Standard"
account_replication_type = "LRS"
account_kind             = "StorageV2"
access_tier              = "Cold"
min_tls_version          = "TLS1_2"

# s2search aks instance
aks_cluster_name   = "s2search-aks-dev"
aks_dns_prefix     = "s2searchaksdev"
kubernetes_version = "1.34" # Fixed: 1.34 doesn't exist

aks_node_count     = 1
aks_node_size      = "Standard_B2s"
aks_network_policy = "azure" # Fixed: "default" -> "azure"

aks_min_count      = 1
aks_max_count      = 2
aks_node_pool_name = "s2nodepool"
