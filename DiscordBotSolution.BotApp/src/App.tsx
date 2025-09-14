import { useState } from "react";
import { LoginForm } from './components/LoginForm';
import { UserTable } from './components/UserTable';
function App() {
    const [loggedIn, setLoggedIn] = useState(false);

    return (
        <div>
            {!loggedIn ? (
                <LoginForm onLogin={() => setLoggedIn(true)} />
            ) : (
                <UserTable />
            )}
        </div>
    );
}

export default App;