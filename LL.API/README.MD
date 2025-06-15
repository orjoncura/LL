--Docker

docker build -t 677276107921.dkr.ecr.eu-west-2.amazonaws.com/ll_api  .

docker network create ll_network

docker run \
-d --name ll_api \
-p 5000:80 \
-e ASPNETCORE_ENVIRONMENT=Development \
-e ASPNETCORE_URLS=http://+:80 \
-v ~/.microsoft/usersecrets:/home/app/.microsoft/usersecrets \
--network ll_network ll_api

docker run -d --name ll_data -p 1433:1433 -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=***"  --network ll_network ll_data

