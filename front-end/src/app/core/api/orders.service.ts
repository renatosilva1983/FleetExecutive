import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedList, PaginationQuery } from '../models/paginated-list';
import { Order, OrderListItem, StatusComercial, StatusOperacional } from './api.models';
import { toHttpParams } from './http-params.util';

export interface OrdersQuery extends PaginationQuery {
  statusComercial?: StatusComercial;
  customerId?: string;
  atendenteId?: string;
}

export interface CreateOrderRequest {
  customerId: string;
  origem: string;
  atendenteId?: string | null;
}

/** Pedidos — /api/v1/pedidos. */
@Injectable({ providedIn: 'root' })
export class OrdersService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/api/v1/pedidos`;

  list(query?: OrdersQuery): Observable<PaginatedList<OrderListItem>> {
    return this.http.get<PaginatedList<OrderListItem>>(this.baseUrl, { params: toHttpParams(query) });
  }

  getById(id: string): Observable<Order> {
    return this.http.get<Order>(`${this.baseUrl}/${id}`);
  }

  create(body: CreateOrderRequest): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.baseUrl, body);
  }

  advanceCycle(id: string, novoStatus: StatusOperacional): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${id}/ciclo`, { novoStatus });
  }

  cancel(id: string, motivo: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${id}/cancelar`, { motivo });
  }

  registerTermsAcceptance(id: string): Observable<{ codigo: string }> {
    return this.http.post<{ codigo: string }>(`${this.baseUrl}/${id}/aceite-termos`, {});
  }
}
