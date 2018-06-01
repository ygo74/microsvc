# Integration Jenkins AKS

Source : [Azure Quick Start - CICD](https://github.com/Azure/azure-quickstart-templates/tree/master/jenkins-cicd-container)  

## Create group
`az group create --name Continuousintegration --location westeurope`

## Create Image registry ACR
Source : [Azure - Container Registry](https://docs.microsoft.com/fr-fr/azure/container-registry/container-registry-get-started-azure-cli)
`az acr create --resource-group Continuousintegration --name mesfContainerRegistry --sku Basic`  
> Name must be ^[a-zA-Z0-9]*$  


### Login to ACR
> Docker must run on the source command line server  

### Query acr
`az acr list --resource-group Continuousintegration --query "[].{acrLoginServer:loginServer}" --output table`

### Tag docker image
`docker tag rabbitmq:3-management-alpine mesfcontainerregistry.azurecr.io/rabbitmq:v1`

### Push docker image to ACR
`docker push mesfcontainerregistry.azurecr.io/rabbitmq:v1`
