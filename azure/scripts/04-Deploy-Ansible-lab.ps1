if ([String]::IsNullOrEmpty($PSScriptRoot)) {
    $rootScriptPath = "D:\devel\Azure\git\microsvc\azure\scripts"
}
else {
    $rootScriptPath = $PSScriptRoot
}    

$ModulePath = "$rootScriptPath\..\powershell\MESF_Azure\MESF_Azure.psd1" 
Import-Module $ModulePath -force


& "$rootScriptPath\..\configuration\04-Ansible-lab.ps1"

Remove-AzureRmResourceGroup -Name $ResourceGroupName -Force

Set-ResourceGroup -ResourceGroupName $ResourceGroupName -Location $Location

foreach($virtualNetwork in $virtualNetworks)
{
    Set-VirtualNetwork -ResourceGroupName $ResourceGroupName -Location $Location -Network $virtualNetwork
}

foreach($PublicIpKey in $PublicIps.Keys)
{
    $publicIp = $PublicIps[$PublicIpKey]
    Set-PublicIP -ResourceGroupName $ResourceGroupName -Location $location `
                 -Name $publicIp.Name -Alias $publicIp.Alias
}

foreach($virtualMachine in $vmDatas)
{
    if ($virtualMachine.Name -eq "Vm2") {
        Set-VirtualMachine -ResourceGroupName $ResourceGroupName -Location $Location -VirtualMachine $virtualMachine
    }        
}    

#Remove-AzureRmResourceGroup -Name $ResourceGroupName

# Install NGINX.
# $PublicSettings = '{"commandToExecute":"apt-get -y update && apt-get -y install nginx"}'

# Set-AzureRmVMExtension -ExtensionName "NGINX" -ResourceGroupName $ResourceGroupName -VMName "Vm2" `
#   -Publisher "Microsoft.Azure.Extensions" -ExtensionType "CustomScript" -TypeHandlerVersion 2.0 `
#   -SettingString $PublicSettings -Location $location

$PublicSettings = '{"commandToExecute":"apt-get update && apt-get --assume-yes install software-properties-common && apt-add-repository ppa:ansible/ansible && apt-get update && apt-get --assume-yes install ansible"}'
Set-AzureRmVMExtension -ExtensionName "Ansible" -ResourceGroupName $ResourceGroupName -VMName "Vm2" `
  -Publisher "Microsoft.Azure.Extensions" -ExtensionType "CustomScript" -TypeHandlerVersion 2.0 `
  -SettingString $PublicSettings -Location $location

