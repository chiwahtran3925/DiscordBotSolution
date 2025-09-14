import type { User } from '../models/User';

export async function Login(username: string, password: string): Promise<boolean> {
    try {
        const result = await fetch('api/bot/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });
        if (result.ok) {
            const response: boolean = await result.json();
            return response;
        }
        else {
            console.error('bot login failure', result.statusText);
            return false;
        }
    }
    catch (error) {
        console.error('login error',error)
        return false;
    }
}

export async function AllUsers(): Promise<User[]> {
    const result = await fetch('api/bot/getallusers');
    if (result.ok) {
        const response: User[] = await result.json();
        return response;
    }
    else {
        console.error('bot get all users failure', result.statusText);
        return [];
    }

}

export async function BlockUser(username: string, isBlocked: boolean): Promise<boolean> {
    try {
        const result = await fetch('api/bot/updateuser', {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, isBlocked })
        });

        return result.ok; 
    } catch (error) {
        console.error('user update error', error);
        return false;
    }
}