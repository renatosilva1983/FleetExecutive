import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedList, PaginationQuery } from '../models/paginated-list';
import { Charge, Commission, Invoice } from './api.models';
import { toHttpParams } from './http-params.util';

export interface ChargesQuery extends PaginationQuery {
  status?: string;
  orderId?: string;
}
export interface CommissionsQuery extends PaginationQuery {
  recebedorTipo?: string;
  recebedorId?: string;
  status?: string;
}
export interface InvoicesQuery extends PaginationQuery {
  status?: string;
}

/**
 * Financeiro — reúne os três recursos do domínio:
 *   /api/v1/cobrancas, /api/v1/comissoes, /api/v1/faturas.
 */
@Injectable({ providedIn: 'root' })
export class FinanceiroService {
  private readonly http = inject(HttpClient);
  private readonly base = `${environment.apiBaseUrl}/api/v1`;

  // ---- Cobranças
  listCharges(query?: ChargesQuery): Observable<PaginatedList<Charge>> {
    return this.http.get<PaginatedList<Charge>>(`${this.base}/cobrancas`, { params: toHttpParams(query) });
  }
  createCharge(body: unknown): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(`${this.base}/cobrancas`, body);
  }
  registerPayment(id: string, recebidoPelaEmpresa: boolean): Observable<void> {
    return this.http.post<void>(`${this.base}/cobrancas/${id}/registrar-recebimento`, { recebidoPelaEmpresa });
  }

  // ---- Comissões
  listCommissions(query?: CommissionsQuery): Observable<PaginatedList<Commission>> {
    return this.http.get<PaginatedList<Commission>>(`${this.base}/comissoes`, { params: toHttpParams(query) });
  }
  adjustCommission(id: string, novoValor: number, justificativa: string): Observable<void> {
    return this.http.patch<void>(`${this.base}/comissoes/${id}/ajustar`, { novoValor, justificativa });
  }
  markCommissionPaid(id: string): Observable<void> {
    return this.http.post<void>(`${this.base}/comissoes/${id}/marcar-paga`, {});
  }

  // ---- Faturas
  listInvoices(query?: InvoicesQuery): Observable<PaginatedList<Invoice>> {
    return this.http.get<PaginatedList<Invoice>>(`${this.base}/faturas`, { params: toHttpParams(query) });
  }
  generateInvoice(orderId: string): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(`${this.base}/faturas/${orderId}/gerar`, {});
  }
}
