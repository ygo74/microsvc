$queryLocation = "westeurope"

Get-AzureRmVMImagePublisher -Location $queryLocation
Get-AzureRmVMExtensionImageType -Location $queryLocation -PublisherName "Canonical"
Get-AzureRmVMExtensionImage -Location $queryLocation -PublisherName "Canonical"

Get-AzureRmVmImagePublisher -Location $queryLocation | `
Get-AzureRmVMExtensionImageType | `
Get-AzureRmVMExtensionImage | Select Type, Version