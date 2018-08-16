#https://docs.microsoft.com/en-sg/azure/virtual-machines/scripts/virtual-machines-linux-powershell-sample-create-vm-oms?toc=%2Fpowershell%2Fmodule%2Ftoc.json
Function Set-VirtualMachine
{
    [cmdletbinding(DefaultParameterSetName="none")]
    Param( 
        [Parameter(Mandatory=$true)]
        [string]$ResourceGroupName,
    
        [Parameter(Mandatory=$true)]
        [string]$Location,
    
        [Parameter(Mandatory=$true)]
        [Object]$VirtualMachine
    )
    begin
    {
        $watch = Trace-StartFunction -InvocationMethod $MyInvocation.MyCommand
    }

    end
    {
        Trace-EndFunction -InvocationMethod $MyInvocation.MyCommand -watcher $watch
    }
    Process
    {


        Trace-Message -Message ("Try to retrieve Virtual Machine '{0}' in resourceGroup '{1}'" -f $VirtualMachine.Name, $ResourceGroupName)
        $vm = Get-AzureRmVM -ResourceGroupName $ResourceGroupName -Name $VirtualMachine.Name -ErrorAction SilentlyContinue

        #Prepare the Network Interface for VMS
        $publicIpName   = [String]::Format("{0}_publicIp_{1}", $ResourceGroupName , $VirtualMachine.Name)
        $NICName        = [String]::Format("{0}_nic_{1}", $ResourceGroupName , $VirtualMachine.Name)
        $diskName       = [String]::Format("{0}_disk_{1}", $ResourceGroupName , $VirtualMachine.Name)
        
        if ($null -eq $Vm)
        {
            
            $PublicIpAddressId = $null
            if ($VirtualMachine.PublicIp)
            {
                $publicip = Set-PublicIP -ResourceGroupName $ResourceGroupName -Location $Location `
                             -Name $VirtualMachine.PublicIp.Name `
                             -Alias $VirtualMachine.PublicIp.Alias 

                $PublicIpAddressId = $publicip.Id
                Trace-Message -Message ("Public IP '{0}' is identified with id '{1}'" -f $VirtualMachine.PublicIp.Name, $PublicIpAddressId)
            }

            $subnet = Get-NetworkSubNet -ResourceGroupName $ResourceGroupName -Location $Location `
                        -NetworkName $virtualMachine.NetworkName `
                        -SubnetName $virtualMachine.SubnetName
            
            $nic = Set-NetworkInterface -ResourceGroupName $ResourceGroupName -Location $location `
                        -TargetName $NICName `
                        -Subnet $subnet `
                        -PublicIpAddressId $PublicIpAddressId

            #Prepare the VM
            $cred = Get-Credential -Message "Type the name and password of the local administrator account."

            $VMConfig = New-AzureRmVMConfig -VMName $VirtualMachine.Name -VMSize $VirtualMachine.Size
            switch($VirtualMachine.Type)
            {
                "windows" 
                    { 
                        Set-AzureRmVMOperatingSystem -VM $VMConfig `
                                                    -Windows `
                                                    -ComputerName $VirtualMachine.ComputerName `
                                                    -Credential $cred `
                                                    -ProvisionVMAgent -EnableAutoUpdate 
                    }
                "linux"
                    { 
                        Set-AzureRmVMOperatingSystem -VM $VMConfig `
                                                    -Linux `
                                                    -ComputerName $VirtualMachine.ComputerName `
                                                    -Credential $cred 


                        if ($null -ne $VirtualMachine.SshPublicKey)
                        {
                            Add-AzureRmVMSshPublicKey -VM $VMConfig `
                                                    -KeyData $VirtualMachine.SshPublicKey `
                                                    -Path "/home/myadmin/.ssh/authorized_keys"

                            $VMConfig.OSProfile.LinuxConfiguration.DisablePasswordAuthentication = $true
                        }                            
                    }

            }

            #Check if Image exists
            $images = Get-AzureRmVMImage -PublisherName $VirtualMachine.Publisher `
                                        -Offer $VirtualMachine.Offer `
                                        -Skus $VirtualMachine.skus `
                                        -location $Location `
                                        -ErrorAction Stop

            #Add images to the VM
            Set-AzureRmVMSourceImage -VM $VMConfig `
                                    -PublisherName $VirtualMachine.Publisher `
                                    -Offer $VirtualMachine.Offer `
                                    -Skus $VirtualMachine.skus `
                                    -Version latest

            #Add network Interface to the VM
            Add-AzureRmVMNetworkInterface -VM $VMConfig -Id $nic.Id


            Set-AzureRmVMOSDisk -VM $VMConfig -Name $diskName `
                                -StorageAccountType $VirtualMachine.StorageType `
                                -DiskSizeInGB $VirtualMachine.DiskSize `
                                -CreateOption FromImage -Caching ReadWrite

            New-AzureRmVM -ResourceGroupName $ResourceGroupName -Location $Location -VM $VMConfig
        
        }

        Write-Output $vm
    }
}