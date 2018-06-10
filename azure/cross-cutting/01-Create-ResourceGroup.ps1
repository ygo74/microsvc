param(
    [Parameter(Mandatory=$true)]
    [string]$ResourceGroupName,

    [Parameter(Mandatory=$true)]
    [string]$Location,
)


#Create the resource group
$resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName -Location $location -ErrorVariable checkResourceGroup  -ErrorAction SilentlyContinue

if ($checkResourceGroup)
{
    Write-Host "Resource Group doesn't exist"
    Write-Output $checkResourceGroup
}

if ($resourceGroup -eq $null)
{
    $resourceGroup = New-AzureRmResourceGroup -Name $resourceGroupName -Location $location
}

$resourceGroup