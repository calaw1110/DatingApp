import { User } from "./user";

export interface IUserParams {
    gender: string;
    minAge: number;
    maxAge: number;
    pageNumber: number;
    pageSize: number;
    orderBy: string;
}
export class UserParams implements IUserParams {
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 3;
    gender = "";
    constructor(user: User) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
    }
    orderBy: string = "lastActive";
}