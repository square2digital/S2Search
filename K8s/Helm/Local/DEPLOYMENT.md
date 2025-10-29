# S2Search - Helm Chart Deployment Guide

Comprehensive deployment instructions for the S2Search platform using Helm charts for production-ready Kubernetes deployments.

## 🎯 Quick Start Deployment

### Prerequisites Checklist

- [ ] **Kubernetes Cluster** - v1.19+ (Local, Cloud, or Enterprise)
- [ ] **Helm** - v3.8+ installed and configured
- [ ] **kubectl** - Configured with cluster access
- [ ] **Container Registry Access** - GitHub Container Registry (GHCR) credentials
- [ ] **Storage Class** - Available storage class for persistent volumes
- [ ] **Load Balancer** - Available for external service exposure (cloud environments)

### Verify Prerequisites

```bash
# Verify Helm installation
helm version

# Verify Kubernetes cluster
kubectl cluster-info
kubectl get nodes

# Verify storage classes
kubectl get storageclass

# Check RBAC permissions
kubectl auth can-i create deployments --namespace=s2search
kubectl auth can-i create services --namespace=s2search
```

## 🚀 Basic Deployment

### 1. Prepare Environment

```bash
# Create namespace
kubectl create namespace s2search

# Create GitHub Container Registry secret
kubectl create secret docker-registry ghcr-secret \
  --docker-server=ghcr.io \
  --docker-username=<your-github-username> \
  --docker-password=<your-github-token> \
  --docker-email=<your-email> \
  --namespace=s2search
```

### 2. Basic Helm Installation

```bash
# Navigate to Helm chart directory
cd S2Search/K8s/Helm/Local

# Install with default values
helm install s2search . --namespace s2search

# Install and wait for readiness
helm install s2search . --namespace s2search --wait --timeout=10m
```

### 3. Verify Basic Deployment

```bash
# Check Helm release status
helm status s2search --namespace s2search

# Verify pods are running
kubectl get pods --namespace s2search

# Check services
kubectl get services --namespace s2search

# Monitor deployment progress
kubectl get events --namespace s2search --sort-by='.lastTimestamp'
```

### 4. Access Applications

```bash
# Port forward for local access
kubectl port-forward service/s2searchui-service 3000:80 --namespace s2search
kubectl port-forward service/search-api-service 8080:80 --namespace s2search

# Access applications
# Search UI: http://localhost:3000
# Backend API: http://localhost:8080/swagger
```

## 🎛️ Advanced Deployment Configurations

### Development Environment

Create `development-values.yaml`:

```yaml
# development-values.yaml
S2Backend:
  replicaCount: 1
  image:
    tag: "dev"
    pullPolicy: Always
  resources:
    limits:
      cpu: 1000m
      memory: 1Gi
    requests:
      cpu: 500m
      memory: 512Mi
  env:
    ASPNETCORE_ENVIRONMENT: "Development"

S2SearchUi:
  replicaCount: 1
  image:
    tag: "dev"
    pullPolicy: Always
  service:
    type: NodePort
  resources:
    limits:
      cpu: 500m
      memory: 512Mi
    requests:
      cpu: 250m
      memory: 256Mi
```

Deploy development environment:

```bash
helm install s2search-dev . \
  --namespace s2search \
  --values development-values.yaml \
  --set S2Backend.image.tag=latest \
  --set S2SearchUi.image.tag=latest
```

### Production Environment

Create `production-values.yaml`:

```yaml
# production-values.yaml
S2Backend:
  replicaCount: 3
  image:
    tag: "v1.2.0"
    pullPolicy: IfNotPresent
  resources:
    limits:
      cpu: 2000m
      memory: 4Gi
    requests:
      cpu: 1000m
      memory: 2Gi
  env:
    ASPNETCORE_ENVIRONMENT: "Production"
  nodeSelector:
    kubernetes.io/arch: amd64
  tolerations:
    - key: "node-type"
      operator: "Equal"
      value: "compute"
      effect: "NoSchedule"

S2SearchUi:
  replicaCount: 2
  image:
    tag: "v1.2.0"
    pullPolicy: IfNotPresent
  service:
    type: LoadBalancer
  resources:
    limits:
      cpu: 1000m
      memory: 1Gi
    requests:
      cpu: 500m
      memory: 512Mi
  nodeSelector:
    kubernetes.io/arch: amd64

# Enable ingress for production
ingress:
  enabled: true
  annotations:
    kubernetes.io/ingress.class: "nginx"
    cert-manager.io/cluster-issuer: "letsencrypt-prod"
  hosts:
    - host: search.yourdomain.com
      paths:
        - path: /
          pathType: Prefix
  tls:
    - secretName: search-tls
      hosts:
        - search.yourdomain.com
```

Deploy production environment:

```bash
helm install s2search-prod . \
  --namespace s2search \
  --values production-values.yaml
```

### Staging Environment

Create `staging-values.yaml`:

```yaml
# staging-values.yaml
S2Backend:
  replicaCount: 2
  image:
    tag: "staging"
  resources:
    limits:
      cpu: 1500m
      memory: 2Gi
    requests:
      cpu: 750m
      memory: 1Gi
  env:
    ASPNETCORE_ENVIRONMENT: "Staging"

S2SearchUi:
  replicaCount: 2
  image:
    tag: "staging"
  service:
    type: ClusterIP
  resources:
    limits:
      cpu: 750m
      memory: 768Mi
    requests:
      cpu: 375m
      memory: 384Mi
```

Deploy staging environment:

```bash
helm install s2search-staging . \
  --namespace s2search-staging \
  --values staging-values.yaml \
  --create-namespace
```

## 🔧 Configuration Management

### Environment-Specific Deployments

#### Local Development

```bash
# Local development with NodePort services
helm install s2search . \
  --namespace s2search \
  --set S2SearchUi.service.type=NodePort \
  --set S2Backend.service.type=NodePort \
  --set S2Backend.env.ASPNETCORE_ENVIRONMENT=Development \
  --set S2Backend.image.pullPolicy=Always \
  --set S2SearchUi.image.pullPolicy=Always
```

#### Cloud Development

```bash
# Cloud development with LoadBalancer
helm install s2search . \
  --namespace s2search \
  --set S2SearchUi.service.type=LoadBalancer \
  --set S2Backend.service.type=LoadBalancer \
  --set S2Backend.resources.limits.cpu=1000m \
  --set S2Backend.resources.limits.memory=1Gi
```

#### Testing Environment

```bash
# Minimal resources for testing
helm install s2search-test . \
  --namespace s2search-test \
  --create-namespace \
  --set S2Backend.replicaCount=1 \
  --set S2SearchUi.replicaCount=1 \
  --set S2Backend.resources.limits.cpu=500m \
  --set S2Backend.resources.limits.memory=512Mi \
  --set S2SearchUi.resources.limits.cpu=300m \
  --set S2SearchUi.resources.limits.memory=256Mi
```

### Custom Image Tags

```bash
# Deploy specific image versions
helm install s2search . \
  --namespace s2search \
  --set S2Backend.image.tag=feature-branch-123 \
  --set S2SearchUi.image.tag=feature-branch-123

# Deploy from different registry
helm install s2search . \
  --namespace s2search \
  --set S2Backend.image.repository=myregistry.azurecr.io/s2search-backend \
  --set S2SearchUi.image.repository=myregistry.azurecr.io/s2search-ui \
  --set S2Backend.image.tag=v2.0.0 \
  --set S2SearchUi.image.tag=v2.0.0
```

### Resource Scaling

```bash
# Scale up for load testing
helm upgrade s2search . \
  --namespace s2search \
  --set S2Backend.replicaCount=5 \
  --set S2SearchUi.replicaCount=3 \
  --set S2Backend.resources.limits.cpu=2000m \
  --set S2Backend.resources.limits.memory=2Gi

# Scale down for resource conservation
helm upgrade s2search . \
  --namespace s2search \
  --set S2Backend.replicaCount=1 \
  --set S2SearchUi.replicaCount=1 \
  --set S2Backend.resources.limits.cpu=500m \
  --set S2Backend.resources.limits.memory=512Mi
```

## 🔄 Lifecycle Management

### Installation

```bash
# Basic installation
helm install s2search . --namespace s2search

# Installation with custom values
helm install s2search . \
  --namespace s2search \
  --values custom-values.yaml

# Installation with inline overrides
helm install s2search . \
  --namespace s2search \
  --set S2Backend.image.tag=v1.3.0 \
  --set S2SearchUi.image.tag=v1.3.0

# Dry run to verify configuration
helm install s2search . \
  --namespace s2search \
  --dry-run --debug
```

### Upgrades

```bash
# Upgrade with new image versions
helm upgrade s2search . \
  --namespace s2search \
  --set S2Backend.image.tag=v1.4.0 \
  --set S2SearchUi.image.tag=v1.4.0

# Upgrade with new values file
helm upgrade s2search . \
  --namespace s2search \
  --values updated-values.yaml

# Upgrade and wait for completion
helm upgrade s2search . \
  --namespace s2search \
  --wait --timeout=10m

# Force upgrade (recreate resources)
helm upgrade s2search . \
  --namespace s2search \
  --force
```

### Rollbacks

```bash
# View release history
helm history s2search --namespace s2search

# Rollback to previous version
helm rollback s2search --namespace s2search

# Rollback to specific revision
helm rollback s2search 3 --namespace s2search

# Rollback and wait for completion
helm rollback s2search --namespace s2search --wait
```

### Uninstallation

```bash
# Uninstall release
helm uninstall s2search --namespace s2search

# Uninstall and delete namespace
helm uninstall s2search --namespace s2search
kubectl delete namespace s2search

# Keep history for potential rollback
helm uninstall s2search --namespace s2search --keep-history
```

## 📊 Monitoring and Verification

### Deployment Status

```bash
# Check Helm release status
helm status s2search --namespace s2search

# List all Helm releases
helm list --namespace s2search

# Get release values
helm get values s2search --namespace s2search

# Get release manifest
helm get manifest s2search --namespace s2search
```

### Kubernetes Resources

```bash
# Check all resources
kubectl get all --namespace s2search

# Check specific resource types
kubectl get deployments --namespace s2search
kubectl get services --namespace s2search
kubectl get configmaps --namespace s2search
kubectl get secrets --namespace s2search

# Describe resources for detailed info
kubectl describe deployment s2search-backend-deployment --namespace s2search
kubectl describe service search-api-service --namespace s2search
```

### Health Checks

```bash
# Check pod health
kubectl get pods --namespace s2search
kubectl describe pods --namespace s2search

# Check application logs
kubectl logs -l app=s2search-backend --namespace s2search
kubectl logs -l app=s2searchui --namespace s2search

# Follow logs in real-time
kubectl logs -f deployment/s2search-backend-deployment --namespace s2search
```

### Application Testing

```bash
# Port forward for testing
kubectl port-forward service/search-api-service 8080:80 --namespace s2search &
kubectl port-forward service/s2searchui-service 3000:80 --namespace s2search &

# Test API health
curl http://localhost:8080/api/SearchStatus

# Test UI accessibility
curl http://localhost:3000

# Load test preparation
kubectl get service s2searchui-service --namespace s2search -o wide
```

## 🐛 Troubleshooting

### Common Issues

#### 1. Image Pull Failures

```bash
# Check secret exists
kubectl get secret ghcr-secret --namespace s2search

# Recreate secret if needed
kubectl delete secret ghcr-secret --namespace s2search
kubectl create secret docker-registry ghcr-secret \
  --docker-server=ghcr.io \
  --docker-username=<username> \
  --docker-password=<token> \
  --docker-email=<email> \
  --namespace=s2search

# Test image pull manually
kubectl run test-pod --image=ghcr.io/square2digital/s2search-backend:latest \
  --namespace s2search --rm -it --restart=Never -- /bin/bash
```

#### 2. Pod Startup Issues

```bash
# Check pod events
kubectl describe pod <pod-name> --namespace s2search

# Check container logs
kubectl logs <pod-name> -c <container-name> --namespace s2search

# Debug with interactive shell
kubectl exec -it <pod-name> --namespace s2search -- /bin/bash

# Check resource constraints
kubectl top pods --namespace s2search
kubectl describe nodes
```

#### 3. Service Connectivity

```bash
# Check service endpoints
kubectl get endpoints --namespace s2search

# Test internal connectivity
kubectl exec -it <backend-pod> --namespace s2search -- \
  curl http://search-api-service/api/SearchStatus

# Check DNS resolution
kubectl exec -it <pod-name> --namespace s2search -- \
  nslookup search-api-service
```

#### 4. Configuration Issues

```bash
# Check ConfigMap contents
kubectl get configmap searchapi-appsettings-localk8s --namespace s2search -o yaml

# Check environment variables
kubectl exec -it <pod-name> --namespace s2search -- env

# Validate Helm template rendering
helm template s2search . --debug

# Check Helm values
helm get values s2search --namespace s2search
```

#### 5. Resource Constraints

```bash
# Check resource usage
kubectl top pods --namespace s2search
kubectl top nodes

# Check resource quotas
kubectl describe resourcequota --namespace s2search

# Check limit ranges
kubectl describe limitrange --namespace s2search

# Scale down if needed
helm upgrade s2search . --namespace s2search \
  --set S2Backend.replicaCount=1 \
  --set S2SearchUi.replicaCount=1
```

### Debug Commands

```bash
# Comprehensive debugging
kubectl get events --namespace s2search --sort-by='.lastTimestamp'
kubectl describe deployment s2search-backend-deployment --namespace s2search
kubectl logs -l app=s2search-backend --namespace s2search --tail=100

# Template debugging
helm template s2search . --debug --namespace s2search

# Values debugging
helm template s2search . --debug --set S2Backend.image.tag=debug

# Installation debugging
helm install s2search . --namespace s2search --dry-run --debug
```

## 🚀 Advanced Scenarios

### Multi-Environment Management

```bash
# Deploy to multiple environments
helm install s2search-dev . --namespace s2search-dev --values dev-values.yaml
helm install s2search-staging . --namespace s2search-staging --values staging-values.yaml
helm install s2search-prod . --namespace s2search-prod --values prod-values.yaml

# Compare configurations
helm get values s2search-dev --namespace s2search-dev
helm get values s2search-prod --namespace s2search-prod

# Sync configuration between environments
helm upgrade s2search-staging . \
  --namespace s2search-staging \
  --reuse-values \
  --set S2Backend.image.tag=v1.5.0
```

### Blue-Green Deployments

```bash
# Deploy blue environment
helm install s2search-blue . \
  --namespace s2search-blue \
  --create-namespace \
  --values production-values.yaml

# Deploy green environment with new version
helm install s2search-green . \
  --namespace s2search-green \
  --create-namespace \
  --values production-values.yaml \
  --set S2Backend.image.tag=v2.0.0 \
  --set S2SearchUi.image.tag=v2.0.0

# Switch traffic (update ingress or load balancer)
# Then cleanup old environment
helm uninstall s2search-blue --namespace s2search-blue
```

### Canary Deployments

```bash
# Deploy canary version with reduced replicas
helm install s2search-canary . \
  --namespace s2search-canary \
  --create-namespace \
  --set S2Backend.replicaCount=1 \
  --set S2SearchUi.replicaCount=1 \
  --set S2Backend.image.tag=v2.0.0-rc1 \
  --set S2SearchUi.image.tag=v2.0.0-rc1

# Monitor canary performance
kubectl top pods --namespace s2search-canary
kubectl logs -f deployment/s2search-backend-deployment --namespace s2search-canary

# Promote or rollback based on results
```

### Disaster Recovery

```bash
# Backup current configuration
helm get values s2search --namespace s2search > backup-values.yaml
kubectl get configmap --namespace s2search -o yaml > backup-configmaps.yaml

# Restore from backup
helm install s2search-restored . \
  --namespace s2search-restored \
  --create-namespace \
  --values backup-values.yaml

kubectl apply -f backup-configmaps.yaml --namespace s2search-restored
```

## 📝 Deployment Checklist

### Pre-Deployment

- [ ] Kubernetes cluster is accessible and has sufficient resources
- [ ] Helm is installed and configured
- [ ] Container registry access is configured
- [ ] Storage class is available for persistent volumes
- [ ] Network policies allow required communication
- [ ] Resource quotas are sufficient
- [ ] Values files are prepared and validated

### During Deployment

- [ ] Monitor Helm installation progress
- [ ] Watch pod startup and readiness
- [ ] Verify service creation and endpoints
- [ ] Check ConfigMap and Secret creation
- [ ] Monitor resource usage
- [ ] Verify health check endpoints

### Post-Deployment

- [ ] All pods are in Running state
- [ ] Services are accessible and responding
- [ ] Health checks are passing
- [ ] Application functionality is verified
- [ ] Performance is within expected ranges
- [ ] Logs show no critical errors
- [ ] Security policies are enforced

### Production Readiness

- [ ] High availability is configured (multiple replicas)
- [ ] Resource limits are appropriately set
- [ ] Monitoring and alerting are configured
- [ ] Backup and recovery procedures are tested
- [ ] Security scanning is complete
- [ ] Performance testing is satisfactory
- [ ] Documentation is updated

## 🆘 Emergency Procedures

### Rapid Recovery

```bash
# Quick rollback to last known good version
helm rollback s2search --namespace s2search

# Emergency scale down
helm upgrade s2search . --namespace s2search \
  --set S2Backend.replicaCount=0 \
  --set S2SearchUi.replicaCount=0

# Emergency uninstall
helm uninstall s2search --namespace s2search

# Force cleanup stuck resources
kubectl delete pods --all --namespace s2search --force --grace-period=0
```

### Recovery from Failed State

```bash
# Clean slate recovery
helm uninstall s2search --namespace s2search
kubectl delete namespace s2search
kubectl create namespace s2search

# Recreate secrets
kubectl create secret docker-registry ghcr-secret \
  --docker-server=ghcr.io \
  --docker-username=<username> \
  --docker-password=<token> \
  --docker-email=<email> \
  --namespace=s2search

# Redeploy with last known good configuration
helm install s2search . --namespace s2search --values backup-values.yaml
```

## 📞 Support and Resources

### Getting Help

- **Chart Issues**: Check template rendering with `helm template --debug`
- **Pod Issues**: Use `kubectl describe` and `kubectl logs` for diagnostics
- **Performance Issues**: Monitor with `kubectl top` and check resource limits
- **Configuration Issues**: Validate values with `helm get values`

### Useful Commands Quick Reference

```bash
# Status and information
helm status s2search -n s2search
helm list -n s2search
kubectl get all -n s2search

# Logs and debugging
kubectl logs -l app=s2search-backend -n s2search
kubectl describe pod <pod-name> -n s2search
helm template s2search . --debug

# Scaling and updates
helm upgrade s2search . -n s2search --set S2Backend.replicaCount=3
kubectl scale deployment s2search-backend-deployment --replicas=2 -n s2search

# Troubleshooting
kubectl top pods -n s2search
kubectl get events -n s2search --sort-by='.lastTimestamp'
kubectl exec -it <pod-name> -n s2search -- /bin/bash
```

---

_For architectural details, see [README.md](README.md)_
_For advanced Helm configuration, see [Helm Documentation](https://helm.sh/docs/)_
