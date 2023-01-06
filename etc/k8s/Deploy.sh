#!/bin/bash

export BASE_PATH=$(realpath "../")

kubectl apply -f https://github.com/jetstack/cert-manager/releases/download/v1.10.0/cert-manager.yaml
kubectl wait --for=condition=Available --timeout=300s apiservice v1.cert-manager.io
envsubst <"certmanager-letsencrypt.yaml" | kubectl apply -f -

envsubst <"traefik-crd.yaml" | kubectl apply -f -
envsubst <"traefik.yaml" | kubectl apply -f -

kubectl apply -f https://download.elastic.co/downloads/eck/1.9.1/crds.yaml
kubectl apply -f https://download.elastic.co/downloads/eck/1.9.1/operator.yaml

kubectl apply -f "elasticsearch.yaml"
kubectl apply -f "kibana.yaml"

export DEMO_ELASTICSEARCH_PASSWORD=$(kubectl get secret elasticsearch-es-elastic-user -o go-template='{{.data.elastic | base64decode}}')

for f in $(ls -x | grep secret.yaml); do
  envsubst <"$f" | kubectl apply -f -
done

for f in $(ls -x | grep persistentvolumeclaim.yaml); do
  envsubst <"$f" | kubectl apply -f -
done

for f in $(ls -x | grep role.yaml); do
  envsubst <"$f" | kubectl apply -f -
done

for f in $(ls -x | grep service.yaml); do
  envsubst <"$f" | kubectl apply -f -
done

for f in $(ls -x | grep middleware.yaml); do
  envsubst <"$f" | kubectl apply -f -
done

for f in $(ls -x | grep ingress.yaml); do
  envsubst <"$f" | kubectl apply -f -
done

for f in $(ls -x | grep deployment.yaml); do
  envsubst <"$f" | kubectl apply -f -
done

for f in $(ls -x | grep job.yaml); do
  envsubst <"$f" | kubectl apply -f -
done
