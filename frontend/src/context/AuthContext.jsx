import { createContext, useContext, useState, useEffect } from 'react';
import { authApi } from '../api/client';

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    const saved = localStorage.getItem('sfm_user');
    return saved ? JSON.parse(saved) : null;
  });
  const [loading, setLoading] = useState(false);

  const login = async (email, password) => {
    setLoading(true);
    try {
      const { data } = await authApi.login({ email, password });
      localStorage.setItem('sfm_token', data.token);
      const userData = {
        userId: data.userId,
        fullName: data.fullName,
        email: data.email,
        role: data.role,
        isVerified: data.isVerified,
      };
      localStorage.setItem('sfm_user', JSON.stringify(userData));
      setUser(userData);
      return userData;
    } finally {
      setLoading(false);
    }
  };

  const register = async (form) => {
    setLoading(true);
    try {
      const roleMap = { client: 0, freelancer: 1 };
      const { data } = await authApi.register({
        fullName: form.fullName,
        email: form.email,
        password: form.password,
        role: roleMap[form.role] ?? 0,
      });
      localStorage.setItem('sfm_token', data.token);
      const userData = {
        userId: data.userId,
        fullName: data.fullName,
        email: data.email,
        role: data.role,
        isVerified: data.isVerified,
      };
      localStorage.setItem('sfm_user', JSON.stringify(userData));
      setUser(userData);
      return userData;
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    localStorage.removeItem('sfm_token');
    localStorage.removeItem('sfm_user');
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, register, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);
