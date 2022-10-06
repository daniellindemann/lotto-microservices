#!/bin/sh

RESOURCE_GROUP="${1:-dli-aks-demo}"
VNET_ADDRESS_PREFIX="${2:-10.100.0.0/16}"
VNET_ADRESS_SUBNET_PREFIX="${3:-10.100.240.0/24}"
AKS_SERVICE_CIDR="${4:-10.101.0.0/24}"
AKS_DNS_SERVICE_IP="${5:-10.101.0.10}"
ACR_SKU="${6:-Standard}"

# local variables
AKS_RANDOM=$(shuf -i 1-100000 -n 1)
REGION_NAME=germanywestcentral
RESOURCE_GROUP=dli-aks-$AKS_RANDOM
SUBNET_NAME=dli-aks-subnet-$AKS_RANDOM
VNET_NAME=dli-aks-vnet-$AKS_RANDOM
AKS_CLUSTER_NAME=dli-aks-$AKS_RANDOM
ACR_NAME=acr$AKS_RANDOM

# resource group
az group create \
    --name $RESOURCE_GROUP \
    --location $REGION_NAME

# network
az network vnet create \
    --resource-group $RESOURCE_GROUP \
    --location $REGION_NAME \
    --name $VNET_NAME \
    --address-prefixes $VNET_ADDRESS_PREFIX \
    --subnet-name $SUBNET_NAME \
    --subnet-prefixes $VNET_ADRESS_SUBNET_PREFIX
SUBNET_ID=$(az network vnet subnet show \
    --resource-group $RESOURCE_GROUP \
    --vnet-name $VNET_NAME \
    --name $SUBNET_NAME \
    --query id -o tsv)

# create cluster
VERSION=$(az aks get-versions \
    --location $REGION_NAME \
    --query 'orchestrators[?!isPreview] | [-1].orchestratorVersion' \
    --output tsv)
az aks create \
    --resource-group $RESOURCE_GROUP \
    --name $AKS_CLUSTER_NAME \
    --tags 'Business Hours Start=08:00' 'Business Hours End=18:00' \
    --vm-set-type VirtualMachineScaleSets \
    --node-count 2 \
    --load-balancer-sku standard \
    --location $REGION_NAME \
    --kubernetes-version $VERSION \
    --network-plugin azure \
    --network-policy calico \
    --vnet-subnet-id $SUBNET_ID \
    --service-cidr $AKS_SERVICE_CIDR \
    --dns-service-ip $AKS_DNS_SERVICE_IP \
    --docker-bridge-address 172.17.0.1/16 \
    --generate-ssh-keys

# acr
az acr create \
    --resource-group $RESOURCE_GROUP \
    --location $REGION_NAME \
    --name $ACR_NAME \
    --sku $ACR_SKU

# connect acr to aks
az aks update \
    --name $AKS_CLUSTER_NAME \
    --resource-group $RESOURCE_GROUP \
    --attach-acr $ACR_NAME
