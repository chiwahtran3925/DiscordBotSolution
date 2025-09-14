import { useState } from "react";
import { BlockUser } from '../services/BotApiService';
import type { BlockButtonProps } from '../models/BlockButtonProps';

export function BlockButton({ username, isBlocked: userBlocked, onBlock }: BlockButtonProps) {
    const [isBlocked, setIsBlocked] = useState(userBlocked);

    const handleUserBlock = async () => {
        const result = await BlockUser(username, !isBlocked);
        if (result) {
            const newBlocked = !isBlocked;
            setIsBlocked(newBlocked);
            onBlock(newBlocked);
        }
    };

    return (
        <button onClick={handleUserBlock}>
            {isBlocked ? 'Unblock' : 'Block'}
        </button>
    );
}
