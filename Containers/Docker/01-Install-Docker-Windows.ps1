
#Powerhsell Infrastructure requirements
Install-PackageProvider -Name Nuget -MinimumVersion 2.8.5.201 -Force
Install-Module -Name PackageManagement -Force
Install-Module -Name PowerShellGet -Force


Install-Module -Name DockerMsftProvider -Repository PSGallery -Force
Install-Package -Name docker -ProviderName DockerMsftProvider

$x = Install-WindowsFeature Containers
$x.RestartNeeded

#http://voloda.bazilisek.net/2017/11/jenkins-docker-slave-windows-build-agent/
