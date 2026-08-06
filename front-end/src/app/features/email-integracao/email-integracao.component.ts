import { ChangeDetectionStrategy, Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

interface EmailRecebido {
  inicial: string;
  bg: string;
  de: string;
  assunto: string;
  tempo: string;
  naoLido: boolean;
  classificacao: string;
}

/**
 * Integração de E-mail (portado de email-integracao.html). Mostra o status da
 * conexão IMAP e a caixa de entrada classificada por IA. Dados representativos
 * até o ConfiguracoesController/serviço de e-mail expor os endpoints.
 */
@Component({
  selector: 'app-email-integracao',
  imports: [RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './email-integracao.component.html',
  styleUrl: './email-integracao.component.scss',
})
export class EmailIntegracaoComponent {
  protected readonly conectado = signal(true);
  protected readonly contaEmail = 'comercial@fleetexecutivetransportes.com.br';

  protected readonly emails = signal<EmailRecebido[]>([
    { inicial: 'I', bg: 'linear-gradient(135deg,#1d4ed8,#3b82f6)', de: 'INSPER — insper@email.com.br', assunto: 'Solicitação de orçamento — 45 pessoas — SP × Campos do Jordão — 15/08/2026', tempo: 'há 18 min', naoLido: true, classificacao: 'Novo orçamento' },
    { inicial: 'L', bg: 'linear-gradient(135deg,#065f46,#10b981)', de: 'Luminus Seguros — financeiro@luminus.com.br', assunto: 'Frete executivo — 8 pessoas — São Paulo × Rio de Janeiro — 22/07/2026', tempo: 'há 1h', naoLido: true, classificacao: 'Novo orçamento' },
    { inicial: 'D', bg: 'linear-gradient(135deg,#7c3aed,#a78bfa)', de: 'Doremus Alimentos — compras@doremus.com.br', assunto: 'Confirmação de agenda do transporte de amanhã', tempo: 'há 3h', naoLido: false, classificacao: 'Operacional' },
  ]);
}
