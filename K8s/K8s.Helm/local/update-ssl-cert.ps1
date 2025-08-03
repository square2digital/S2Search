$DeploymentRoot = "E:\devops"
$S2Namespace = "ingress-nginx"

#################################
## - we need to understand this
#################################
$letsEncryptCertPath = "C:\Certbot\live\s2search.co.uk-0002"
#$letsEncryptCertPath = "C:\Certbot\archive\s2search.co.uk-0002"

Write-Host "################################"
Write-Host "Create tls Secret"
Write-Host "################################"

kubectl delete secret s2search-ssl-cert --namespace $S2Namespace
kubectl create secret tls s2search-ssl-cert --cert="$letsEncryptCertPath\cert.pem" --key="$letsEncryptCertPath\privkey.pem" --namespace $S2Namespace
kubectl apply -f "$DeploymentRoot\K8s.Local.Cluster.Setup\local\Kubernetes\ingress-controller\nginx\customers-ingress\s2search-ingress.yml" --namespace $S2Namespace