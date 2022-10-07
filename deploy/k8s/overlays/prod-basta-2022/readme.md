# Local Docker Desktop Kubernetes Deployment

## Prep: Install nginx Ingress Controller

### Install

- nginx-ingress repo herunterladen
    ```
    helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
    helm repo update
    ```
- import images to acr
    ```
    ACR_URL=<ACR_URL>
    SOURCE_REGISTRY=k8s.gcr.io
    CONTROLLER_IMAGE=ingress-nginx/controller
    CONTROLLER_TAG=v1.2.1
    PATCH_IMAGE=ingress-nginx/kube-webhook-certgen
    PATCH_TAG=v1.1.1
    DEFAULTBACKEND_IMAGE=defaultbackend-amd64
    DEFAULTBACKEND_TAG=1.5

    az acr import --name $ACR_URL --source $SOURCE_REGISTRY/$CONTROLLER_IMAGE:$CONTROLLER_TAG --image $CONTROLLER_IMAGE:$CONTROLLER_TAG
    az acr import --name $ACR_URL --source $SOURCE_REGISTRY/$PATCH_IMAGE:$PATCH_TAG --image $PATCH_IMAGE:$PATCH_TAG
    az acr import --name $ACR_URL --source $SOURCE_REGISTRY/$DEFAULTBACKEND_IMAGE:$DEFAULTBACKEND_TAG --image $DEFAULTBACKEND_IMAGE:$DEFAULTBACKEND_TAG
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
        --set controller.image.registry=$ACR_URL \
        --set controller.image.image=$CONTROLLER_IMAGE \
        --set controller.image.tag=$CONTROLLER_TAG \
        --set controller.image.digest="" \
        --set controller.admissionWebhooks.patch.nodeSelector."kubernetes\.io/os"=linux \
        --set controller.service.annotations."service\.beta\.kubernetes\.io/azure-load-balancer-health-probe-request-path"=/healthz \
        --set controller.admissionWebhooks.patch.image.registry=$ACR_URL \
        --set controller.admissionWebhooks.patch.image.image=$PATCH_IMAGE \
        --set controller.admissionWebhooks.patch.image.tag=$PATCH_TAG \
        --set controller.admissionWebhooks.patch.image.digest="" \
        --set defaultBackend.nodeSelector."kubernetes\.io/os"=linux \
        --set defaultBackend.image.registry=$ACR_URL \
        --set defaultBackend.image.image=$DEFAULTBACKEND_IMAGE \
        --set defaultBackend.image.tag=$DEFAULTBACKEND_TAG \
        --set defaultBackend.image.digest=""
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
        - <INGRESS_PUBLIC_IP> --> web.lotto.<DOMAIN_NAME>
        - <INGRESS_PUBLIC_IP> --> api.lotto.<DOMAIN_NAME>

## Environment

- Alle Dienste und services verfügbar
    - Random Number Service
        - not public
    - Lotto Number Service
        - public
        - http://web.lotto.<DOMAIN_NAME>
    - Web
        - public
        - http://api.lotto.<DOMAIN_NAME>
- Alles auf Port 80

## Deployment

- In den Manifest-Dateien die URL inkl. Domain anpassen
    - configmap-web.yaml
    - ingress-lotto-service.yaml
    - ingress-web.yaml
- Apply
    ```bash
    cd deploy/k8s/overlays/prod-basta-2022
    kubectl apply -k .
    ```
- Delete
    ```bash
    kubectl delete -k .
    ```
