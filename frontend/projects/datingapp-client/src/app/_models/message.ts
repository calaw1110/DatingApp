/** 訊息介面 */
export interface Message {
    /** 訊息的唯一識別碼 */
    id: number;

    /** 發送者的唯一識別碼 */
    senderId: number;

    /** 發送者的使用者名稱 */
    senderUsername: string;

    /** 發送者的照片 URL */
    senderPhotoUrl: string;

    /** 接收者的唯一識別碼 */
    recipientId: number;

    /** 接收者的使用者名稱 */
    recipientUsername: string;

    /** 接收者的照片 URL */
    recipientPhotoUrl: string;

    /** 訊息內容 */
    content: string;

    /** 已讀的日期 */
    dateRead?: Date;

    /** 訊息發送的日期 */
    messageSent: string;
}
