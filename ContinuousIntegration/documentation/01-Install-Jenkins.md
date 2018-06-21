# Preparation
## Create a resource group  

> az group create --name ResourceGroupJenkinsCI --location westeurope  

## Create a VM  
cloud-init-jenkins.yml file is in ContinuousIntegration  

```bash
az vm create --resource-group Continuousintegration 
--name JenkinsCI01 
--image UbuntuLTS 
--admin-username azureuser 
--ssh-key-value @%USERPROFILE%\.ssh\.azureuser.pub
--public-ip-address-dns-name jenkins-ci-01
--custom-data cloud-init-jenkins.yml
```

