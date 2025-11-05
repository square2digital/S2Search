# terraform globals
tags_environment = "terraform-managed"
tags_project     = "S2Search"
tags_service     = "storage"

# azure globals
resource_group_name = "s2 search"
location            = "northeurope"

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

# s2search k8s instance
k8s_cluster_name   = "s2search-k8s"
kubernetes_version = "1.34"

k8s_dns_prefix = "s2search-aks-dev"

k8s_node_count     = 1
k8s_node_size      = "Standard_B2s"
k8s_network_policy = "default"

k8s_min_count      = 1
k8s_max_count      = 2
k8s_node_pool_name = "s2nodepool"
