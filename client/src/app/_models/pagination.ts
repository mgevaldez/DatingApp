export interface Pagination {
  currentPage: number;
  totalPages: number;
  totalCount: number;
  pageSize: number;
}

export class PaginatedResult<T> {
  items?: T;
  pagination?: Pagination;
}
