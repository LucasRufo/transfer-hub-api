$containerName = "postgres"

# Verifica se o container está rodando
$status = & docker ps --format "{{.Names}}" | Where-Object { $_ -eq $containerName }

if (!$status) {
    docker run --name postgres -p 5432:5432 -v /tmp/database:/var/lib/postgresql/data -e POSTGRES_USER=dev -e POSTGRES_PASSWORD=dev@password123 -d postgres
}

dotnet run --project ../src/TransferHub.Migrations/TransferHub.Migrations.csproj up -s "Server=localhost;Database=transferhub;Port=5432;User Id=dev;Password=dev@password123"