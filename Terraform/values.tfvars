# azure globals
location = "northeurope"

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
