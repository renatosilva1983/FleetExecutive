import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { NotificationService } from '../../../core/notifications/notification.service';

/** Renderiza a fila de toasts do NotificationService. Fica no MainLayout. */
@Component({
  selector: 'app-toast-host',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="toast-host">
      @for (toast of notify.toasts(); track toast.id) {
        <div class="toast" [class]="'toast--' + toast.kind" (click)="notify.dismiss(toast.id)">
          <span class="toast__icon">{{ icon(toast.kind) }}</span>
          <span class="toast__msg">{{ toast.message }}</span>
        </div>
      }
    </div>
  `,
  styles: [`
    .toast-host {
      position: fixed; top: 18px; right: 18px; z-index: 1000;
      display: flex; flex-direction: column; gap: 10px; max-width: 360px;
    }
    .toast {
      display: flex; align-items: center; gap: 10px;
      padding: 12px 14px; border-radius: 12px; cursor: pointer;
      background: var(--card-bg); border: 1px solid var(--card-border);
      box-shadow: 0 8px 28px #1e1b4b1f; font-size: 13px; color: var(--text);
      animation: toast-in 0.18s ease-out;
    }
    .toast__icon { font-size: 16px; }
    .toast--success { border-left: 4px solid var(--accent); }
    .toast--error { border-left: 4px solid var(--danger); }
    .toast--warn { border-left: 4px solid var(--warning); }
    .toast--info { border-left: 4px solid var(--info); }
    @keyframes toast-in { from { opacity: 0; transform: translateX(12px); } to { opacity: 1; transform: none; } }
  `],
})
export class ToastHostComponent {
  protected readonly notify = inject(NotificationService);

  protected icon(kind: string): string {
    switch (kind) {
      case 'success': return '✅';
      case 'error': return '⛔';
      case 'warn': return '⚠';
      default: return 'ℹ';
    }
  }
}
