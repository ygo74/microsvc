$Debug = $true
Function Trace-Message
{
    param([string] $Message)
    
    if ($Debug)
    {
        #Write-Host $Message -ForegroundColor Magenta
        #Check why Verbose is not OK
        $VerbosePreference="Continue"
        Write-Verbose $Message
    }
}


Function Trace-StartFunction
{
    [OutputType( [system.diagnostics.stopwatch] )]
    param(
        [Parameter(Mandatory = $true)]
        [System.Management.Automation.FunctionInfo] $InvocationMethod        
    )
    
    if ($Debug)
    {
        $watch = [system.diagnostics.stopwatch]::StartNew()         
        $Message = "{0} : Start {1}" -f [System.DateTime]::Now, $InvocationMethod.Name
        Trace-Message -Message $Message
        return $watch
    }
}

Function Trace-EndFunction
{
    param(
        [Parameter(Mandatory = $true)]
        [System.Management.Automation.FunctionInfo] $InvocationMethod,

        [Parameter(Mandatory = $false)]
        [System.Diagnostics.Stopwatch] $watcher
    )
    
    if ($Debug)
    {
        $Message = "{0} : End {1}" -f [System.DateTime]::Now, $InvocationMethod.Name
        if ($watcher -ne $null)
        {
           $watcher.Stop(); 
           $message += " => Completion Time : $($watcher.Elapsed)"    
        }
        Trace-Message -Message $Message
    }
}