import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { VehicleAvailability } from './api.models';
import { toHttpParams } from './http-params.util';

export interface AgendaQuery {
  dataInicio: string; // yyyy-MM-dd
  dataFim: string; // yyyy-MM-dd
  garagemId?: string;
  fleetId?: string;
  vehicleId?: string;
}

/** Agenda / disponibilidade de veículos — /api/v1/agenda. */
@Injectable({ providedIn: 'root' })
export class AgendaService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/api/v1/agenda`;

  disponibilidade(query: AgendaQuery): Observable<VehicleAvailability[]> {
    return this.http.get<VehicleAvailability[]>(this.baseUrl, { params: toHttpParams(query) });
  }
}
