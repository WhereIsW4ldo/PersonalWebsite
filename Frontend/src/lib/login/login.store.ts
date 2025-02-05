import {get, writable, type Writable} from "svelte/store";

export type loginInformation = {
    access_token: string;
    refresh_token: string;
};

export const loginStore: Writable<loginInformation> = writable({
    access_token: "access_token",
    refresh_token: "refresh_token",
});

export const getLoginInformation = () => get(loginStore);