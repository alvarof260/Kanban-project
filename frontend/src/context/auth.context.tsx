import { createContext, Dispatch, ReactNode, SetStateAction, useContext, useState } from "react";
import { User } from "../models";

interface AuthContextType {
  user: User | null;
  setUser: Dispatch<SetStateAction<User | null>>
}

interface Props {
  children: ReactNode
}

const AuthContext = createContext<AuthContextType>({
  user: null,
  setUser: () => { }
});

export const useAuthContext = () => {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth debe estar dentro de de un AuthProvider");
  }

  return context;
};

export const AuthProvider = ({ children }: Props) => {
  const [user, setUser] = useState<User | null>(null);

  return (
    <AuthContext.Provider value={{ user, setUser }}>
      {children}
    </AuthContext.Provider>
  );
};
