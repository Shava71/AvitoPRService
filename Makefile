# Makefile — AvitoPRService
.PHONY: all build up down logs test test-watch migrate clean

# Цель по умолчанию
all: up

# Сборка Docker-образа
build:
	docker-compose build

# Запуск сервиса
up:
	docker-compose up -d
	@echo "API: http://localhost:8080"
	@echo "pgAdmin: http://localhost:5050"

# Остановка
down:
	docker-compose down

# Логи
logs:
	docker-compose logs -f avitoprservice

# Запуск тестов (в хосте, не в Docker)
test:
	dotnet test ./AvitoPRService/AvitoPRService.Tests/AvitoPRService.Tests.csproj --verbosity normal

# Автозапуск тестов при изменении
test-watch:
	dotnet watch --project .AvitoPRService//AvitoPRService.Tests/AvitoPRService.Tests.csproj test

# Применение миграций 
migrate:
	dotnet ef database update --project .AvitoPRService/AvitoPRService.Infrastructure --startup-project ../AvitoPRService.Api

# Очистка
clean:
	docker-compose down -v
	docker system prune -f

# Нагрузочное тестирование (k6)
load-test:
	k6 run load-test.js

# Линтер
lint:
	dotnet format --verify-no-changes ./AvitoPRService/AvitoPRService.sln