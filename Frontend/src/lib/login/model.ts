export type LoginResponse = {
    access_token: string;
    refresh_token: string;
    expires_in: number;
};

export type LoginRequest = {
    username: string;
    password: string;
};