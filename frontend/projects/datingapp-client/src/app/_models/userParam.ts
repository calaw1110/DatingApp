import { User } from "./user";

/** 使用者查詢參數介面 */
export interface IUserParams {
    /** 性別 */
    gender: string;
    /** 最小年齡 */
    minAge: number;
    /** 最大年齡 */
    maxAge: number;
    /** 頁碼 */
    pageNumber: number;
    /** 每頁顯示的項目數量 */
    pageSize: number;
    /** 排序依據 */
    orderBy: string;
}

/** 使用者查詢參數類別 */
export class UserParams implements IUserParams {

    /** 最小年齡 */
    minAge = 18;

    /** 最大年齡 */
    maxAge = 99;

    /** 頁碼 */
    pageNumber = 1;

    /** 每頁顯示的項目數量 */
    pageSize = 5;

    /** 性別 */
    gender = "";

    /** 建構子
     *  @param user - 使用者物件
     */
    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
    }

    /** 排序依據 */
    orderBy: string = "lastActive";
}
