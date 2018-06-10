#Install-Module AzureRM
Login-AzureRmAccount

#Test DSC
D:\devel\Azure\Init-Config.ps1


#Create the resource group
$resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName -Location $location
if ($resourceGroup -eq $null)
{
    $resourceGroup = New-AzureRmResourceGroup -Name $resourceGroupName -Location $location
}


#Create the network configurqtion
$vnetName = "vnet_"  + $resourceGroup.ResourceGroupName
$subNetName = "subnet_" + $resourceGroup.ResourceGroupName
$subnetAddressPrefix = "10.0.64.0/24"
$vnetAddressPrefix = "10.0.0.0/16"

$vnet = Get-AzureRmVirtualNetwork -Name $vnetName -ResourceGroupName $resourceGroup.ResourceGroupName
if ($vnet -eq $null)
{
    $subnet = New-AzureRmVirtualNetworkSubnetConfig -Name $subNetName -AddressPrefix $subnetAddressPrefix
    $vnet = New-AzureRmVirtualNetwork -Name $vnetName -ResourceGroupName $resourceGroup.ResourceGroupName -Subnet $subnet -AddressPrefix $vnetAddressPrefix -Location $resourceGroup.location

}
else
{
    $subnet = Get-AzureRmVirtualNetworkSubnetConfig -Name $subNetName -VirtualNetwork $vnet
}

$VmData = $vmDatas[3]
#Prepare the Network Interface for VMS
$publicIpName = $resourceGroup.ResourceGroupName + "_publicIp_" + $VmData.Name
$NICName = $resourceGroup.ResourceGroupName + "_nic_" + $VmData.Name


$publicIp = Get-AzureRmPublicIpAddress -Name $publicIpName -ResourceGroupName $resourceGroup.ResourceGroupName
if ($publicIp -eq $null)
{
    $publicIp = New-AzureRmPublicIpAddress -Name $publicIpName -ResourceGroupName $resourceGroup.ResourceGroupName -Location $resourceGroup.Location -AllocationMethod Dynamic
}

$nic = Get-AzureRmNetworkInterface -Name $NICName -ResourceGroupName $resourceGroup.ResourceGroupName
if ($nic -eq $null)
{
    $nic = New-AzureRmNetworkInterface -Name $NICName -ResourceGroupName $resourceGroup.ResourceGroupName -Location $resourceGroup.Location -SubnetId $vnet.Subnets[0].Id -PublicIpAddressId $publicIp.Id
}

#Prepare the VM
$cred = Get-Credential -Message "Type the name and password of the local administrator account."

$VMConfig = New-AzureRmVMConfig -VMName $VmData.Name -VMSize $VmData.Size
switch($VmData.Type)
{
    "windows" 
        { Set-AzureRmVMOperatingSystem -VM $VMConfig -Windows -ComputerName $VmData.ComputerName -Credential $cred -ProvisionVMAgent -EnableAutoUpdate }
    "linux"
        { Set-AzureRmVMOperatingSystem -VM $VMConfig -Linux -ComputerName $VmData.ComputerName -Credential $cred }

}

#Check if Image exists
$images = Get-AzureRmVMImage -PublisherName $VmData.Publisher -Offer $VmData.Offer -Skus $VmData.skus -location $resourceGroup.Location -ErrorAction Stop

#Add images to the VM
Set-AzureRmVMSourceImage -VM $VMConfig -PublisherName $VmData.Publisher -Offer $VmData.Offer -Skus $VmData.skus -Version latest
Add-AzureRmVMNetworkInterface -VM $VMConfig -Id $nic.Id

$diskName = $resourceGroup.ResourceGroupName + "_disk_" + $VmData.Name + "_" + $VmData.DiskName
Set-AzureRmVMOSDisk -VM $VMConfig -Name $diskName -StorageAccountType $VmData.StorageType -DiskSizeInGB $VmData.DiskSize -CreateOption FromImage -Caching ReadWrite

$vm = New-AzureRmVM -ResourceGroupName $resourceGroup.ResourceGroupName -Location $resourceGroup.Location -VM $VMConfig

Get-AzureRmVM -ResourceGroupName  $resourceGroup.ResourceGroupName | Start-AzureRmVM
Get-AzureRmVM -ResourceGroupName  $resourceGroup.ResourceGroupName | Stop-AzureRmVM -Force

$existingVMs = Get-AzureRmVM -ResourceGroupName  $resourceGroup.ResourceGroupName

workflow Start-AzureVMs 
{ 
    param( 
       [string]$ResourceGroupName 
    ) 

    $existingVMs = Get-AzureRmVM -ResourceGroupName  $ResourceGroupName


    Foreach -parallel ($VM in $existingVMs) { 
        AzureRM.Compute\Start-AzureRmVm -ResourceGroupName "$ResourceGroupName" -Name $VM.Name 
        } 

}

Start-AzureVMs -ResourceGroupName $resourceGroup.ResourceGroupName

#$myVM = Set-AzureRmVMSourceImage -VM $myVM -PublisherName "MicrosoftVisualStudio" -Offer "VisualStudio" -Skus "VS-2015-Ent-VSU3-AzureSDK-29-WS2012R2" -Version "latest"

#$myVM = Set-AzureRmVMOperatingSystem -VM $myVM -Linux -ComputerName "Ubuntu01" -Credential $cred 
#$VMConfig
#$VMConfig.OSProfile
#
#
#Set-AzureRmVMOperatingSystem -VM $VMConfig -Windows -ComputerName $computerName -Credential $cred -ProvisionVMAgent -EnableAutoUpdate
#Set-AzureRmVMSourceImage -VM $VMConfig -PublisherName "MicrosoftWindowsServer" -Offer "WindowsServer" -Skus "2012-R2-Datacenter-smalldisk" -Version latest
#Add-AzureRmVMNetworkInterface -VM $VMConfig -Id $nic.Id
#Set-AzureRmVMOSDisk -VM $VMConfig -Name $diskName -StorageAccountType PremiumLRS -DiskSizeInGB 128 -CreateOption FromImage -Caching ReadWrite
#
#$vm = New-AzureRmVM -ResourceGroupName $resourceGroupName -Location $location -VM $VMConfig
#
#$myVM = Set-AzureRmVMSourceImage -VM $myVM -PublisherName "Canonical" -Offer "UbuntuServer" -Skus "16.04-LTS" -Version "latest"
#
#$myVM = Add-AzureRmVMNetworkInterface -VM $myVM -Id $myNIC.Id
#$myVM = Add-AzureRmVMNetworkInterface -VM $myVM -Id $myNICUbuntu.Id
#
#$myVM = Set-AzureRmVMOSDisk -VM $myVM -Name "myOsDisk2" -StorageAccountType PremiumLRS -DiskSizeInGB 128 -CreateOption FromImage -Caching ReadWrite
#
#New-AzureRmVM -ResourceGroupName $myResourceGroup -Location $location -VM $myVM
#
#Stop-AzureRmVM -Name myVM -ResourceGroupName MYRESOURCEGROUP
#Start-AzureRmVM -Name myVM -ResourceGroupName MYRESOURCEGROUP
#
#
#Get-AzureRmVM -ResourceGroupName $myResourceGroup -Name ‘Ubuntu01’ #| Get-AzureRmPublicIpAddress
#Get-AzureRmPublicIpAddress -Name "myPublicIpUbuntu" -ResourceGroupName $myResourceGroup
#
#Stop-AzureRmVM -Name Ubuntu01 -ResourceGroupName MYRESOURCEGROUP
#Start-AzureRmVM -Name Ubuntu01 -ResourceGroupName MYRESOURCEGROUP