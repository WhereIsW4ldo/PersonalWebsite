export type LoginResponse = {
	accessToken: string;
	refreshToken: string;
};

export type LoginRequest = {
	username: string;
	password: string;
};
