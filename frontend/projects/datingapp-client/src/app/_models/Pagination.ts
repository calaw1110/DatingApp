/**
 * 分頁資訊介面
 */
export interface Pagination {

    /** 目前頁面 */
    currentPage: number;

    /** 每頁項目數量 */
    itemsPerPage: number;

    /** 總項目數量 */
    totalItems: number;

    /** 總頁數 */
    totalPages: number;

}

/**
 * 分頁結果類別
 */
export class PaginatedResult<T> {

    /** 查詢結果 */
    result?: T;
    
    /** 分頁資訊 */
    pagination?: Pagination;
}
