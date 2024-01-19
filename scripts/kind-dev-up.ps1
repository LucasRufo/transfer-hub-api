$containerName = "kind-control-plane"

# Verifica se o Kind está rodando
$status = & docker ps --format "{{.Names}}" | Where-Object { $_ -eq $containerName }

if (!$status) {
    kindxt --create-cluster --nginx-ingress --postgres
}

kubectl config use-context kind-kind

kubectl create namespace transfer-hub

helm uninstall transfer-hub -n transfer-hub

docker build -t transfer-hub/transfer-hub-api:latest -f ../src/TransferHub.API/Dockerfile --no-cache ../

kind load docker-image transfer-hub/transfer-hub-api:latest

dotnet run --project ../src/TransferHub.Migrations/TransferHub.Migrations.csproj up -s "Server=localhost;Database=transferhub;Port=5432;User Id=desenv;Password=P@ssword123"

helm install transfer-hub ../helm/transfer-hub-api/ --debug --wait `
    -n transfer-hub `
    -f ../helm/transfer-hub-api/values.local.yaml

