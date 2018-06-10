param(
    [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,

    [Parameter(Mandatory=$true)]
    [string]$Location
)

$nw = Get-AzurermResource `
  | Where-Object {$_.ResourceType -eq "Microsoft.Network/networkWatchers" -and $_.Location -eq $location }

if ($nw -eq $null)
{
    $networkWatcher = New-AzureRmNetworkWatcher -Name "NetworkWatcher_$resourceGroupName" `
                      -ResourceGroupName $resourceGroupName `
                      -Location $location
}  
else
{
    $networkWatcher = Get-AzureRmNetworkWatcher `
    -Name $nw.Name `
    -ResourceGroupName $nw.ResourceGroupName      
}

$networkWatcher