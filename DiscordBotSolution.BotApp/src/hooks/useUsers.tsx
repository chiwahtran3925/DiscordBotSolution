import { useEffect, useState } from 'react';
import type { User } from '../models/User';
import { AllUsers } from '../services/BotApiService';

export function useUsers() {
    const [users, setUsers] = useState<User[]>([]);
    async function getUsers() {
        const result = await AllUsers();
        setUsers(result);
    }

    useEffect(() => {
        getUsers();
    }, []);

    return { users, setUsers, getUsers };
}