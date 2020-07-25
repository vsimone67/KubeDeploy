param(
    [Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $command
    
)
kubedeploy $command --name NAMEGOESHERE --namespace NAMESPACEGOESHERE --projdir .
