# Bessa Transportes — Front-end (FleetExecutive)

Painel de gestão de frota executiva da **Bessa Transportes**, construído em
**Angular 20** (standalone components + signals) a partir dos protótipos HTML
(`Proposta 1/Versao3-Com-Melhorias`) e integrado à API .NET **FleetExecutive**
(`back-end/FleetExecutive.sln`).

## Pré-requisitos

- Node.js `^20.19` || `^22.12` || `^24` (o projeto foi criado com Node 22)
- npm 10+
- API .NET rodando localmente em `http://localhost:5248` (perfil `http` do
  `FleetExecutive.Api`) para os dados reais.

## Como rodar

```bash
npm install
npm start
```

Abra `http://localhost:4200`. O app redireciona para `/login` quando não há
sessão. O dev server usa **proxy** (`proxy.conf.json`) que encaminha todas as
chamadas `/api/*` para a API .NET, evitando CORS em desenvolvimento.

### Build de produção

```bash
npm run build
```

Artefatos em `dist/bessa-frontend/`. O build de produção troca
`environment.ts` por `environment.production.ts` (ver `angular.json` →
`fileReplacements`).

## Integração com o backend

| Item | Detalhe |
|------|---------|
| Base das rotas | `/api/v1/*` |
| Autenticação | JWT Bearer (`POST /api/v1/auth/login`) |
| Multi-tenant | Resolvido pelo **Host** (Finbuckle `WithHostStrategy`). Com o proxy `changeOrigin`, o Host chega como `localhost:5248` e o tenant é resolvido automaticamente. |
| Perfis (RBAC) | `Administrador`, `Operacional`, `Financeiro`, `Fornecedor` |

O login exige que o Host resolva um tenant válido — garanta que o seed local do
backend tenha um tenant apontando para `localhost`. O token de acesso é
curto (15 min); ainda **não há endpoint de refresh** no backend, então um token
expirado leva ao logout (ponto de extensão em `AuthService.restoreSession`).

### Serviços de API já ligados a dados reais

`Pedidos` (lista + detalhe), `Clientes`, `Prestadores`, `Veículos`, `Tarefas`,
`Financeiro` (cobranças/comissões/faturas), `Funil` (orçamentos) e `Agenda`
(disponibilidade) consomem os respectivos endpoints via os serviços em
`src/app/core/api/`.

### Páginas com dados representativos (aguardando endpoint dedicado)

`Dashboard`, `Monitoramento ao vivo`, `NPS`, `Relatórios`,
`Integração de E-mail`, `Novo orçamento (IA)` e `Empresa` estão portadas
fielmente do HTML e usam dados de exemplo expostos como estado do componente.
Cada uma está estruturada para trocar o estado local por uma chamada de serviço
assim que o endpoint correspondente existir.

## Estrutura do projeto

```
src/
  environments/            environment.ts (dev, com proxy) e .production.ts
  styles.scss              design tokens + primitivos globais (portado de _shared.css)
  app/
    app.config.ts          providers (router, HttpClient + interceptors, locale pt-BR)
    app.routes.ts          rotas raiz (login público + shell autenticado, tudo lazy)
    core/
      auth/                AuthService (signals), TokenStorage, guards (auth/guest/role), models
      interceptors/        auth-token (Bearer) e error (401/403/validação → toast)
      api/                 um serviço tipado por recurso + api.models.ts (espelha os DTOs)
      notifications/       NotificationService (fila de toasts)
      models/              PaginatedList<T> (espelha PaginatedList do backend)
    layout/                MainLayout (sidebar + topbar + toast host)
    shared/
      components/          ToastHost
      ui/                  status-badge (mapa status → rótulo/badge)
    features/              uma pasta por página (standalone, lazy-loaded)
```

## Convenções

- **Standalone components** com `ChangeDetectionStrategy.OnPush` e **signals**
  para estado; sem NgModules.
- **Lazy loading** por rota (`loadComponent`) — cada página é um chunk.
- Estado de tela sempre com **loading / error / empty** explícitos.
- Formatação **pt-BR** (moeda `BRL`, datas) via `LOCALE_ID`.
- Estilos: tokens e primitivos no `styles.scss` global; o específico de cada
  página fica no `.scss` do componente (escopado).

## Testes

```bash
npm test
```
