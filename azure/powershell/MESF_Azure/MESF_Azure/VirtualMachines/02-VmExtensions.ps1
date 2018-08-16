
# Get-AzureRmVMImagePublisher -Location $Location
# Get-AzureRmVMExtensionImageType -Location $Location -PublisherName "Canonical"
# Get-AzureRmVMExtensionImage -Location $Location -PublisherName "Canonical"

# Get-AzureRmVmImagePublisher -Location $Location | `
# Get-AzureRmVMExtensionImageType | `
# Get-AzureRmVMExtensionImage | Select Type, Version