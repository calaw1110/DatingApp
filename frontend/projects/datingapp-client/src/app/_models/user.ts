/** 使用者介面 */
export interface IUser {
    /** 使用者名稱 */
    username: string;
    /** 身份驗證令牌 */
    token: string;
    /** 大頭照網址 */
    photoUrl: string;
    /** 使用者別名 */
    knownAs: string;
    /** 性別 */
    gender: string;
    /** 身分 */
    roles: string[];
}
export class User implements IUser {
    username: string = "";
    token: string = "";
    photoUrl: string = "";
    knownAs: string = "";
    gender: string = "";
    roles: string[] = [];
}
