# Integration Jenkins AKS

Source : [Azure Quick Start - CICD](https://github.com/Azure/azure-quickstart-templates/tree/master/jenkins-cicd-container)  

## Create group
`az group create --name Continuousintegration --location westeurope`

## Create Image registry ACR
Source : [Azure - Container Registry](https://docs.microsoft.com/fr-fr/azure/container-registry/container-registry-get-started-azure-cli)  
`az acr create --resource-group Continuousintegration --name mesfContainerRegistry --sku Basic`  
> Name must be ^[a-zA-Z0-9]*$  


### Login to ACR
`az acr login --name mesfContainerRegistry`  
> Docker must run on the source command line server  

### Query acr
`az acr list --resource-group Continuousintegration --query "[].{acrLoginServer:loginServer}" --output table`  

### Tag docker image
`docker tag rabbitmq:3-management-alpine mesfcontainerregistry.azurecr.io/rabbitmq:v1`  

### Push docker image to ACR
`docker push mesfcontainerregistry.azurecr.io/rabbitmq:v1`  

### List Container in ACR
`az acr repository list --name mesfContainerRegistry --output table`  

### List tags on container
`az acr repository show-tags --name mesfContainerRegistry --repository rabbitmq --output table`

## Create cluster AKS

### Prerequisites
Providers  
`az provider register -n Microsoft.Network`  
`az provider register -n Microsoft.Storage`  
`az provider register -n Microsoft.Compute`  
`az provider register -n Microsoft.ContainerService`  

### Create cluster
`az aks create --resource-group Continuousintegration --name mesfAKS --node-count 1 --generate-ssh-keys`  

> Create also a dedidacated Service Principal : mesfAKS  
> 03/06/2018 : version 1.9.6  

### Install integrated kubectl
`az aks install-cli`

### Retrieve credential Locally
`az aks get-credentials --resource-group Continuousintegration --name mesfAKS`  

> Stored in %HOMEPATH%\.kube\config  

### Verify Kubernete Access
`kubectl get nodes`  

### Browse the dashboard
`az aks browse --resource-group Continuousintegration --name mesfAKS`


