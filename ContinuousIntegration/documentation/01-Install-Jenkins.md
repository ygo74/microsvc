# Preparation
## Follow prerequisites  
* Ensure SSH Public-private key for VM
* Resource group **ContinuousIntegration** has been created   


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
## Open Internet Access  
`az vm open-port --resource-group ContinuousIntegration --name JenkinsCI01 --port 8080 --priority 1001`  

## Unlock Jenkins
Jenkins unlock password : > sudo cat /var/lib/jenkins/secrets/initialAdminPassword  


