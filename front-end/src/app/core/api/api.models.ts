/* =============================================================================
   Modelos da API (camelCase) — espelham os DTOs do backend
   (FleetExecutive.Application, pastas Dtos). Datas chegam como string ISO 8601.
   Enums são mantidos como `string` para evitar acoplamento rígido; os valores
   conhecidos ficam documentados nos union types abaixo.
   ============================================================================= */

export type Iso = string; // DateTimeOffset / DateOnly / TimeOnly serializados

// ------------------------------------------------------------------ Clientes
export type CustomerTipo = 'PessoaFisica' | 'PessoaJuridica' | string;

export interface CustomerListItem {
  id: string;
  nome: string;
  tipo: CustomerTipo;
  ativo: boolean;
  email: string;
  atendenteId: string | null;
}

export interface Customer {
  id: string;
  tipo: CustomerTipo;
  nome: string;
  cpfCnpj: string | null;
  email: string;
  telefone: string | null;
  atendenteId: string | null;
  ativo: boolean;
  observacoes: string | null;
  tags: string[];
  createdAt: Iso;
}

export interface UpsertCustomerRequest {
  tipo: CustomerTipo;
  nome: string;
  cpfCnpj?: string | null;
  email: string;
  telefone?: string | null;
  atendenteId?: string | null;
  observacoes?: string | null;
  tags?: string[];
}

// ------------------------------------------------------------------ Orçamentos
export type QuoteStatus = 'Rascunho' | 'Enviado' | 'Ganho' | 'Perdido' | string;

export interface QuoteService {
  id: string;
  tipoServico: string;
  subtipo: string;
  dataIda: Iso;
  horaIda: string;
  origem: string;
  destino: string;
  dataVolta: Iso | null;
  horaVolta: string | null;
  numPassageiros: number;
  tipoVeiculoPreferido: string | null;
  caracteristicas: string[];
  idiomaRequerido: string | null;
  observacoes: string | null;
}

export interface Quote {
  id: string;
  customerId: string;
  atendenteId: string | null;
  origem: string;
  status: QuoteStatus;
  dataServico: Iso | null;
  valorEstimado: number;
  motivoPerda: string | null;
  createdAt: Iso;
  servicos: QuoteService[];
}

export interface QuoteListItem {
  id: string;
  customerId: string;
  origem: string;
  status: QuoteStatus;
  dataServico: Iso | null;
  valorEstimado: number;
  createdAt: Iso;
}

// -------------------------------------------------------------------- Pedidos
export type StatusComercial = 'AFaturar' | 'Faturado' | 'Concluido' | 'Cancelado' | string;
export type StatusOperacional =
  | 'Agendado' | 'EmDeslocamento' | 'EmServico' | 'Finalizado' | 'AguardandoCheckin' | string;

export interface OrderItem {
  id: string;
  driverId: string | null;
  vehicleId: string | null;
  tipo: string;
  origem: string;
  destino: string;
  dataHoraIda: Iso;
  dataHoraVolta: Iso | null;
  valorServico: number;
  acrescimo: number;
  subtotal: number;
  chaveAcessoCheckin: string;
  checkinEm: Iso | null;
  inicioServicoEm: Iso | null;
  fimServicoEm: Iso | null;
}

export interface OrderAuditLog {
  id: string;
  autorId: string | null;
  tipoEvento: string;
  descricao: string;
  justificativa: string | null;
  criadoEm: Iso;
}

export interface Order {
  id: string;
  quoteId: string | null;
  customerId: string;
  atendenteId: string | null;
  origem: string;
  statusComercial: StatusComercial;
  statusOperacional: StatusOperacional;
  formaPagamento: string | null;
  codigoAceiteTermos: string | null;
  aceiteEm: Iso | null;
  valorTotal: number;
  motivoCancelamento: string | null;
  createdAt: Iso;
  itens: OrderItem[];
  historico: OrderAuditLog[];
}

export interface OrderListItem {
  id: string;
  customerId: string;
  origem: string;
  statusComercial: StatusComercial;
  statusOperacional: StatusOperacional;
  valorTotal: number;
  createdAt: Iso;
}

// --------------------------------------------------------------- Prestadores
export interface DriverLanguage {
  id: string;
  idioma: string;
  nivel: string;
}
export interface DriverDocument {
  id: string;
  tipo: string;
  categoria: string | null;
  numero: string | null;
  validoAte: Iso | null;
  status: string;
}
export interface Driver {
  id: string;
  nome: string;
  tipo: string;
  ativo: boolean;
  telefone: string | null;
  referencia: string | null;
  comissaoPercentual: number;
  indicacao: boolean;
  fonteIndicacao: string | null;
  capacidades: string[];
  idiomas: DriverLanguage[];
  documentos: DriverDocument[];
}
export interface DriverListItem {
  id: string;
  nome: string;
  tipo: string;
  ativo: boolean;
  referencia: string | null;
  telefone: string | null;
  comissaoPercentual: number;
  indicacao: boolean;
}

// ------------------------------------------------------------------ Veículos
export interface Fleet {
  id: string;
  titulo: string;
  descricao: string | null;
  tipo: string;
  categoria: string;
  capacidade: number;
  temWc: boolean;
  temAr: boolean;
  temWifi: boolean;
  temAntt: boolean;
  garagemId: string | null;
  ativo: boolean;
}
export interface Vehicle {
  id: string;
  fleetId: string;
  numeroOrdem: string;
  placa: string;
  status: string;
  garagemId: string | null;
  motoristaHabitualId: string | null;
  disponivelDesde: Iso | null;
  motivoIndisponibilidade: string | null;
  features: string[];
}
export interface VehicleListItem {
  id: string;
  numeroOrdem: string;
  placa: string;
  fleetTitulo: string;
  tipo: string;
  capacidade: number;
  status: string;
  garagemId: string | null;
}

// -------------------------------------------------------------------- Agenda
export interface VehicleBooking {
  orderId: string;
  orderItemId: string;
  dataHoraIda: Iso;
  dataHoraVolta: Iso | null;
  origem: string;
  destino: string;
  emServicoAgora: boolean;
}
export interface VehicleAvailability {
  vehicleId: string;
  numeroOrdem: string;
  placa: string;
  fleetTitulo: string;
  garagemId: string | null;
  statusAdministrativo: string;
  reservas: VehicleBooking[];
}

// ------------------------------------------------------------------- Tarefas
export type TaskPrioridade = 'Baixa' | 'Media' | 'Alta' | string;
export type TaskStatus = 'Aberta' | 'EmAndamento' | 'Concluida' | string;

export interface TaskItem {
  id: string;
  titulo: string;
  descricao: string | null;
  responsavelId: string | null;
  prioridade: TaskPrioridade;
  status: TaskStatus;
  prazo: Iso | null;
  vinculoTipo: string | null;
  vinculoId: string | null;
  createdAt: Iso;
}

// ---------------------------------------------------------------- Financeiro
export interface Commission {
  id: string;
  orderItemId: string;
  tipo: string;
  recebedorTipo: string;
  recebedorId: string;
  percentual: number;
  valor: number;
  status: string;
  pagoEm: Iso | null;
  alterada: boolean;
  justificativaAlteracao: string | null;
}
export interface Charge {
  id: string;
  orderId: string;
  tipo: string;
  status: string;
  origem: string;
  valor: number;
  venceEm: Iso;
  pagoEm: Iso | null;
  impostoRetido: number;
}
export interface Invoice {
  id: string;
  orderId: string;
  status: string;
  geradoEm: Iso | null;
}
