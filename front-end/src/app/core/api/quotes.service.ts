import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedList, PaginationQuery } from '../models/paginated-list';
import { Quote, QuoteListItem, QuoteStatus } from './api.models';
import { toHttpParams } from './http-params.util';

export interface QuotesQuery extends PaginationQuery {
  status?: QuoteStatus;
  customerId?: string;
  atendenteId?: string;
}

/** Orçamentos / funil de vendas — /api/v1/orcamentos. */
@Injectable({ providedIn: 'root' })
export class QuotesService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/api/v1/orcamentos`;

  list(query?: QuotesQuery): Observable<PaginatedList<QuoteListItem>> {
    return this.http.get<PaginatedList<QuoteListItem>>(this.baseUrl, { params: toHttpParams(query) });
  }

  getById(id: string): Observable<Quote> {
    return this.http.get<Quote>(`${this.baseUrl}/${id}`);
  }

  create(command: unknown): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.baseUrl, command);
  }

  updateStatus(id: string, novoStatus: QuoteStatus, motivoPerda?: string): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${id}/status`, { novoStatus, motivoPerda });
  }

  convertToOrder(id: string): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(`${this.baseUrl}/${id}/converter-em-pedido`, {});
  }
}
