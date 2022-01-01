param(
    [Parameter(Position = 0, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $appName,

    [Parameter(Position = 1, Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.String]
    $projectDir
)

Set-Location $projectDir
docker build -t $appName -f .
docker tag "$($appName):latest" vsimone67/"$($appName):latest"
docker push vsimone67/"$($appName):latest";