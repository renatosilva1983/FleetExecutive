import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedList, PaginationQuery } from '../models/paginated-list';
import { Driver, DriverListItem } from './api.models';
import { toHttpParams } from './http-params.util';

export interface DriversQuery extends PaginationQuery {
  busca?: string;
  tipo?: string;
  capacidade?: string;
  indicacao?: boolean;
  ativo?: boolean;
}

/** Prestadores (motoristas/fornecedores) — /api/v1/prestadores. */
@Injectable({ providedIn: 'root' })
export class DriversService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/api/v1/prestadores`;

  list(query?: DriversQuery): Observable<PaginatedList<DriverListItem>> {
    return this.http.get<PaginatedList<DriverListItem>>(this.baseUrl, { params: toHttpParams(query) });
  }

  getById(id: string): Observable<Driver> {
    return this.http.get<Driver>(`${this.baseUrl}/${id}`);
  }

  create(body: unknown): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.baseUrl, body);
  }

  update(id: string, body: unknown): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, body);
  }

  bulkUpdateCommission(driverIds: string[], novaComissaoPercentual: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/comissoes-em-massa`, { driverIds, novaComissaoPercentual });
  }
}
