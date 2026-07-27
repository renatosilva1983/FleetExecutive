import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { NotificationService } from '../../core/notifications/notification.service';

/**
 * Configurações da Empresa. Página de settings (não havia HTML de origem
 * correspondente — construída sobre o design system). Ligar ao
 * EmpresaController quando o endpoint de configuração da empresa/tenant existir.
 */
@Component({
  selector: 'app-empresa',
  imports: [ReactiveFormsModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './empresa.component.html',
  styleUrl: './empresa.component.scss',
})
export class EmpresaComponent {
  private readonly fb = inject(FormBuilder);
  private readonly notify = inject(NotificationService);

  protected readonly form = this.fb.nonNullable.group({
    razaoSocial: ['Bessa Transportes Ltda'],
    cnpj: ['12.345.678/0001-90'],
    email: ['comercial@bessatransportes.com.br'],
    telefone: ['(11) 4002-8922'],
    margemMinima: [40],
    metaNps: [70],
  });

  protected salvar(): void {
    this.notify.success('Configurações da empresa salvas.');
  }
}
