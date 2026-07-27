import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PaginatedList, PaginationQuery } from '../models/paginated-list';
import { TaskItem, TaskPrioridade, TaskStatus } from './api.models';
import { toHttpParams } from './http-params.util';

export interface TasksQuery extends PaginationQuery {
  status?: TaskStatus;
  responsavelId?: string;
  vinculoTipo?: string;
  vinculoId?: string;
}

export interface CreateTaskRequest {
  titulo: string;
  descricao?: string | null;
  responsavelId?: string | null;
  prioridade: TaskPrioridade;
  prazo?: string | null;
  vinculoTipo?: string | null;
  vinculoId?: string | null;
}

/** Tarefas — /api/v1/tarefas. */
@Injectable({ providedIn: 'root' })
export class TasksService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiBaseUrl}/api/v1/tarefas`;

  list(query?: TasksQuery): Observable<PaginatedList<TaskItem>> {
    return this.http.get<PaginatedList<TaskItem>>(this.baseUrl, { params: toHttpParams(query) });
  }

  create(body: CreateTaskRequest): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.baseUrl, body);
  }

  update(id: string, body: Partial<CreateTaskRequest>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, body);
  }

  updateStatus(id: string, novoStatus: TaskStatus): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}/${id}/status`, { novoStatus });
  }
}
