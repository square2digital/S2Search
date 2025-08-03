# Introduction 
The sole purpose of this API is to provide client configuration to the Search API.

In order to enable a scalable solution for the Search UI and to protect key variables, 
this API will expose endpoints that will allow the Search API to retrieve:
1. Search index query credentials
2. Theme configuration per client

This API is not exposed publicly and will only be accessible from within a Kubernetes cluster.