import { useState } from "react";
import { Login } from '../services/BotApiService';
import type { LoginFormProps } from '../models/LoginFormProps';
export function LoginForm({ onLogin }: LoginFormProps) {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const handleLogin = async () => {
        const result = await Login(username, password);
        if (result) {
            onLogin();
        }
        else {
            setError("Incorrect Login");
        }
    };

    return (
    <div className="login-container">
            <h1>Login</h1>
            <form onSubmit={e => { e.preventDefault(); handleLogin(); }}>
                <input placeholder="Username" value={username} onChange={e => setUsername(e.target.value)} />
                <input placeholder="Password" type="password" value={password} onChange={e => setPassword(e.target.value)} />
                <button onClick={handleLogin}>Login</button>
                {error && <p className="error-message">{error}</p>}
            </form>
    </div>);    
}