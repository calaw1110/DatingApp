import { Photo } from "./photo";

/** 會員介面 */
export interface Member {

    /** 會員ID */
    id: number;

    /** 使用者名稱 */
    userName: string;

    /** 照片網址 */
    photoUrl: string;

    /** 年齡 */
    age: number;

    /** 別名 */
    knownAs: string;

    /** 建立時間 */
    created: string;

    /** 最後活躍時間 */
    lastActive: string;

    /** 性別 */
    gender: string;

    /** 自我介紹 */
    introduction: string;

    /** 尋找條件 */
    lookingFor: string;

    /** 興趣 */
    interests: string;

    /** 城市 */
    city: string;

    /** 國家 */
    country: string;

    /** 照片列表 */
    photos: Photo[];
}
