# Local Docker Desktop Kubernetes Deployment

## Environment

- Alle Dienste und services verfügbar
    - Random Number Service
        - not public
    - Lotto Number Service
        - public
        - http://localhost:8081/api/lottonumber
    - Web
        - public
        - http://localhost:8080
- Lotto Number Service und Web haben verschiedene Ports
    - Web muss auf Lotto Service zugreifen

## Deployment

- Apply
    ```bash
    cd deploy/k8s/overlays/local-docker-desktop
    kubectl apply -k .
    ```
- Delete
    ```bash
    kubectl delete -k .
    ```
