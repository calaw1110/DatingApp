/** 對該會員按下喜歡 */
export interface Like {
    /** 對該會員按下喜歡的唯一識別碼 */
    id: number;

    /** 使用者名稱 */
    userName: string;

    /** 年齡 */
    age: number;

    /** 別名 */
    knownAs: string;

    /** 照片 URL */
    photoUrl: string;

    /** 城市 */
    city: string;
}
