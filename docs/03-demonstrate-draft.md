# Draft

<https://draft.sh>

- nicht bevorzugt: draft aufrufen über `az aks draft -h`
    - klappt noch nicht so ganz
- draft selbst installieren
    - draft laden und installieren
        ```bash
        DRAFT_VERSION="0.0.24" # draft version
        DRAFT_ARCH="amd64" # draft archicture
        curl -O -L "https://github.com/Azure/draft/releases/download/v${DRAFT_VERSION}/draft-linux-$DRAFT_ARCH"
        mv draft-linux-$DRAFT_ARCH draft && chmod +x draft && sudo mv draft /usr/bin/draft
        ```
- draft ausführen
    ```bash
    draft
    ```
- draft zum Erstellen von Dockerfile und Manifests benutzen
    - in Projekt wechseln
    - draft `create help` ausführen
        ```bash
        draft create -h
        ```
    - draft für Projekt ausführen
        ```bash
        draft create
        ```
    - Instructions folgen
- draft für alle Projekte ausführen mit `scripts/draft-create-all.sh`
    ```bash
    ./scripts/draft-create-all.sh
    ```

