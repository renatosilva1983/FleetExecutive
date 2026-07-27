/**
 * Espelha `PaginatedList<T>` do backend
 * (FleetExecutive.Application.Common.Models.PaginatedList) serializado em camelCase.
 * Toda listagem de módulo pagina no servidor — ver achado #9 da análise do sistema.
 */
export interface PaginatedList<T> {
  items: T[];
  pagina: number;
  tamanhoPagina: number;
  totalRegistros: number;
  totalPaginas: number;
}

/** Parâmetros de paginação enviados como query string (`?pagina=&tamanhoPagina=`). */
export interface PaginationQuery {
  pagina?: number;
  tamanhoPagina?: number;
}
