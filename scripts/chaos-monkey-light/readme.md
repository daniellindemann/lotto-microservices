# chaos-monkey-light

This sample run a script that dynamically kills a specific count of pods (default 2) in the kubernetes cluster.

## Execute the script

Switch the directory in a terminal to `scripts/chaos-monkey-light` and execute the script.

Bash:
```BASH
./chaos-monkey-light.sh
```

PowerShell
```PowerShell
./chaos-monkey-light.ps1
```

You can take a look into the scripts for futher configuration.

## See the results

Open a new terminal and execute this command.

Bash:
```BASH
watch kubectl get pods
```

PowerShell
```PowerShell
while ($true) { Invoke-Expression "kubectl get pods" | Out-Host; Sleep 2; Clear }
```
