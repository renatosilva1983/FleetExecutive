# Bessa Sistema

Monorepo do sistema real da Bessa Transportes (substitui o Fretatech). Ver a documentação de
planejamento completa em `C:\Projetos\Bessa\Estrutura\` (arquivos `01` a `13` + `ESTRUTURA-PROJETO.md`)
antes de mexer em qualquer módulo — aqueles arquivos têm o contexto de negócio, o modelo de dados
proposto, RBAC e o roadmap; este README só orienta a rodar o código.

## Estrutura

```
backend/    Solução .NET 9 (Clean Architecture) — Bessa.sln
frontend/   Workspace Nx/Angular — 2 apps (bessa-empresa1, bessa-empresa2) + libs compartilhadas
infra/      env por empresa, Nginx (reverse proxy de produção)
docs/       ADRs e documentação técnica (contratos de API, módulos)
```

Este projeto **não usa Docker** — backend e frontend rodam diretamente no sistema operacional
(dev ou servidor). Ver `Estrutura/Requisitos-Instalacao-Servidor.pdf` para o que precisa estar
instalado no servidor (Windows e Linux).

## Rodando localmente

### Pré-requisitos
- .NET SDK 9.0.x
- Node.js 22.x LTS + npm 10.x
- PostgreSQL 16.x rodando localmente (`localhost:5432`), com os bancos `bessa_master`,
  `bessa_empresa1`, `bessa_empresa2` criados

### Backend
```
cd backend
dotnet build
dotnet ef database update --project src/Bessa.Infrastructure --startup-project src/Bessa.Api --context MasterDbContext
dotnet ef database update --project src/Bessa.Infrastructure --startup-project src/Bessa.Api --context BessaDbContext
dotnet run --project src/Bessa.Api
```

### Frontend
```
cd frontend
npm install
npx nx serve bessa-empresa1   # ou bessa-empresa2
```

## Status

Ver `docs/adr/` para o histórico de decisões módulo a módulo. Bloco **v2 (núcleo comercial e
operacional)** do roadmap concluído: Clientes, Veículos/Frotas, Prestadores/Motoristas,
Orçamentos/Funil/Pedidos, Agenda de Veículos, Financeiro (Comissões/Cobranças/Faturas) e Tarefas —
todos com backend completo (Domain/Application/Infrastructure/Api + migrations). Frontend ainda
no scaffold, sem telas implementadas. Próximo bloco: v3 (Integração de E-mail, sugestão de preço
por IA, Monitoramento ao Vivo, NPS) — ver `Estrutura/13-deploy-multitenant-e-roadmap.md`.
