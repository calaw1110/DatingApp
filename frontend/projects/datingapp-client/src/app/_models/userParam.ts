import { User } from "./user";

export interface IUserParams {
    gender: string;
    minAge: number;
    maxAge: number;
    pageNumber: number;
    pageSize: number;
}
export class UserParams implements IUserParams {
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 5;
    gender = "";
    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
    }
}