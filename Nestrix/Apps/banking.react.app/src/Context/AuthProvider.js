import React, { createContext, useState, useContext } from 'react';

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
    const [isLoggedIn, setIsLoggedIn] = useState(false);
    const [user, setUser] = useState(null);
    const [rekening1, setRekening1] = useState([])
    const [rekening2, setRekening2] = useState([])

    return (
        <AuthContext.Provider value={{ isLoggedIn, setIsLoggedIn, user, setUser, rekening1, setRekening1, rekening2, setRekening2 }}>
            {children}
        </AuthContext.Provider>
    );
};

export function useAuth() {
    return useContext(AuthContext);
}

export default AuthProvider;
