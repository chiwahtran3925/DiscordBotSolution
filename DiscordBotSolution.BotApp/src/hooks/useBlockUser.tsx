import type { User } from '../models/User';

export function useBlockUser(users: User[], setUsers: (users: User[]) => void) {
    function handleBlockToggle(username: string, newBlocked: boolean) {
        setUsers(prevUsers =>
            prevUsers.map(user =>
                user.username === username ? { ...user, isBlocked: newBlocked } : user
            )
        );
    }

    return { handleBlockToggle };
}