<script lang="ts">
    import {getLoginInformation, type loginInformation, loginStore} from "$lib/login/login.store";
    import {login} from "$lib/login/api";

    let accessToken: string = '';
    
    loginStore.subscribe((loginInfo: loginInformation) => {
        accessToken = getLoginInformation().access_token;
    });
    
    let userName: string = '';
    let password: string = '';
    
    function HandleLogin()
    {
        login(userName, password);
    }
</script>

<style>
    
</style>

<div>
    <label>
        Username
        <input bind:value={userName} />
    </label>
    <label>
        Password
        <input bind:value={password} />
    </label>
    
    <button on:click={HandleLogin}>Log me in!</button>
</div>

<p>AccessToken filled in with: {accessToken} </p>

