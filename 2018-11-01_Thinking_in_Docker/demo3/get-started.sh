#!/bin/sh
mkdir ./myapp
cd ./myapp
curl -LO https://raw.githubusercontent.com/bitnami/bitnami-docker-rails/master/docker-compose.yml
docker-compose up
