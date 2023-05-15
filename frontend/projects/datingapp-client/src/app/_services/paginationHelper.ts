import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs";
import { PaginatedResult } from "../_models/Pagination";

/**
* 取得分頁結果
* @param {string} url - 要執行 HTTP GET 請求的 URL
* @param {HttpParams} params - HTTP 請求的參數
* @returns {Observable<PaginatedResult<T>>} 分頁結果的可觀察物件
*/
export function getPaginatedResult<T>(url: string, params: HttpParams, http: HttpClient) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
        map(response => {
            if (response.body) {
                // 從 response.body 中取得查詢結果
                paginatedResult.result = response.body;
            }
            const pagination = response.headers.get('Pagination');
            if (pagination) {
                // 從 response.headers 中取得分頁資訊並解析為物件
                paginatedResult.pagination = JSON.parse(pagination);
            }
            return paginatedResult;
        })
    );
}

/**
 * 取得分頁標頭
 * @param {number} pageNumber - 目標頁碼
 * @param {number} pageSize -  顯示比數
 * @returns {HttpParams} HTTP 請求的參數，包含分頁相關資訊
 */
export function getPaginationHeaders(pageNumber: number, pageSize: number): HttpParams {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    return params;
}