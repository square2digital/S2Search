# S2Search - Local Kubernetes Development Environment

A comprehensive Kubernetes deployment solution for local development of the S2Search platform. This environment provides complete orchestration for all S2Search microservices, databases, and supporting infrastructure.

## 🚀 Overview

This Kubernetes setup enables developers to run the entire S2Search ecosystem locally, including:

- **Frontend Applications** - React/Next.js UIs for search and administration
- **Backend APIs** - .NET Core microservices for search, admin, and elastic operations
- **Search Engines** - Elasticsearch cluster with Kibana dashboard
- **Caching Layer** - Redis cluster with high availability
- **File Transfer** - SFTPGo secure file transfer service
- **Infrastructure** - Ingress controllers, cert management, and storage

## 📁 Project Structure

```
K8s.Local.Development.Environment/
├── deployment-script.ps1           # 🎯 Main deployment orchestration script
├── DevOps/
│   └── templates/                  # Deployment templates and configs
├── Kubernetes/                     # Core K8s infrastructure
│   ├── cert-manager/              # SSL certificate management
│   ├── container-registry/        # Local container registry
│   ├── ingress-controller/        # Traffic routing and load balancing
│   └── storage/                   # Persistent volume configurations
├── ElasticSearch/                 # Elasticsearch cluster setup
│   ├── elastic-statefulset.yml   # Elasticsearch nodes
│   ├── elastic-service.yml       # Service definitions
│   ├── kibana-deployment.yml     # Kibana dashboard
│   └── *.yml                     # Load balancers and services
├── redis/                        # Redis cluster configuration
│   ├── redis-statefulset.yaml   # Redis nodes with persistence
│   ├── redis-configmap.yaml     # Redis configuration
│   ├── sentinel/                 # Redis Sentinel for HA
│   └── SingleInstance/           # Single instance setup
├── S2SearchAPI/                  # Main Search API service
│   ├── deployment.yml           # API deployment configuration
│   ├── ConfigMaps/              # Application configuration
│   └── service-*.yml            # Service definitions (ClusterIP, LoadBalancer, NodePort)
├── S2ElasticAPI/                # Elasticsearch API service
├── S2AdminAPI/                  # Administration API service
├── S2SearchUI/                  # Search UI frontend
├── S2ElasticUI/                 # Elasticsearch UI frontend
├── S2AdminUI/                   # Admin UI frontend
├── S2ClientConfigurationApi/    # Client configuration service
├── S2CustomerResourceApi/       # Customer resource management
├── SFTPGo/                      # Secure file transfer service
├── MySql/                       # MySQL database setup
└── K8s-Dashboard/               # Kubernetes dashboard
```

## 🛠️ Prerequisites

### Required Software

- **Kubernetes Cluster** - Docker Desktop with Kubernetes enabled, Minikube, or Kind
- **kubectl** - Kubernetes command-line tool
- **PowerShell 7+** - Cross-platform PowerShell for deployment scripts
- **Docker Desktop** - Container runtime and local registry
- **.NET 8 SDK** - For building .NET microservices
- **Node.js 18+** - For building React/Next.js frontends

### System Resources

**Minimum Requirements:**

- **CPU**: 4 cores
- **RAM**: 8GB
- **Storage**: 20GB free space

**Recommended:**

- **CPU**: 8 cores
- **RAM**: 16GB
- **Storage**: 50GB free space

## ⚡ Quick Start

### 1. Environment Setup

```powershell
# Enable Kubernetes in Docker Desktop
# or start Minikube
minikube start --memory=8192 --cpus=4

# Verify cluster is running
kubectl cluster-info
kubectl get nodes
```

### 2. Full Platform Deployment

```powershell
# Navigate to deployment directory
cd "path\to\S2Search\K8s\Legacy\K8s.Local.Development.Environment"

# Deploy complete platform (recommended for first-time setup)
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $true `
  -includeAdminUI $true `
  -includeSearchAPI $true `
  -includeElasticAPI $true `
  -includeAdminAPI $true `
  -includeRedis $true `
  -includeSftpGo $true `
  -includeElastic $true `
  -deleteS2Namespace $true
```

### 3. Access Applications

After successful deployment, access the applications at:

| Service           | URL                            | Description                      |
| ----------------- | ------------------------------ | -------------------------------- |
| **Search UI**     | http://localhost:30001         | Main vehicle search interface    |
| **Elastic UI**    | http://localhost:30002         | Elasticsearch-powered search UI  |
| **Admin UI**      | http://localhost:30003         | Administrative dashboard         |
| **Search API**    | http://localhost:30004/swagger | Main search API documentation    |
| **Elastic API**   | http://localhost:30005/swagger | Elasticsearch API documentation  |
| **Admin API**     | http://localhost:30006/swagger | Administration API documentation |
| **Kibana**        | http://localhost:5601          | Elasticsearch data visualization |
| **K8s Dashboard** | http://localhost:30007         | Kubernetes cluster dashboard     |

## 🎯 Deployment Options

### Selective Component Deployment

Deploy only specific components for focused development:

```powershell
# Frontend Development (UIs only)
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $true `
  -includeAdminUI $true `
  -includeSearchAPI $false `
  -includeElasticAPI $false `
  -includeAdminAPI $false `
  -includeRedis $false `
  -includeSftpGo $false `
  -includeElastic $false

# Backend Development (APIs + Infrastructure)
.\deployment-script.ps1 `
  -includeElasticUI $false `
  -includeSearchUI $false `
  -includeAdminUI $false `
  -includeSearchAPI $true `
  -includeElasticAPI $true `
  -includeAdminAPI $true `
  -includeRedis $true `
  -includeSftpGo $true `
  -includeElastic $true

# Search Engine Focus (Elasticsearch + Related Services)
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchAPI $false `
  -includeElasticAPI $true `
  -includeAdminAPI $false `
  -includeRedis $false `
  -includeSftpGo $false `
  -includeElastic $true
```

### Development Workflow

```powershell
# Clean slate deployment (removes existing namespace)
.\deployment-script.ps1 -deleteS2Namespace $true -deleteAllImages $true

# Quick rebuild without image deletion
.\deployment-script.ps1 -deleteS2Namespace $false -deleteAllImages $false

# Debugging specific service
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeElasticAPI $true `
  -includeElastic $true `
  -deleteS2Namespace $false
```

## 🏗️ Architecture Components

### Frontend Services

#### S2SearchUI

- **Technology**: React/TypeScript with Azure Cognitive Services
- **Port**: 30001
- **Purpose**: Main vehicle search interface with advanced filtering

#### S2ElasticUI

- **Technology**: React/TypeScript with Elasticsearch integration
- **Port**: 30002
- **Purpose**: Elasticsearch-powered search interface

#### S2AdminUI

- **Technology**: React/TypeScript administrative interface
- **Port**: 30003
- **Purpose**: Platform administration and configuration

### Backend APIs

#### S2SearchAPI

- **Technology**: .NET 8 Web API
- **Port**: 30004
- **Purpose**: Main search orchestration and business logic
- **Health Check**: `/api/Status`

#### S2ElasticAPI

- **Technology**: .NET 8 Web API with Elasticsearch client
- **Port**: 30005
- **Purpose**: Direct Elasticsearch operations and indexing
- **Health Check**: `/api/Status`

#### S2AdminAPI

- **Technology**: .NET 8 Web API
- **Port**: 30006
- **Purpose**: Administrative operations and configuration management
- **Health Check**: `/api/Status`

### Infrastructure Services

#### Elasticsearch Cluster

- **Configuration**: StatefulSet with persistent volumes
- **Nodes**: 3-node cluster for high availability
- **Kibana**: Data visualization and cluster management
- **Ports**: 9200 (Elasticsearch), 5601 (Kibana)

#### Redis Cluster

- **Configuration**: StatefulSet with Redis Sentinel
- **Setup**: 3 Redis nodes + 3 Sentinel instances
- **Purpose**: Caching, session management, and pub/sub
- **High Availability**: Automatic failover with Sentinel

#### SFTPGo Service

- **Purpose**: Secure file transfer for data feeds
- **Features**: Web admin interface, user management
- **Integration**: Feed processing workflows

## 🔧 Configuration Management

### ConfigMaps

Each service uses Kubernetes ConfigMaps for environment-specific configuration:

```yaml
# Example: S2SearchAPI ConfigMap
apiVersion: v1
kind: ConfigMap
metadata:
  name: searchapi-appsettings-localk8s
data:
  appsettings.json: |
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=mysql;Database=S2Search;...",
        "Redis": "redis:6379"
      },
      "ElasticsearchSettings": {
        "Url": "http://elasticsearch:9200",
        "DefaultIndex": "vehicles"
      }
    }
```

### Environment Variables

Services are configured with appropriate environment variables:

- `ASPNETCORE_ENVIRONMENT=Production`
- `ASPNETCORE_URLS=http://+:80`
- Custom application settings via ConfigMaps

## 📊 Monitoring & Health Checks

### Health Checks

All APIs implement health check endpoints:

```yaml
livenessProbe:
  httpGet:
    path: /api/Status
    port: 80
  initialDelaySeconds: 60
  periodSeconds: 180

readinessProbe:
  httpGet:
    path: /api/Status
    port: 80
  initialDelaySeconds: 60
  periodSeconds: 180
```

### Resource Limits

Services are configured with appropriate resource limits:

```yaml
resources:
  requests:
    memory: 210Mi
    cpu: 10m
  limits:
    memory: 225Mi
    cpu: 200m
```

### Monitoring Tools

- **Kubernetes Dashboard** - Cluster overview and resource monitoring
- **Kibana** - Elasticsearch cluster health and data visualization
- **kubectl** commands for real-time monitoring

## 🐛 Troubleshooting

### Common Issues

#### Pods Not Starting

```powershell
# Check pod status
kubectl get pods -n s2

# Describe problematic pod
kubectl describe pod <pod-name> -n s2

# Check logs
kubectl logs <pod-name> -n s2
```

#### Service Connectivity Issues

```powershell
# Check services
kubectl get svc -n s2

# Test internal connectivity
kubectl exec -it <pod-name> -n s2 -- curl http://service-name:port/health
```

#### Persistent Volume Issues

```powershell
# Check persistent volumes
kubectl get pv
kubectl get pvc -n s2

# Check storage class
kubectl get storageclass
```

#### Image Pull Issues

```powershell
# Rebuild images
.\deployment-script.ps1 -deleteAllImages $true

# Check Docker images
docker images | grep s2
```

### Debugging Commands

```powershell
# Get all resources in s2 namespace
kubectl get all -n s2

# Check cluster events
kubectl get events -n s2 --sort-by='.lastTimestamp'

# Check node resources
kubectl top nodes
kubectl top pods -n s2

# Access pod shell for debugging
kubectl exec -it <pod-name> -n s2 -- /bin/bash
```

### Log Analysis

```powershell
# Follow logs in real-time
kubectl logs -f <pod-name> -n s2

# Get logs from all replicas
kubectl logs -l app=s2searchapi -n s2

# Export logs for analysis
kubectl logs <pod-name> -n s2 > debug.log
```

## 🔄 Development Workflow

### Code Change Deployment

1. **Make code changes** in your development environment
2. **Rebuild specific service**:
   ```powershell
   .\deployment-script.ps1 -includeSearchAPI $true -deleteAllImages $false
   ```
3. **Test changes** via service endpoints
4. **Check logs** for any issues

### Database Changes

1. **Update database scripts** in [`DB/`](../../../DB) directory
2. **Apply migrations** via admin tools or direct SQL execution
3. **Restart dependent services** if schema changes affect APIs

### Configuration Updates

1. **Modify ConfigMaps** in service directories
2. **Reapply configurations**:
   ```powershell
   kubectl apply -f ConfigMaps/configmap-localk8s.yml -n s2
   ```
3. **Restart pods** to pick up new configuration:
   ```powershell
   kubectl rollout restart deployment/<deployment-name> -n s2
   ```

## 🚀 Performance Optimization

### Resource Tuning

- **Monitor resource usage** with `kubectl top`
- **Adjust CPU/memory limits** based on actual usage
- **Scale services** horizontally for load testing

### Storage Optimization

- **Use appropriate StorageClass** for your environment
- **Configure persistent volume sizes** based on data requirements
- **Implement backup strategies** for persistent data

### Network Optimization

- **Use ClusterIP** for internal service communication
- **Configure ingress** for external access in production-like setups
- **Implement service mesh** for advanced traffic management

## 📚 Additional Resources

### Kubernetes Commands Reference

```powershell
# Namespace management
kubectl create namespace s2
kubectl delete namespace s2

# Deployment management
kubectl apply -f deployment.yml -n s2
kubectl delete deployment <name> -n s2
kubectl rollout restart deployment/<name> -n s2

# Service management
kubectl apply -f service.yml -n s2
kubectl get svc -n s2
kubectl describe svc <name> -n s2

# Configuration management
kubectl apply -f configmap.yml -n s2
kubectl get configmap -n s2
kubectl describe configmap <name> -n s2
```

### Useful Monitoring Commands

```powershell
# Real-time cluster monitoring
watch kubectl get pods -n s2

# Resource usage monitoring
kubectl top nodes
kubectl top pods -n s2

# Event monitoring
kubectl get events -w -n s2
```

## 🤝 Contributing

1. **Test changes locally** using this K8s environment
2. **Update deployment scripts** if adding new services
3. **Document configuration changes** in service-specific READMEs
4. **Verify health checks** work correctly
5. **Test scaling scenarios** where applicable

## 📄 License

This project is proprietary software. See [LICENSE](../../../LICENSE) for details.

## 🔗 Related Documentation

- [Main S2Search Documentation](../../../README.md)
- [Backend APIs](../../../APIs/README.md)
- [Frontend Applications](../../../UIs/README.md)
- [Redis Configuration](./redis/readme.md)

---

_Built for enterprise-scale local development with Kubernetes and Docker_
