# Dapr

<https://dapr.io>

# Installation

-  Install DAPR CLI
    ```
    wget -q https://raw.githubusercontent.com/dapr/cli/master/install/install.sh -O - | /bin/bash
    ```
- Initialize DAPR containers
    ```
    dapr init
    ```

# Implementation

## Service Invocation

- Show in `Program.cs` and `DaprRandomNumberService.cs` of LottoService
- Uses the .net SDK
- Show Dapr Debugging with Code

## Code Integration

- Scaffold Dapr Tasks via Command Palette
- Scaffold Dapr Components via Command Palette

## State Store Redis

- Show components `dapr/components/state.redis.yaml`
- Show `DaprCachedLottoNumberService.cs` 
- Run Dapr debug

## Sate Store Mongo

- Show components `dapr/components/state.mongo.yaml`
- Change `appsettings.Development.json` in LottoService to
    ```json
    "DaprStoreId": "lottostatestore-mongo"
    ```
- Rerun Dapr debug

# Uninstall

- Uninstall completely
    ```
    dapr uninstall --all    # uninstall everything dapr related
    ```
