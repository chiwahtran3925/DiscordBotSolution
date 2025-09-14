import { useUsers } from '../hooks/useUsers';
import { useBlockUser } from '../hooks/useBlockUser';
import { BlockButton } from './BlockButton';
export function UserTable() {
    const { users, setUsers, getUsers } = useUsers();
    const { handleBlockToggle } = useBlockUser(users, setUsers);

    return (
        <div className="user-table-container">
            <h1>All Users</h1>
            <button onClick={getUsers}>Refresh</button>
            {users.length > 0 ? (
                <table>
                    <thead>
                        <tr>
                            <th>Username</th>
                            <th>Is Blocked</th>
                            <td></td>
                        </tr>
                    </thead>
                   <tbody>
                        {users.map(user => (
                            <tr key={user.username}>
                                <td>{user.username}</td>
                                <td>{user.isBlocked ? 'Yes' : 'No'}</td>
                                <td>
                                    <BlockButton 
                                        username={user.username}
                                        isBlocked={user.isBlocked}
                                        onBlock={(newBlocked) => handleBlockToggle(user.username, newBlocked)}
                                    />
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
                )
                : (
                    <p>No Users</p>
                )}
        </div>
    );
}
