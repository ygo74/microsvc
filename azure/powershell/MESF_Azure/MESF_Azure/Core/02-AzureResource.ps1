function Test-Resource
{
    <#
        .Description
        Test if a Resource exist in your Azure workspace

        .Example
        Test-Resource -Name

        .Example

        .Notes
        The output is a Boolean
    #>
#    [Diagnostics.CodeAnalysis.SuppressMessageAttribute( "PSAvoidDefaultValueForMandatoryParameter", "" )]
    [CmdletBinding( DefaultParameterSetName = 'Default', SupportsShouldProcess=$false )]
    [OutputType( [Boolean] )]
    param(

        [Parameter(Mandatory = $true, Position = 0)]
        [string]
        $ResourceName,

        [Parameter(Mandatory = $true, Position = 1)]
        [string]
        $ResourceType,

        [Parameter(Mandatory = $true, Position = 2)]
        [string]
        $ResourceGroupName
    )

    begin
    {
        $watch = Trace-StartFunction -InvocationMethod $MyInvocation.MyCommand
    }

    end
    {
        Trace-EndFunction -InvocationMethod $MyInvocation.MyCommand -watcher $watch
    }

    process
    {
        try
        {
            $azResource = Get-AzureRmResource -ResourceType $ResourceType `
                                -ResourceGroupName $ResourceGroupName `
                          | Where-Object {$_.ResourceName -eq $ResourceName}

            return ($azResource -ne $null)                                
        }
        catch
        {
            $PSCmdlet.ThrowTerminatingError( $PSitem )
        }
    }

}
#Export-ModuleMember -Function Test-Resource