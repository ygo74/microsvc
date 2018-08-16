Function Set-PublicIP
{
    [cmdletbinding(DefaultParameterSetName="none")]
    Param( 
        [Parameter(Mandatory=$true)]
        [string]$ResourceGroupName,
    
        [Parameter(Mandatory=$true)]
        [string]$Location,
    
        [Parameter(Mandatory=$true)]
        [String]$Name,

        [Parameter(Mandatory=$true)]
        [String]$Alias

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


        Trace-Message -Message ("Try to retrieve Public IP '{0}' in resourceGroup '{1}'" -f $Name, $ResourceGroupName)
        $publicIp = Get-AzureRmPublicIpAddress -Name $Name `
                                               -ResourceGroupName $ResourceGroupName `
                                               -ErrorAction SilentlyContinue
                                               

        if ($null -eq $publicIp)
        {
            Trace-Message -Message ("Public IP '{0}' in resourceGroup '{1}' doesn't exist, it will be created" -f $Name, $ResourceGroupName)
            $publicIp = New-AzureRmPublicIpAddress -Name $Name `
                                                -ResourceGroupName $ResourceGroupName `
                                                -Location $Location `
                                                -AllocationMethod Static `
                                                -IdleTimeoutInMinutes 4 `
                                                -DomainNameLabel $Alias
        }

        write-output $publicIp

    }        
}