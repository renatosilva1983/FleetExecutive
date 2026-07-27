import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedList, PaginationQuery } from '../models/paginated-list';
import { Customer, CustomerListItem, UpsertCustomerRequest } from './api.models';
import { toHttpParams } from './http-params.util';

export interface CustomersQuery extends PaginationQuery {
  busca?: string;
  ativo?: boolean;
}

/** Clientes — /api/v1/clientes. */
@Injectable({ providedIn: 'root' })
export class CustomersService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/api/v1/clientes`;

  list(query?: CustomersQuery): Observable<PaginatedList<CustomerListItem>> {
    return this.http.get<PaginatedList<CustomerListItem>>(this.baseUrl, {
      params: toHttpParams(query),
    });
  }

  getById(id: string): Observable<Customer> {
    return this.http.get<Customer>(`${this.baseUrl}/${id}`);
  }

  create(body: UpsertCustomerRequest): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.baseUrl, body);
  }

  update(id: string, body: UpsertCustomerRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, body);
  }
}
