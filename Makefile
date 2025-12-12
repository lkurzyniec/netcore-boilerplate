.ONESHELL:
SHELL       = /bin/bash
.SHELLFLAGS = -eu -o pipefail -E -c
.DEFAULT_GOAL := help

help:
	@sed -rn 's/^([a-zA-Z_-]+):.*?## (.*)$$/"\1" "\2"/p' < $(MAKEFILE_LIST) | xargs printf "make %-20s# %s\n"

run: ## Runs the app
	dotnet run --project src/HappyCode.NetCoreBoilerplate.Api

docker-build: ## Builds docker image
	docker build -t netcore-boilerplate:local .

docker-compose: ## Starts the docker-compose
	docker compose up

docker-compose-build: ## Builds and starts the docker-compose
	docker compose up --build

docker-compose-down: ## Stops docker-compose
	docker compose down
