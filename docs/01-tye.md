## Tye Demo

- install tye
    ```bash
    dotnet tool install -g Microsoft.Tye --version "0.11.0-alpha.22111.1"
    ```
- tye help
    ```bash
    tye -h
    ```
- init tye
    ```bash
    tye init lotto-microservices.sln
    ```
    - Erstellt tye config `tye.yaml`
    - wie Docker Compose file
- tye ausführen
    ```bash
    tye run
    ```
    - Startet im watch mode: Code kann angepasst werden
- Code Extension
    - https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-tye
    - Command Palatte: `Tye: Scaffold Tye Tasks`
- Redis hinzufügen
    - siehe `tye.redis.yaml`