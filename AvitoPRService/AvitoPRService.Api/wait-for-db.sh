#!/bin/sh
set -e

DB_HOST="avitoprservice-db"
DB_PORT=5432
DB_USER="avitopruser"
DB_PASSWORD="avitoprpass"
DB_NAME="avitoprdb"


until nc -z "$DB_HOST" "$DB_PORT"; do
  sleep 2
done


dotnet AvitoPRService.Api.dll --migrate

exec dotnet AvitoPRService.Api.dll