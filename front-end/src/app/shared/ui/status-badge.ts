/**
 * Mapeamento status → (rótulo amigável + classe de badge). Centralizado para
 * manter consistência visual entre Pedidos, Funil, Financeiro e Tarefas.
 */
export interface BadgeView {
  label: string;
  css: string; // classe(s) do .badge (ver styles.scss)
}

const FALLBACK: BadgeView = { label: '—', css: 'badge-info' };

const STATUS_COMERCIAL: Record<string, BadgeView> = {
  AFaturar: { label: 'A Faturar', css: 'badge-warn' },
  Faturado: { label: 'Faturado', css: 'badge-info' },
  Concluido: { label: 'Concluído', css: 'badge-success' },
  EmAndamento: { label: 'Em andamento', css: 'badge-info' },
  Cancelado: { label: 'Cancelado', css: 'badge-danger' },
};

const STATUS_FUNIL: Record<string, BadgeView> = {
  Rascunho: { label: 'Rascunho', css: 'badge-info' },
  Novo: { label: 'Novo', css: 'badge-info' },
  Enviado: { label: 'Enviado', css: 'badge-purple' },
  EmNegociacao: { label: 'Em negociação', css: 'badge-warn' },
  Ganho: { label: 'Ganho', css: 'badge-success' },
  Perdido: { label: 'Perdido', css: 'badge-danger' },
};

const STATUS_TAREFA: Record<string, BadgeView> = {
  Aberta: { label: 'Aberta', css: 'badge-info' },
  EmAndamento: { label: 'Em andamento', css: 'badge-warn' },
  Concluida: { label: 'Concluída', css: 'badge-success' },
};

const STATUS_FINANCEIRO: Record<string, BadgeView> = {
  Pendente: { label: 'Pendente', css: 'badge-warn' },
  Cobranca: { label: 'Cobrança', css: 'badge-danger' },
  Paga: { label: 'Paga', css: 'badge-success' },
  Pago: { label: 'Pago', css: 'badge-success' },
  Recebido: { label: 'Recebido', css: 'badge-success' },
  Cancelada: { label: 'Cancelada', css: 'badge-danger' },
  Gerada: { label: 'Gerada', css: 'badge-info' },
};

const STATUS_VEICULO: Record<string, BadgeView> = {
  Disponivel: { label: 'Disponível', css: 'badge-success' },
  EmServico: { label: 'Em serviço', css: 'badge-info' },
  Manutencao: { label: 'Manutenção', css: 'badge-warn' },
  Indisponivel: { label: 'Indisponível', css: 'badge-danger' },
};

export function comercialBadge(status: string): BadgeView {
  return STATUS_COMERCIAL[status] ?? { label: status, css: FALLBACK.css };
}
export function funilBadge(status: string): BadgeView {
  return STATUS_FUNIL[status] ?? { label: status, css: FALLBACK.css };
}
export function tarefaBadge(status: string): BadgeView {
  return STATUS_TAREFA[status] ?? { label: status, css: FALLBACK.css };
}
export function financeiroBadge(status: string): BadgeView {
  return STATUS_FINANCEIRO[status] ?? { label: status, css: FALLBACK.css };
}
export function veiculoBadge(status: string): BadgeView {
  return STATUS_VEICULO[status] ?? { label: status, css: FALLBACK.css };
}
