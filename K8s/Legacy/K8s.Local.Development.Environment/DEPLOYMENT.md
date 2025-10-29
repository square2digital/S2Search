# S2Search - Local K8s Development Environment Deployment Guide

Step-by-step deployment instructions for the S2Search platform using the legacy Kubernetes configuration with PowerShell automation.

## 🎯 Quick Start Deployment

### Prerequisites Checklist

- [ ] **Kubernetes Cluster** - Docker Desktop with Kubernetes enabled
- [ ] **PowerShell 7+** - Cross-platform PowerShell
- [ ] **kubectl** - Kubernetes CLI configured and connected
- [ ] **Docker Desktop** - Running with sufficient resources (8GB RAM minimum)
- [ ] **Git** - Repository cloned locally
- [ ] **Network Access** - Ability to pull container images

### Verify Prerequisites

```powershell
# Verify Kubernetes cluster
kubectl cluster-info
kubectl get nodes

# Verify PowerShell version
$PSVersionTable.PSVersion

# Verify Docker
docker version

# Check available resources
kubectl top nodes
```

## 🚀 Full Platform Deployment

### 1. Navigate to Deployment Directory

```powershell
# Navigate to the deployment directory
cd "path\to\S2Search\K8s\Legacy\K8s.Local.Development.Environment"

# Verify you're in the correct directory
ls .\deployment-script.ps1
```

### 2. Complete Platform Deployment

```powershell
# Deploy entire S2Search platform
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
  -deleteS2Namespace $true `
  -deleteAllImages $false
```

### 3. Monitor Deployment Progress

```powershell
# Watch pods starting up
kubectl get pods -n s2 -w

# Check deployment status
kubectl get deployments -n s2

# Monitor services
kubectl get svc -n s2
```

### 4. Verify Deployment Success

```powershell
# Check all resources are running
kubectl get all -n s2

# Test application endpoints (script will auto-test)
# - Search UI: http://localhost:30001
# - Elastic UI: http://localhost:30002
# - Admin UI: http://localhost:30003
# - Search API: http://localhost:30004/swagger
# - Elastic API: http://localhost:30005/swagger
# - Admin API: http://localhost:30006/swagger
```

## 🎛️ Selective Component Deployment

### Frontend Only Deployment

Perfect for frontend development when you want to use external APIs:

```powershell
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $true `
  -includeAdminUI $true `
  -includeSearchAPI $false `
  -includeElasticAPI $false `
  -includeAdminAPI $false `
  -includeRedis $false `
  -includeSftpGo $false `
  -includeElastic $false `
  -deleteS2Namespace $false
```

### Backend Only Deployment

Ideal for API development and testing:

```powershell
.\deployment-script.ps1 `
  -includeElasticUI $false `
  -includeSearchUI $false `
  -includeAdminUI $false `
  -includeSearchAPI $true `
  -includeElasticAPI $true `
  -includeAdminAPI $true `
  -includeRedis $true `
  -includeSftpGo $true `
  -includeElastic $true `
  -deleteS2Namespace $false
```

### Search Engine Focus

For Elasticsearch-specific development:

```powershell
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $false `
  -includeAdminUI $false `
  -includeSearchAPI $false `
  -includeElasticAPI $true `
  -includeAdminAPI $false `
  -includeRedis $false `
  -includeSftpGo $false `
  -includeElastic $true `
  -deleteS2Namespace $false
```

### Single Service Development

Deploy only specific services for focused development:

```powershell
# Admin UI and API only
.\deployment-script.ps1 `
  -includeElasticUI $false `
  -includeSearchUI $false `
  -includeAdminUI $true `
  -includeSearchAPI $false `
  -includeElasticAPI $false `
  -includeAdminAPI $true `
  -includeRedis $false `
  -includeSftpGo $false `
  -includeElastic $false

# Search UI and API only
.\deployment-script.ps1 `
  -includeElasticUI $false `
  -includeSearchUI $true `
  -includeAdminUI $false `
  -includeSearchAPI $true `
  -includeElasticAPI $false `
  -includeAdminAPI $false `
  -includeRedis $true `
  -includeSftpGo $false `
  -includeElastic $false
```

## 🔄 Development Workflow Deployments

### Clean Slate Deployment

Start fresh with a completely clean environment:

```powershell
# Complete clean deployment
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
  -deleteS2Namespace $true `
  -deleteAllImages $true
```

### Quick Update Deployment

Fast deployment for code changes without rebuilding images:

```powershell
# Quick update without deleting images
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $true `
  -includeAdminUI $true `
  -includeSearchAPI $true `
  -includeElasticAPI $true `
  -includeAdminAPI $true `
  -includeRedis $false `
  -includeSftpGo $false `
  -includeElastic $false `
  -deleteS2Namespace $false `
  -deleteAllImages $false
```

### Debug-Focused Deployment

Deployment optimized for debugging specific issues:

```powershell
# Debug specific service with infrastructure
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $false `
  -includeAdminUI $false `
  -includeSearchAPI $false `
  -includeElasticAPI $true `
  -includeAdminAPI $false `
  -includeRedis $true `
  -includeSftpGo $false `
  -includeElastic $true `
  -deleteS2Namespace $false `
  -deleteAllImages $false
```

## 📊 Post-Deployment Verification

### Health Check Procedures

#### 1. Pod Status Verification

```powershell
# Check all pods are running
kubectl get pods -n s2

# Look for any failed or pending pods
kubectl get pods -n s2 | findstr -v "Running"

# Check pod events for issues
kubectl get events -n s2 --sort-by='.lastTimestamp'
```

#### 2. Service Accessibility

```powershell
# Check all services are accessible
kubectl get svc -n s2

# Test internal service connectivity
kubectl exec -it $(kubectl get pods -n s2 -l app=s2searchapi -o jsonpath='{.items[0].metadata.name}') -n s2 -- curl http://redis:6379

# Port forward for local testing
kubectl port-forward svc/s2searchui-service 3001:80 -n s2
kubectl port-forward svc/s2searchapi-service 5001:80 -n s2
```

#### 3. Application Health Checks

```powershell
# Test API health endpoints
curl http://localhost:30004/api/Status
curl http://localhost:30005/api/Status
curl http://localhost:30006/api/Status

# Test UI accessibility
Start-Process "http://localhost:30001"  # Search UI
Start-Process "http://localhost:30002"  # Elastic UI
Start-Process "http://localhost:30003"  # Admin UI
```

#### 4. Infrastructure Services

```powershell
# Check Elasticsearch health
curl http://localhost:9200/_cluster/health

# Check Redis connectivity
kubectl exec -it $(kubectl get pods -n s2 -l app=redis -o jsonpath='{.items[0].metadata.name}') -n s2 -- redis-cli ping

# Check Kibana accessibility
Start-Process "http://localhost:5601"
```

### Performance Verification

```powershell
# Check resource usage
kubectl top pods -n s2
kubectl top nodes

# Monitor deployment status
kubectl rollout status deployment/s2searchapi-deployment -n s2
kubectl rollout status deployment/s2searchui-deployment -n s2
kubectl rollout status deployment/s2adminui-deployment -n s2
```

## 🐛 Troubleshooting Deployment Issues

### Common Deployment Problems

#### 1. PowerShell Execution Policy

```powershell
# If script execution is blocked
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser

# Verify execution policy
Get-ExecutionPolicy -List
```

#### 2. Docker Image Pull Issues

```powershell
# Check Docker daemon status
docker system info

# Manually pull images if needed
docker pull redis:latest
docker pull elasticsearch:7.17.0
docker pull kibana:7.17.0

# Clean Docker cache if space issues
docker system prune -f
```

#### 3. Kubernetes Resource Issues

```powershell
# Check cluster resources
kubectl describe nodes

# Check available storage
kubectl get persistentvolumes
kubectl get persistentvolumeclaims -n s2

# Delete stuck resources
kubectl delete pod <pod-name> -n s2 --force --grace-period=0
```

#### 4. Port Conflicts

```powershell
# Check port usage
netstat -an | findstr :30001
netstat -an | findstr :30002
netstat -an | findstr :30003

# Kill processes using required ports
Stop-Process -Id (Get-NetTCPConnection -LocalPort 30001).OwningProcess -Force
```

#### 5. Service Connectivity Issues

```powershell
# Check service endpoints
kubectl get endpoints -n s2

# Debug DNS resolution
kubectl exec -it $(kubectl get pods -n s2 -l app=s2searchapi -o jsonpath='{.items[0].metadata.name}') -n s2 -- nslookup redis

# Check ConfigMaps
kubectl get configmap -n s2
kubectl describe configmap searchapi-appsettings-localk8s -n s2
```

### Debugging Commands

```powershell
# Get detailed pod information
kubectl describe pod <pod-name> -n s2

# Check pod logs
kubectl logs <pod-name> -n s2 -f

# Execute into pod for debugging
kubectl exec -it <pod-name> -n s2 -- /bin/bash

# Check deployment history
kubectl rollout history deployment/<deployment-name> -n s2

# Check resource quotas
kubectl describe resourcequota -n s2
```

## 🔄 Update and Maintenance Procedures

### Code Update Deployment

When you have new code changes:

```powershell
# 1. Build new images (if needed)
# This is handled automatically by the deployment script

# 2. Deploy with updated code
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $true `
  -includeAdminUI $true `
  -includeSearchAPI $true `
  -includeElasticAPI $true `
  -includeAdminAPI $true `
  -includeRedis $false `
  -includeSftpGo $false `
  -includeElastic $false `
  -deleteS2Namespace $false `
  -deleteAllImages $false

# 3. Verify updates
kubectl rollout status deployment/s2searchapi-deployment -n s2
kubectl rollout status deployment/s2searchui-deployment -n s2
```

### Configuration Updates

For configuration changes only:

```powershell
# 1. Update ConfigMaps
kubectl apply -f .\S2SearchAPI\ConfigMaps\configmap-localk8s.yml -n s2
kubectl apply -f .\S2ElasticAPI\ConfigMaps\configmap-localk8s.yml -n s2

# 2. Restart deployments to pick up changes
kubectl rollout restart deployment/s2searchapi-deployment -n s2
kubectl rollout restart deployment/s2elasticapi-deployment -n s2

# 3. Verify restart
kubectl rollout status deployment/s2searchapi-deployment -n s2
```

### Infrastructure Updates

For infrastructure-only updates:

```powershell
# Update Redis only
.\deployment-script.ps1 `
  -includeElasticUI $false `
  -includeSearchUI $false `
  -includeAdminUI $false `
  -includeSearchAPI $false `
  -includeElasticAPI $false `
  -includeAdminAPI $false `
  -includeRedis $true `
  -includeSftpGo $false `
  -includeElastic $false

# Update Elasticsearch only
.\deployment-script.ps1 `
  -includeElasticUI $false `
  -includeSearchUI $false `
  -includeAdminUI $false `
  -includeSearchAPI $false `
  -includeElasticAPI $false `
  -includeAdminAPI $false `
  -includeRedis $false `
  -includeSftpGo $false `
  -includeElastic $true
```

## 🧹 Cleanup Procedures

### Partial Cleanup

Remove specific components:

```powershell
# Remove UI components only
kubectl delete deployment s2searchui-deployment -n s2
kubectl delete deployment s2elasticui-deployment -n s2
kubectl delete deployment s2adminui-deployment -n s2

kubectl delete service s2searchui-service -n s2
kubectl delete service s2elasticui-service -n s2
kubectl delete service s2adminui-service -n s2
```

### Complete Environment Cleanup

```powershell
# Complete cleanup
.\deployment-script.ps1 `
  -includeElasticUI $false `
  -includeSearchUI $false `
  -includeAdminUI $false `
  -includeSearchAPI $false `
  -includeElasticAPI $false `
  -includeAdminAPI $false `
  -includeRedis $false `
  -includeSftpGo $false `
  -includeElastic $false `
  -deleteS2Namespace $true `
  -deleteAllImages $true

# Alternative manual cleanup
kubectl delete namespace s2

# Remove persistent volumes (if needed)
kubectl delete pv --all

# Clean Docker resources
docker system prune -a -f
```

## 📚 Advanced Deployment Scenarios

### Multi-Environment Deployment

Set up different configurations for different environments:

```powershell
# Development environment
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $true `
  -includeAdminUI $true `
  -includeSearchAPI $true `
  -includeElasticAPI $true `
  -includeAdminAPI $true `
  -includeRedis $true `
  -includeSftpGo $false `
  -includeElastic $true

# Testing environment (minimal resources)
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $false `
  -includeAdminUI $false `
  -includeSearchAPI $true `
  -includeElasticAPI $true `
  -includeAdminAPI $false `
  -includeRedis $true `
  -includeSftpGo $false `
  -includeElastic $true
```

### Performance Testing Deployment

```powershell
# Scale up for performance testing
kubectl scale deployment s2searchapi-deployment --replicas=3 -n s2
kubectl scale deployment s2elasticapi-deployment --replicas=2 -n s2

# Deploy with all components for load testing
.\deployment-script.ps1 `
  -includeElasticUI $true `
  -includeSearchUI $true `
  -includeAdminUI $true `
  -includeSearchAPI $true `
  -includeElasticAPI $true `
  -includeAdminAPI $true `
  -includeRedis $true `
  -includeSftpGo $true `
  -includeElastic $true

# Monitor performance
kubectl top pods -n s2
kubectl top nodes
```

## 📝 Deployment Checklist

### Pre-Deployment

- [ ] Kubernetes cluster is running and accessible
- [ ] Docker Desktop has sufficient resources allocated
- [ ] PowerShell execution policy allows script execution
- [ ] All required ports are available (30001-30006, 9200, 5601)
- [ ] Network connectivity for image pulls
- [ ] Sufficient disk space for images and persistent volumes

### During Deployment

- [ ] Monitor deployment script output for errors
- [ ] Verify pods are starting successfully
- [ ] Check for any image pull failures
- [ ] Monitor resource usage
- [ ] Verify ConfigMaps are applied correctly

### Post-Deployment

- [ ] All pods are in `Running` state
- [ ] All services are accessible
- [ ] Health check endpoints respond correctly
- [ ] UI applications load properly
- [ ] API documentation is accessible
- [ ] Elasticsearch and Redis are functioning
- [ ] Log aggregation is working

### Verification Tests

- [ ] Search functionality works end-to-end
- [ ] Admin operations are functional
- [ ] Data persistence works across pod restarts
- [ ] Inter-service communication is working
- [ ] Performance meets expectations

## 🆘 Emergency Procedures

### Rapid Recovery

If deployment fails completely:

```powershell
# 1. Quick cleanup
kubectl delete namespace s2 --force --grace-period=0

# 2. Clean Docker resources
docker system prune -f

# 3. Restart Docker Desktop

# 4. Redeploy with clean slate
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
  -deleteS2Namespace $true `
  -deleteAllImages $true
```

### Rollback Procedures

```powershell
# Rollback specific deployment
kubectl rollout undo deployment/s2searchapi-deployment -n s2

# Rollback to specific revision
kubectl rollout undo deployment/s2searchapi-deployment --to-revision=2 -n s2

# Check rollout history
kubectl rollout history deployment/s2searchapi-deployment -n s2
```

## 📞 Support and Resources

### Getting Help

- **GitHub Issues**: Report deployment problems
- **Documentation**: Refer to main README.md for architecture details
- **Logs**: Always include pod logs when reporting issues
- **Environment**: Include kubectl version, Docker version, and OS details

### Useful Commands Reference

```powershell
# Quick status check
kubectl get all -n s2

# Resource usage
kubectl top pods -n s2

# Events monitoring
kubectl get events -n s2 --sort-by='.lastTimestamp'

# Logs collection
kubectl logs -l app=s2searchapi -n s2 --tail=100

# Troubleshooting access
kubectl exec -it <pod-name> -n s2 -- /bin/bash
```

---

_For architectural details, see [README.md](README.md)_
_For advanced configuration, see individual service documentation_
