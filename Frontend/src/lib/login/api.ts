import type {LoginRequest, LoginResponse} from "$lib/login/model";
import {loginStore} from "$lib/login/login.store";
import {PUBLIC_API_URL} from "$env/static/public";

const apiUrl: string = `${PUBLIC_API_URL}/api/login`;

export async function login(username: string, password: string): Promise<boolean> {
    const request: LoginRequest = {
        username: username,
        password: password
    };
    
    let response: Response;
    
    try {
        response = await fetch(apiUrl, {
            method: "POST",
            headers: new Headers({'Content-Type': 'application/json'}),
            body: JSON.stringify(request)
        });
    }
    catch (error) {
        console.error(`Error was thrown while trying to log on: `, error);
        loginStore.set({access_token: "access_token", refresh_token: "refresh_token"});
        return false;
    }
    
    if (!response.ok)
    {
        console.error(`Failed to log in: ${response.status}`, await response.text());
        loginStore.set({access_token: "access_token", refresh_token: "refresh_token"});
        return false;
    }
    
    const body = await response.text();
    
    const parsedBody = JSON.parse(body) as LoginResponse;
    
    loginStore.set({access_token: parsedBody.accessToken, refresh_token: parsedBody.refreshToken});
    return true;
}