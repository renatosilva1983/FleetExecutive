import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedList, PaginationQuery } from '../models/paginated-list';
import { Fleet, Vehicle, VehicleListItem } from './api.models';
import { toHttpParams } from './http-params.util';

export interface VehiclesQuery extends PaginationQuery {
  busca?: string;
  tipo?: string;
  capacidadeMinima?: number;
  garagemId?: string;
  status?: string;
}

/** Veículos e frotas — /api/v1/veiculos e /api/v1/frotas. */
@Injectable({ providedIn: 'root' })
export class VehiclesService {
  private readonly http = inject(HttpClient);
  private readonly vehiclesUrl = `${environment.apiBaseUrl}/api/v1/veiculos`;
  private readonly fleetsUrl = `${environment.apiBaseUrl}/api/v1/frotas`;

  list(query?: VehiclesQuery): Observable<PaginatedList<VehicleListItem>> {
    return this.http.get<PaginatedList<VehicleListItem>>(this.vehiclesUrl, { params: toHttpParams(query) });
  }

  getById(id: string): Observable<Vehicle> {
    return this.http.get<Vehicle>(`${this.vehiclesUrl}/${id}`);
  }

  create(body: unknown): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.vehiclesUrl, body);
  }

  updateStatus(id: string, novoStatus: string, motivo?: string, disponivelDesde?: string): Observable<void> {
    return this.http.patch<void>(`${this.vehiclesUrl}/${id}/status`, { novoStatus, motivo, disponivelDesde });
  }

  listFleets(ativo?: boolean): Observable<Fleet[]> {
    return this.http.get<Fleet[]>(this.fleetsUrl, { params: toHttpParams({ ativo }) });
  }

  createFleet(body: unknown): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.fleetsUrl, body);
  }
}
