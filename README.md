# AvitoPRService: Сервис назначения ревьюеров для Pull Request'ов

## Тестовое задание Avito Backend Autumn 2025\
Реализация микросервиса для автоматического назначения ревьюверов на PR,
управления командами и пользователями.

Стек: **.NET 9**, **EF Core**, **PostgreSQL**, **Docker**, **xUnit**, **Moq**,
**k6**, **OpenAPI**\
Автор: *\Ериков Илья*\

---

## Содержание

1.  [Запуск проекта](#запуск-проекта)
2.  [API эндпоинты](#api-эндпоинты)
3.  [Тестирование](#тестирование)
4.  [Нагрузочное тестирование](#нагрузочное-тестирование-k6)
5.  [Дополнительные задания](#дополнительные-задания)
6.  [Преимущества решения](#преимущества-решения)
7.  [Трудности и допущения](#трудности-и-допущения)
8.  [Диаграмма базы данных](#диаграмма-базы-данных)
9.  [SQL-скрипты](SQL-скрипты)

---
## Структура проекта:
```
AvitoPRService/
├── AvitoPRService.Api/               # Веб-API слой
│   ├── Contracts/                    # Контракты API (request/response)
│   ├── Controllers/                  # HTTP-контроллеры
│   ├── Dto/                          # DTO-модели, используемые API
│   ├── Extensions/                   # Расширения для DI, middleware, конфигурации
│   ├── Mapper/                       # Профили Mapper
│   ├── NSwag/                        # NSwag генерация клиента/спеки из openapi.yml
│   ├── sql/                          # SQL-скрипты (скрипт триггера максимального количества reviewer)
│   ├── appsettings*.json             # Конфигурация окружений
│   ├── Dockerfile                    # Docker-образ сервиса
│   └── Program.cs                    # Точка входа веб-приложения
│
├── AvitoPRService.Application/       # Application слой (бизнес-логика)
│   ├── Dto/                          # DTO уровня приложения
│   ├── Mapper/                       # Мапперы между Entity <-> DTO
│   └── Services/                     # Use-case’ы и сервисы бизнес-логики
│
├── AvitoPRService.Domain/            # Domain слой (центральная модель)
│   ├── Entities/                     # Доменные сущности
│   ├── Exception/                    # Доменные исключения
│   ├── Repositories/                 # Интерфейсы репозиториев
│   └── ValueObject/                  # Value Objects
│
├── AvitoPRService.Infrastructure/    # Infrastructure слой (реализация портов)
│   ├── Data/                         # DbContext и конфигурации EF Core
│   ├── Migrations/                   # EF Core миграции
│   └── Repositories/                 # Реализация доменных репозиториев
│
├── AvitoPRService.Tests/             # Тесты (Unit + Integration)
│   ├── Helpers/                      # Общие утилиты тестов
│   ├── Integration/                  # Интеграционные тесты
│   └── Unit/                         # Модульные тесты
│
├── docker/                           # Docker-related скрипты/конфиги
├── docs/                             # Документация проекта
├── sql/                              # Скрипты БД (например, инициализация)
├── pgadmin/                          # Конфигурация PgAdmin
├── docker-compose.yml                # Поднятие сервисов окружения
└── AvitoPRService.sln                # Solution файла проекта
```

---

## Запуск проекта

### Требования

-   Docker
-   Docker Compose
-   make

### Шаг 1: Клонирование
``` bash
git clone https://github.com/yourname/AvitoPRService.git](https://github.com/Shava71/AvitoPRService.git)
cd AvitoPRService
```

### Шаг 2: Запуск одной командой
``` bash
make up
```

После запуска доступны:\
- API (app-service): http://localhost:8080\
- OpenAPI / Swagger UI: http://localhost:8080\
- PostgreSQL: http://localhost:5432\
- pgAdmin: http://localhost:5050 (admin@admin.com / admin)

### Шаг 3: Применение миграций\
``` bash
make migrate
```
Данный пункт можно пропустить, так как проект уже имеет миграцию с автоматическим внедрением

### Шаг 4: Пример запроса

``` bash
curl http://localhost:8080/team/add -X POST -H "Content-Type: application/json" -d '{
  "team_name": "test-team",
  "members": [
    {"user_id": "u1", "username": "Alice", "is_active": true},
    {"user_id": "u2", "username": "Bob", "is_active": true}
  ]
}'
```

### Полезные Make команды

| Команда           | Описание                              |
|-------------------|----------------------------------------|
| `make build`      | Сборка Docker-образа                   |
| `make logs`       | Показывает логи API                    |
| `make test`       | Запуск интеграционных тестов           |
| `make test-watch` | Автоперезапуск тестов                  |
| `make load-test`  | Нагрузочное тестирование               |
| `make down`       | Остановка сервисов                     |
| `make clean`      | Полная очистка volumes + images        |
| `make lint`       | Запуск линтера без изменений (только отображение ошибок      |

---

### API эндпоинты

| Эндпоинт                       | Метод | Описание                                   |
|--------------------------------|--------|---------------------------------------------|
| `/team/add`                    | POST   | Создать команду                             |
| `/team/get?team_name=...`      | GET    | Получить команду                            |
| `/users/setIsActive`           | POST   | Активировать/деактивировать пользователя    |
| `/pullRequest/create`          | POST   | Создать PR + назначить ревьюверов          |
| `/pullRequest/merge`           | POST   | Merge PR (идемпотентно)                    |
| `/pullRequest/reassign`        | POST   | Переназначить ревьювера                    |
| `/users/getReview?...`         | GET    | Получить PR'ы на ревью                     |
| `/stats`                       | GET    | Статистика                                  |
| `/team/deactivateUsers`        | POST   | Массовая деактивация пользователей          |



## Тестирование

Интеграционные тесты (xUnit + `WebApplicationFactory`):
``` bash
make test
```
InMemoryDatabase --- быстрые и изолированные тесты\
### Unit-тесты (PullRequestService)

| Тест                                         | Что проверяет                                                        |
|----------------------------------------------|----------------------------------------------------------------------|
| CreateAsync_ShouldAssignTwoReviewers...      | PR получает **ровно двух активных ревьюверов**, исключая автора     |
| ReassignReviewerAsync (данный тест закомментирован из-за его неидемпотентности          | Переназначение ревьювера на другого активного пользователя           |

### Интеграционные тесты

| Класс / Тест                          | Что проверяет                                                    |
|---------------------------------------|------------------------------------------------------------------|
| CreatePR_ShouldReturn201...           | Создание PR + автоматическое назначение ревьюверов              |
| ReassignPR_ShouldReturn200...         | Переназначение ревьювера: заменяет неактивного на активного     |
| SetupTeamAsync (helper)               | Создание команды с 3 участниками                                 |
| CreatePRAsync (helper)                | Создание PR через API                                            |
| DeactivateUserAsync (helper)          | Деактивация пользователя                                         |
| DeactivateUser_ShouldReturn200...     | Деактивация пользователя → is_active = false                    |
| ReactivateUser_ShouldReturn200...     | Активация пользователя → is_active = true                       |
| SetupTeamAsync                        | Создание команды для теста                                       |
| GetUserReviews (закомментирован)       | (Проверка PR на ревью конкретному пользователю)                  |
| AddTeam_ShouldReturn201               | Добавление команды                                               |
| GetTeam_ShouldReturn200               | Получение команды по имени                                       |


## Нагрузочное тестирование k6
``` bash
make load-test
```
### Вывод:
```bash
script: load-test.js
        output: -

     scenarios: (100.00%) 1 scenario, 5 max VUs, 5m50s max duration (incl. graceful stop):
              * default: Up to 5 looping VUs for 5m20s over 3 stages (gracefulRampDown: 30s, gracefulStop: 30s)

INFO[0321] [k6-reporter v3.0.3] Generating HTML summary report, with theme: default  source=console
     ✓ PR created
     ✗ deactivate ok
      ↳  99% — ✓ 1528 / ✗ 1
     ✓ deactivate < 100ms

     █ setup

       ✓ team created

   ✓ checks.........................: 99.97% ✓ 4587     ✗ 1   
     data_received..................: 906 kB 2.8 kB/s
     data_sent......................: 712 kB 2.2 kB/s
     http_req_blocked...............: avg=9.95µs  min=1µs    med=8µs    max=1.13ms  p(90)=13µs    p(95)=15µs   
     http_req_connecting............: avg=767ns   min=0s     med=0s     max=840µs   p(90)=0s      p(95)=0s     
   ✓ http_req_duration..............: avg=8.87ms  min=1.32ms med=7.52ms max=99.47ms p(90)=14.95ms p(95)=17.06ms
       { expected_response:true }...: avg=8.87ms  min=1.32ms med=7.51ms max=99.47ms p(90)=14.94ms p(95)=17.05ms
     ✓ { scenario:deactivate }......: avg=5.24ms  min=1.32ms med=5.11ms max=34ms    p(90)=7.37ms  p(95)=8.29ms 
   ✓ http_req_failed................: 0.03%  ✓ 1        ✗ 3058
     http_req_receiving.............: avg=81.24µs min=8µs    med=67µs   max=3.26ms  p(90)=131µs   p(95)=151µs  
     http_req_sending...............: avg=37.75µs min=4µs    med=33µs   max=2.15ms  p(90)=55µs    p(95)=64µs   
     http_req_tls_handshaking.......: avg=0s      min=0s     med=0s     max=0s      p(90)=0s      p(95)=0s     
     http_req_waiting...............: avg=8.75ms  min=1.26ms med=7.39ms max=98.98ms p(90)=14.79ms p(95)=16.87ms
     http_reqs......................: 3059   9.527397/s
     iteration_duration.............: avg=1.01s   min=1s     med=1.01s  max=1.11s   p(90)=1.02s   p(95)=1.02s  
     iterations.....................: 1529   4.762141/s
     vus............................: 1      min=1      max=5 

running (5m21.1s), 0/5 VUs, 1529 complete and 0 interrupted iterations
default ✓ [======================================] 0/5 VUs  5m20s
```

### Результаты (5 минут, \~3000 запросов):

| Параметр              | Цель        | Фактический результат      |
|-----------------------|-------------|-----------------------------|
| RPS                   | ≥ 5         | ~9.5 req/s                  |
| p95                   | < 300 мс    | 17.06 мс                    |
| Успешность            | ≥ 99.9%     | 99.97% (1 ошибка на 3059)   |
| Массовая деактивация | < 100 мс    | p95 = 8.29 мс               |

SLI: **выполнены**\
Отчёт: *k6-report.html*

---

## Дополнительные задания

| Задание                | Статус | Описание                                |
|------------------------|--------|------------------------------------------|
| Статистика             | Сделано | Полная аналитика `/stats`                |
| Нагрузка               | Сделано | SLI выполнены                             |
| Массовая деактивация   | Сделано | POST `/team/deactivateUsers`              |
| Интеграционные тесты   | Сделано | Прокиданы HttpClients на endpoint'ы       |
| Линтер                 | Сделано | `dotnet format` в Makefile                |

---

## Преимущества решения

- Почти полное покрытие API тестами
- SLI выполнены (p95=17ms, 99.97% success)
- Полная статистика PR и ревьюверов
- Идемпотентные `create` и `merge`
- Docker + Makefile (запуск в 1 команду)
- Быстрые InMemory тесты
- Массовая деактивация по UserIds (гибче, чем по team_name)
- Выполнены обязательные и дополнительные задачи
- Подход разработки: DDD
- Соблюдение подходов проектирования и паттернов: SOLID, KISS, DRY, YAGNI, GRASP, UnitOfWork

---

## Трудности и допущения

-   create_pull_request и reassign неидемпотентны (берутся 0-2 рандомных ревьювера, поэтому невозможно быть уверенным, какой пользователь будет назначен в качестве reviewer)
-   тесты не проверяют конкретных ревьюверов, только:
    - статус\
    - количество\
    - что не автор\
-   нагрузочные тесты дали 1 ошибку на ~3000 запросов
-   деактивация реализована по UserIds:  логичнее, чем по всей команде

---
## Диаграмма базы данных
![Диаграмма][1]

[1]: ./docs/db.png

---

## SQL-скрипты
### Триггер проверки количество reviewer в pull_request (максимум 2 ревьюера может быть)
```SQL
CREATE OR REPLACE FUNCTION check_reviewer_limit()
RETURNS trigger AS $$
BEGIN
    IF (
        SELECT COUNT(*) FROM reviewers
        WHERE pull_request_id = NEW.pull_request_id
    ) >= 2 THEN
        RAISE EXCEPTION 'Too many reviewers for PR';
    END IF;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_reviewer_limit
BEFORE INSERT ON reviewers
FOR EACH ROW
EXECUTE FUNCTION check_reviewer_limit();
```

