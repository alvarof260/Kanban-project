import { createContext, ReactNode, useContext, useState } from "react";
import { User } from "../models";
import { useNavigate } from "react-router";

interface SessionContextType {
  user: User;
  login: (user: User) => void;
  logout: () => void;
}

interface Props {
  children: ReactNode
}

const SessionContext = createContext<SessionContextType | undefined>(undefined);

export const useSessionContext = () => {
  const context = useContext(SessionContext);

  if (!context) {
    throw new Error("useSessionContext debe estar dentro de un SessionProvider");
  }

  return context;
};

const initialStateUser: User = {
  id: 0,
  nombreDeUsuario: "",
  password: "",
  rolUsuario: 1
};

export const SessionProvider = ({ children }: Props) => {
  const [user, setUser] = useState<User>(() => {
    try {
      const localStorageUser = localStorage.getItem("user");
      return localStorageUser ? JSON.parse(localStorageUser) : initialStateUser;
    } catch (error) {
      console.error("Error al parsear el usuario de localStorage:", error);
      return null;
    }
  });
  const navigate = useNavigate();

  const login = (user: User) => {
    setUser(user);
    localStorage.setItem('user', JSON.stringify(user));
    navigate("/boards");
  };

  const logout = () => {
    setUser(initialStateUser);
    localStorage.removeItem('user');
    navigate("/");
  };

  return (
    <SessionContext.Provider value={{ user, login, logout }}>
      {children}
    </SessionContext.Provider>
  );
};
