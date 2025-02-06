import type {LoginRequest, LoginResponse} from "$lib/login/model";
import {loginStore} from "$lib/login/login.store";

const apiUrl: string = "https://waldo.today/api/login";

export async function login(username: string, password: string): Promise<boolean> {
    const request: LoginRequest = {
        username: username,
        password: password
    };
    
    let response: Response;
    
    try {
        response = await fetch(apiUrl, {
            method: "POST",
            body: JSON.stringify(request)
        });
    }
    catch (error) {
        console.error(`Failed to log in:`, error);
        return false;
    }
    
    if (!response.ok)
    {
        console.error(`Failed to log in: ${response.status}`, response.body);
        return false;
    }
    
    const parsedBody = await response.json() as LoginResponse;
    
    loginStore.set({access_token: parsedBody.access_token, refresh_token: parsedBody.refresh_token});
    return true;
}