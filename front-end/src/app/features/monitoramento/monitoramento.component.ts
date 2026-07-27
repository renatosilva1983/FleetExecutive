import { ChangeDetectionStrategy, Component, signal } from '@angular/core';

interface ServicoAoVivo {
  id: string;
  pedido: string;
  cliente: string;
  rota: string;
  passo: number;
  status: string;
  motorista: string;
  veiculo: string;
  online: boolean;
  alerta?: boolean;
  eta?: string;
}

const PASSOS = ['Agendado', 'A caminho do embarque', 'Em serviço', 'Retornando', 'Finalizado'];

/**
 * Monitoramento ao vivo (portado de monitoramento.html). Enquanto o endpoint de
 * telemetria em tempo real não é exposto, usa dados representativos; a estrutura
 * (board de serviços + resumo) está pronta para receber um stream (SSE/WebSocket)
 * ou polling do MonitoramentoController.
 */
@Component({
  selector: 'app-monitoramento',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './monitoramento.component.html',
  styleUrl: './monitoramento.component.scss',
})
export class MonitoramentoComponent {
  protected readonly passos = PASSOS;

  protected readonly servicos = signal<ServicoAoVivo[]>([
    {
      id: '#319', pedido: '#165', cliente: 'Sind. Trab. Refeições SP', rota: 'São Paulo → Praia Grande',
      passo: 2, status: '📍 Em deslocamento para embarque', motorista: 'EDSON', veiculo: 'Ônibus Exec. 46L', online: true, eta: 'ETA embarque: 08:20',
    },
    {
      id: '#310', pedido: '#162', cliente: 'INSPER', rota: 'São Paulo → Campos do Jordão',
      passo: 3, status: '🚌 Em serviço', motorista: 'Igor Martins', veiculo: 'Ônibus Exec. 50L', online: true, eta: 'ETA destino: 11:05',
    },
    {
      id: '#285', pedido: '#158', cliente: 'Doremus Alimentos Ltda', rota: 'São Paulo → Santos',
      passo: 1, status: '⏳ Aguardando check-in do prestador', motorista: 'Grecia Turismo', veiculo: 'Van Exec. 15L', online: false, alerta: true,
    },
    {
      id: '#288', pedido: '#159', cliente: 'Fundação Instituto', rota: 'Guarulhos → São Paulo',
      passo: 4, status: '✅ Finalizado', motorista: 'Marcos Vinícius', veiculo: 'Sedan de Luxo', online: true,
    },
  ]);

  protected get emServico(): number {
    return this.servicos().filter((s) => s.passo >= 1 && s.passo < 4).length;
  }
  protected get alertas(): number {
    return this.servicos().filter((s) => s.alerta).length;
  }
  protected get finalizados(): number {
    return this.servicos().filter((s) => s.passo >= 4).length;
  }

  protected pips(n: number): Array<'done' | 'active' | ''> {
    return [0, 1, 2, 3, 4].map((i) => (i < n ? 'done' : i === n ? 'active' : ''));
  }
}
