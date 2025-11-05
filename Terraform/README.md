# S2Search Terraform Infrastructure

This directory contains the Terraform configuration for provisioning S2Search Azure infrastructure including Search Services, Storage Accounts, and AKS cluster.

## 📁 File Structure

```
Terraform/
├── main.tf          # Provider configuration and main resources
├── variables.tf     # Variable declarations
├── outputs.tf       # Output definitions
├── values.tfvars    # Configuration values (non-sensitive)
├── secrets.tfvars   # Sensitive credentials (gitignored)
├── .terraform.lock.hcl  # Provider version lock file
└── README.md        # This file
```

## 🚀 Quick Start

### Prerequisites

- [Terraform](https://terraform.io) >= 1.8
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli)
- Valid Azure subscription and service principal

### Initialize Terraform

```powershell
terraform init
```

### Plan Infrastructure Changes

```powershell
terraform plan -var-file="values.tfvars" -var-file="secrets.tfvars"
```

### Apply Infrastructure

```powershell
terraform apply -var-file="values.tfvars" -var-file="secrets.tfvars"
```

### Destroy Infrastructure

```powershell
terraform destroy -var-file="values.tfvars" -var-file="secrets.tfvars"
```

## 📋 Resources Created

### Core Infrastructure

- **Resource Group**: `s2search-terraform-test-rg`
- **Azure Search Service**: Configurable cognitive search instance
- **Storage Account**: Blob storage with containers and queues
- **AKS Cluster**: Kubernetes cluster for container orchestration

### Storage Components

- **Blob Containers**:
  - `assets` - Static assets and files
  - `feed-services` - Feed processing data
- **Storage Queues**:
  - `feed-process` - Feed processing queue
  - `feed-validate` - Feed validation queue
  - `feed-extract` - Feed extraction queue

### Kubernetes Cluster

- **Node Pool**: Auto-scaling enabled (1-2 nodes)
- **VM Size**: Standard_B2s (configurable)
- **Network**: Azure CNI with network policies
- **Security**: RBAC enabled, Azure Policy integration

## 🔐 Security Features

- **Managed Identity**: System-assigned identity for AKS
- **Network Security**: Private endpoints and network policies
- **RBAC**: Role-based access control enabled
- **TLS**: Minimum TLS 1.2 for storage accounts
- **Blob Security**: Versioning and change feed enabled

## 📝 Configuration Files

### values.tfvars

Contains non-sensitive configuration values:

```terraform
# Global settings
tags_environment = "terraform-managed"
tags_project     = "S2Search"
location         = "northeurope"

# Search service configuration
search_service_name = "s2-search-dev"
search_service_sku  = "free"

# Storage configuration
storage_account_name = "s2storage"
account_tier         = "Standard"

# AKS configuration
k8s_cluster_name     = "s2search-k8s"
kubernetes_version   = "1.28.0"
k8s_node_count       = 1
```

### secrets.tfvars

Contains sensitive credentials (gitignored):

```terraform
serviceprinciple_client_id     = "your-client-id"
serviceprinciple_client_secret = "your-client-secret"
tenant_id                      = "your-tenant-id"
subscription_id                = "your-subscription-id"
```

## 📊 Outputs

Get infrastructure information after deployment:

```powershell
# View all outputs
terraform output

# Get specific outputs
terraform output search_service_url
terraform output storage_account_name
terraform output aks_cluster_name

# Get sensitive outputs
terraform output -raw search_service_primary_key
terraform output -raw storage_account_primary_key
```

## 🔧 Variables Reference

| Variable                         | Type   | Description                 | Default    | Required |
| -------------------------------- | ------ | --------------------------- | ---------- | -------- |
| **Global Variables**             |        |                             |            |          |
| `tags_environment`               | string | Environment tag             | -          | Yes      |
| `tags_project`                   | string | Project tag                 | -          | Yes      |
| `location`                       | string | Azure region                | -          | Yes      |
| **Search Service**               |        |                             |            |          |
| `search_service_name`            | string | Search service name         | -          | Yes      |
| `search_service_sku`             | string | Search service SKU          | -          | Yes      |
| `search_service_replicas`        | number | Number of replicas          | -          | Yes      |
| `search_service_partitions`      | number | Number of partitions        | -          | Yes      |
| **Storage Account**              |        |                             |            |          |
| `storage_account_name`           | string | Storage account base name   | `s2search` | No       |
| `account_tier`                   | string | Storage tier                | -          | Yes      |
| `account_replication_type`       | string | Replication type            | -          | Yes      |
| `min_tls_version`                | string | Minimum TLS version         | -          | Yes      |
| **AKS Cluster**                  |        |                             |            |          |
| `k8s_cluster_name`               | string | AKS cluster name            | -          | Yes      |
| `aks_dns_prefix`                 | string | DNS prefix for AKS          | -          | Yes      |
| `kubernetes_version`             | string | Kubernetes version          | `1.28`     | No       |
| `k8s_node_count`                 | number | Initial node count          | -          | Yes      |
| `k8s_node_size`                  | string | VM size for nodes           | -          | Yes      |
| `aks_min_count`                  | number | Min nodes for auto-scaling  | -          | Yes      |
| `aks_max_count`                  | number | Max nodes for auto-scaling  | -          | Yes      |
| **Authentication**               |        |                             |            |          |
| `serviceprinciple_client_id`     | string | Service principal client ID | -          | Yes      |
| `serviceprinciple_client_secret` | string | Service principal secret    | -          | Yes      |
| `tenant_id`                      | string | Azure tenant ID             | -          | Yes      |
| `subscription_id`                | string | Azure subscription ID       | -          | Yes      |

## 🚨 Common Issues

### Kubernetes Version Error

If you get a Kubernetes version error, update your `values.tfvars`:

```terraform
kubernetes_version = "1.28.0"  # Use a valid K8s version
```

### Storage Account Name Conflicts

Storage account names must be globally unique. The configuration automatically appends a random 3-digit suffix to prevent conflicts.

### Service Principal Permissions

Ensure your service principal has the following permissions:

- `Contributor` role on the subscription
- `User Access Administrator` for AKS managed identity

## 📚 Additional Resources

- [Azure Search Service Documentation](https://docs.microsoft.com/azure/search/)
- [Azure Kubernetes Service Documentation](https://docs.microsoft.com/azure/aks/)
- [Azure Storage Documentation](https://docs.microsoft.com/azure/storage/)
- [Terraform Azure Provider](https://registry.terraform.io/providers/hashicorp/azurerm/latest)
