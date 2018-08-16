
# # Create an inbound network security group rule for port 22
# $nsgRuleSSH = New-AzureRmNetworkSecurityRuleConfig -Name myNetworkSecurityGroupRuleSSH  -Protocol Tcp `
#   -Direction Inbound -Priority 1000 -SourceAddressPrefix * -SourcePortRange * -DestinationAddressPrefix * `
#   -DestinationPortRange 22 -Access Allow

# # Create a network security group
# $nsg = New-AzureRmNetworkSecurityGroup -ResourceGroupName $resourceGroup -Location $location `
#   -Name myNetworkSecurityGroup -SecurityRules $nsgRuleSSH

$global:FirewallDefaultRules=@{
    allowSsh = @{
        Name="AllowSsh"
        Protocol="Tcp"
        Direction="Inbound"
        Priority=1000
        SourceAddressPrefix="*"
        SourcePortRange="*"
        DestinationAddressPrefix="*"
        DestinationPortRange=22
        Access="Allow"
    }
    allowRdp = @{
        Name="allowRdp"
        Protocol="Tcp"
        Direction="Inbound"
        Priority=1000
        SourceAddressPrefix="*"
        SourcePortRange="*"
        DestinationAddressPrefix="*"
        DestinationPortRange=3389
        Access="Allow"
    }
    allowWinrm = @{
        Name="allowWinrm"
        Protocol="Tcp"
        Direction="Inbound"
        Priority=1000
        SourceAddressPrefix="*"
        SourcePortRange="*"
        DestinationAddressPrefix="*"
        DestinationPortRange=5985
        Access="Allow"
    }
    allowWinrms = @{
        Name="allowWinrms"
        Protocol="Tcp"
        Direction="Inbound"
        Priority=1000
        SourceAddressPrefix="*"
        SourcePortRange="*"
        DestinationAddressPrefix="*"
        DestinationPortRange=5986
        Access="Allow"
    }
}


function ConvertTo-AzureRMSecurityRule
{
    [cmdletbinding(DefaultParameterSetName="none")]
    Param( 
        [Parameter(Mandatory=$true,  ValueFromPipeline=$true)]
        [NetworkRule]$InputObject
    )

    #Check Attribute defqult Value
    $description = $InputObject.Description
    if ([String]::IsNullOrEmpty($description))
    {
        $description = "Network Secuirty rule : {0}" -f $InputObject.Name
    }

    New-AzureRmNetworkSecurityRuleConfig -Name     $InputObject.Name `
                        -Description               $description `
                        -Protocol                  $InputObject.Protocol `
                        -Direction                 $InputObject.Direction `
                        -Priority                  $InputObject.Priority `
                        -SourcePortRange           $InputObject.SourcePortRange `
                        -SourceAddressPrefix       $InputObject.SourceAddressPrefix `
                        -DestinationPortRange      $InputObject.DestinationPortRange `
                        -DestinationAddressPrefix  $InputObject.DestinationAddressPrefix

}        

function Set-NetworkSecurityGroup
{
    [cmdletbinding(DefaultParameterSetName="none")]
    Param( 
        [Parameter(Mandatory=$true)]
        [string]$ResourceGroupName,
    
        [Parameter(Mandatory=$true)]
        [string]$Location,

        [Parameter(Mandatory=$true)]
        [string]$Name,

        [Parameter(Mandatory=$true)]
        [NetworkRule[]]$Rules
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
        $securityGroup = Get-AzureRmNetworkSecurityGroup -Name $Name `
                             -ResourceGroupName $ResourceGroupName -ErrorAction SilentlyContinue

        if (-$null -eq $securityGroup)
        {
            Trace-Message -Message ("Security group '{0}' in resourceGroup '{1}' doesn't exist, it will be created")
            $securityGroup = New-AzureRmNetworkSecurityGroup -Name $Name `
                                -ResourceGroupName $ResourceGroupName -Location $Location `
                                -SecurityRules
        }
        else {
            $securityGroup.SecurityRules 
        }

    }
}