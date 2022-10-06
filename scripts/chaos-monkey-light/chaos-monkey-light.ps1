Param(
    $SleepSeconds = 30,
    $Namespace = 'default',
    $Count = 2
)

Write-Host "Killing $COUNT pods every $SleepSeconds seconds from namespace $NAMESPACE"
while ($true) {
    kubectl get pods -o name | Select -Last $Count | % { Invoke-Expression "kubectl -n $Namespace delete --wait=false $_" }
    Start-Sleep $SleepSeconds
}
