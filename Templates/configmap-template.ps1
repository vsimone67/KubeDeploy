kubectl delete secret appsettings-secret-NAMEGOESHERE --namespace NAMESPACEGOESHERE
 
kubectl delete configmap appsettings-NAMEGOESHERE --namespace NAMESPACEGOESHERE

kubectl create secret generic appsettings-secret-NAMEGOESHERE --namespace NAMESPACEGOESHERE --from-file=../appsettings.secrets.json

kubectl create configmap appsettings-NAMEGOESHERE --namespace NAMESPACEGOESHERE --from-file=../appsettings.json