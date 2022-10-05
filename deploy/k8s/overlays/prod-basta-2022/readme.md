# Local Docker Desktop Kubernetes Deployment

## Prep: Install nginx Ingress Controller

### Install

- nginx-ingress repo herunterladen
    ```
    helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
    helm repo update
    ```
- Namespace variable
    ```
    INGRESS_NAMESPACE=ingress-nginx
    ```
- Ingress Controller installieren
    ```
    helm install nginx-ingress ingress-nginx/ingress-nginx \
        --version 4.1.3 \
        --namespace $INGRESS_NAMESPACE \
        --create-namespace \
        --set controller.replicaCount=2 \
        --set controller.nodeSelector."kubernetes\.io/os"=linux \
        --set defaultBackend.nodeSelector."kubernetes\.io/os"=linux
    ```
- Check installation
    ```
    kubectl get pods,services --namespace $INGRESS_NAMESPACE
    ```

### Ingress konfigurieren

- ip adresse auslesen
    ```
    INGRESS_PUBLIC_IP=$(kubectl get services --namespace $INGRESS_NAMESPACE -o jsonpath='{.items[0].status.loadBalancer.ingress[0].ip}')
    echo $INGRESS_PUBLIC_IP
    ```

- Domain update
    - im Domain DNS die folgenden A Einträge machen:
        - <INGRESS_PUBLIC_IP> --> web.lotto.<DOMAIN>
        - <INGRESS_PUBLIC_IP> --> api.lotto.<DOMAIN>

## Environment

- Alle Dienste und services verfügbar
    - Random Number Service
        - not public
    - Lotto Number Service
        - public
        - http://web.lotto.<DOMAIN>
    - Web
        - public
        - http://api.lotto.<DOMAIN>
- Alles auf Port 80

## Deployment

- Apply
    ```bash
    cd deploy/k8s/overlays/prod-basta-2022
    kubectl apply -k .
    ```
- Delete
    ```bash
    kubectl delete -k .
    ```
