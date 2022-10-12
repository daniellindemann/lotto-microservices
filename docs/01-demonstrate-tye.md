## Tye Demo

<https://github.com/dotnet/tye>

- Install tye
    ```bash
    dotnet tool install -g Microsoft.Tye --version "0.11.0-alpha.22111.1"
    ```
- Show what's possible with tye
    ```bash
    tye -h
    ```
- Initialize tye
    ```bash
    tye init lotto-microservices.sln
    ```
    - Erstellt tye config `tye.yaml`
    - It's like a docker compose file
- Execute tye run
    ```bash
    tye run
    ```
    - Launches all applications as individual processes
        - Configures custom ports
    - Adds build in service discovery <https://github.com/dotnet/tye/blob/main/docs/reference/service_discovery.md>
    - Starts all applications in watch mode
        - Let's you change code
        - Does a live reload of the application
        - Debugging is working with new code
    - Provides a dashboard
        - See all running applications
        - See restarts
        - See logs
    - Show in `Program.cs` in LottoService
- Code Extension
    - Don't waste time in the terminal
    - https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-tye
    - Command Palatte: `Tye: Scaffold Tye Tasks`
- Add redis
    - you can add additional services
        - like docker containers
        - executables
    - see `tye.redis.yaml`
    - If using containers, Tye create proxy containers to route the traffik