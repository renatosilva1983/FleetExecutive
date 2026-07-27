# =============================================================================
#  Dockerfile da FleetExecutive.Api  (.NET 9.0 Web API)
# -----------------------------------------------------------------------------
#  Usamos "multi-stage build" (build em vários estágios). A ideia:
#    - ESTÁGIO 1 (build): tem o SDK completo do .NET (compilador, restore, etc.).
#      É pesado (~800 MB), mas só existe durante a construção da imagem.
#    - ESTÁGIO 2 (final): tem apenas o runtime do ASP.NET (bem menor) e recebe
#      somente os arquivos JÁ compilados. É essa imagem enxuta que vai pra produção.
#
#  Assim a imagem final não carrega o compilador nem o código-fonte: fica menor,
#  mais rápida de baixar e com menos superfície de ataque (mais segura).
# =============================================================================


# =============================================================================
#  ESTÁGIO 1 — BUILD  (compila e publica a aplicação)
# =============================================================================

# FROM define a imagem-base deste estágio. Aqui pegamos a imagem oficial da
# Microsoft com o SDK do .NET 9.0 (SDK = tudo que precisa para COMPILAR).
# O "AS build" dá um apelido a este estágio, para referenciá-lo mais tarde.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# WORKDIR cria (se não existir) e entra num diretório DENTRO do contêiner —
# equivale a "mkdir -p /app && cd /app". É uma pasta nova e vazia do contêiner,
# sem relação com nenhuma pasta da sua máquina. Todos os comandos seguintes
# rodam a partir daqui, e os destinos de COPY são relativos a ele.
WORKDIR /app

# ---------------------------------------------------------------------------
#  Passo 1: copiar SÓ os arquivos de projeto (.csproj) e restaurar pacotes.
#
#  Por que copiar os .csproj antes de todo o resto do código?
#  Docker guarda "cache" de cada camada. Se os .csproj não mudarem, o Docker
#  reaproveita o cache do "dotnet restore" (que é o passo lento: baixa NuGet).
#  Como o código-fonte muda muito mais do que a lista de dependências,
#  isolar o restore aqui deixa os builds seguintes MUITO mais rápidos.
# ---------------------------------------------------------------------------

# COPY <origem-no-host> <destino-no-contêiner>.
# Copiamos cada .csproj mantendo a mesma estrutura de pastas do projeto,
# porque os projetos referenciam uns aos outros por caminho relativo
# (ex.: ..\FleetExecutive.Domain\FleetExecutive.Domain.csproj).
COPY FleetExecutive.sln .
COPY src/FleetExecutive.Api/FleetExecutive.Api.csproj                     src/FleetExecutive.Api/
COPY src/FleetExecutive.Application/FleetExecutive.Application.csproj      src/FleetExecutive.Application/
COPY src/FleetExecutive.Infrastructure/FleetExecutive.Infrastructure.csproj src/FleetExecutive.Infrastructure/
COPY src/FleetExecutive.Domain/FleetExecutive.Domain.csproj               src/FleetExecutive.Domain/
COPY src/FleetExecutive.Shared/FleetExecutive.Shared.csproj               src/FleetExecutive.Shared/

# RUN executa um comando durante a construção da imagem.
# "dotnet restore" baixa todos os pacotes NuGet necessários. Apontamos direto
# para o .csproj da API; o restore segue as referências e restaura a cadeia
# inteira (Application, Infrastructure, Domain, Shared).
RUN dotnet restore src/FleetExecutive.Api/FleetExecutive.Api.csproj

# ---------------------------------------------------------------------------
#  Passo 2: copiar o restante do código-fonte e publicar.
# ---------------------------------------------------------------------------

# Agora sim copiamos TODO o resto do repositório para dentro do contêiner.
# (O .dockerignore garante que lixo como bin/, obj/ e .git fiquem de fora.)
COPY . .

# "dotnet publish" compila em modo Release e gera os arquivos prontos para
# rodar dentro da pasta /app/publish.
#   -c Release        -> configuração otimizada (sem símbolos de debug pesados)
#   -o /app/publish   -> pasta de saída dos arquivos publicados
#   --no-restore      -> pula o restore (já fizemos acima, aproveitando o cache)
RUN dotnet publish src/FleetExecutive.Api/FleetExecutive.Api.csproj -c Release -o /app/publish --no-restore


# =============================================================================
#  ESTÁGIO 2 — FINAL  (imagem enxuta que realmente vai rodar)
# =============================================================================

# Aqui usamos a imagem de RUNTIME do ASP.NET (só o necessário para EXECUTAR,
# sem o compilador). É bem menor que a imagem do SDK usada acima.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

# Copiamos apenas o resultado da publicação vindo do estágio "build".
# "--from=build" é o pulo do gato do multi-stage: pegamos só os binários
# prontos, deixando SDK e código-fonte para trás.
COPY --from=build /app/publish .

# ---------------------------------------------------------------------------
#  Configuração de rede e ambiente
# ---------------------------------------------------------------------------

# A partir do .NET 8, a imagem oficial roda como usuário SEM privilégios e
# escuta na porta 8080 por padrão (portas abaixo de 1024 exigiriam root).
# Deixamos isso explícito para ficar claro em qual porta o Kestrel vai ouvir.
ENV ASPNETCORE_HTTP_PORTS=8080

# EXPOSE é apenas DOCUMENTAÇÃO: informa que o contêiner escuta na 8080.
# Ele não publica a porta sozinho — na hora de rodar você mapeia com -p, ex.:
#   docker run -p 5000:8080 FleetExecutive-api
# (host 5000 -> contêiner 8080; acesse em http://localhost:5000)
EXPOSE 8080

# ENTRYPOINT define o comando que roda quando o contêiner inicia.
# Iniciamos a aplicação executando a DLL publicada com o runtime do .NET.
ENTRYPOINT ["dotnet", "FleetExecutive.Api.dll"]
