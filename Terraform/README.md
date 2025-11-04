# S2Search Terraform Infrastructure

This directory contains the Terraform configuration for provisioning S2Search Azure infrastructure.

## 📁 File Structure

```
Terraform/
├── main.tf          # Provider configuration and main resources
├── variables.tf     # Variable declarations
├── outputs.tf       # Output definitions
├── secrets.tfvars   # Variable values (sensitive, gitignored)
└── README.md        # This file
```

## 🚀 Usage

### Initialize Terraform

```bash
terraform init
```

### Plan Infrastructure Changes

```bash
terraform plan -var-file="secrets.tfvars"
```

### Apply Infrastructure

```bash
terraform apply -var-file="secrets.tfvars"
```

### Get Outputs

```bash
# View all outputs
terraform output

# Get specific output
terraform output search_service_url

# Get sensitive output
terraform output -raw search_service_primary_key
```

## 📋 Resources Created

- **Resource Group**: `s2search-terraform-test-rg`
- **Azure Search Service**: Configurable search instance
- **Outputs**: Connection strings, keys, and service information

## 🔐 Security

- All sensitive values are marked as `sensitive = true`
- Service principal credentials stored in `secrets.tfvars` (gitignored)
- Admin keys and query keys are output securely

## 📝 Variables

| Variable                         | Description                 | Required                  |
| -------------------------------- | --------------------------- | ------------------------- |
| `location`                       | Azure region                | No (default: northeurope) |
| `search_service_name`            | Name for the search service | Yes                       |
| `search_service_sku`             | SKU tier for search service | Yes                       |
| `search_service_replicas`        | Number of replicas          | Yes                       |
| `search_service_partitions`      | Number of partitions        | Yes                       |
| `serviceprinciple_client_id`     | Service principal client ID | Yes                       |
| `serviceprinciple_client_secret` | Service principal secret    | Yes                       |
| `tenant_id`                      | Azure tenant ID             | Yes                       |
| `subscription_id`                | Azure subscription ID       | Yes                       |
