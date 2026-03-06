# CurrencyService

Микросервисная система работы с курсами валют пользователя (Backend, .NET 8, CQRS, Clean Architecture)

## 📋 Требования

- Docker и Docker Compose
- Visual Studio 2022
- .NET SDK 8.0
- Redis Insight (опционально)

## 🚀 Порядок запуска

### 1. Запуск инфраструктуры

Откройте терминал и перейдите в корень решения:

	docker-compose up -d

### 2. Применение миграций базы данных

Запустите проект CurrencyService.DatabaseMigrator для применения миграций.

### 3. Запуск сервисов

Запустите отладку в Visual Studio 2022 (в Docker) для следующих проектов:

- CurrencyService.FinanceService.Api
- CurrencyService.UserService.Api
- CurrencyService.ApiGateway
- CurrencyService.BackgroundWorker

### 4. Проверка работы API

- Откройте браузер и перейдите по адресу:

		http://localhost:5157/swagger

- Выполните регистрацию пользователя
- Полученный токен введите в поле Authorize
- Протестируйте доступные endpoints

## 🗄️ Работа с миграциями

### Создание новой миграции

Откройте терминал и перейдите в корень решения
Выполните команду:

	dotnet ef migrations add <название_миграции>

### Применение миграций

Запустите проект CurrencyService.DatabaseMigrator

## 🔍 Проверка отозванных токенов в Redis

### Через Redis Insight

- Откройте браузер и перейдите по адресу:

		http://localhost:5540

- Добавьте подключение к базе данных:

		Host: redis
		Port: 6379

- Просмотрите ключи с отозванными токенами

## 🛠️ Полезные команды

	# Остановка всех контейнеров
	docker-compose down
	
	# Просмотр логов
	docker-compose logs -f
	
	# Пересборка контейнеров
	docker-compose up -d --build
	
	# Запуск конкретного сервиса
	docker-compose up -d <service_name>

## 📝 Примечания

- Убедитесь, что порты 5157, 5540 и 6379 не заняты
- Для работы с базой данных используется PostgreSQL
- Redis используется для кэширования и хранения отозванных токенов
- Все сервисы запускаются в Docker контейнерах