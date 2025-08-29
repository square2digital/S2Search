# Redis on Kubernetes

## Namespace

```
kubectl create ns redis
```

## Storage Class

```
kubectl get storageclass
NAME                 PROVISIONER             RECLAIMPOLICY   VOLUMEBINDINGMODE      ALLOWVOLUMEEXPANSION   AGE
hostpath (default)   docker.io/hostpath      Delete          Immediate              false                  36d
```

## Deployment: Redis nodes

```
cd "C:\git\S2 Search\S2Search.Infrastructure.K8s\local\redis"
kubectl apply -n redis -f ./redis-configmap.yaml
kubectl apply -n redis -f ./redis-statefulset.yaml

kubectl -n redis get pods
kubectl -n redis get pv

kubectl -n redis logs redis-0
kubectl -n redis logs redis-1
kubectl -n redis logs redis-2
```

## Test replication status

```
kubectl -n redis exec -it redis-0 sh

on the okteto instance its -> kubectl exec redis-master-0 -it -- sh

redis-cli
auth a-very-complex-password-here
info replication
```

## Deployment: Redis Sentinel (3 instances)

```
cd "C:\git\S2 Search\S2Search.Infrastructure.K8s\local\redis\sentinel"
kubectl apply -n redis -f ./sentinel-statefulset.yaml

kubectl -n redis get pods
kubectl -n redis get pv
kubectl -n redis logs sentinel-0
```
