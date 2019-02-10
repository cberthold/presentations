#!/bin/sh
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Password123!' -p 1433:1433 -d microsoft/mssql-server-linux:latest