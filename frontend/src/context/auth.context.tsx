import { createContext, ReactNode, useContext, useEffect, useState } from "react";
import { User } from "../models"

interface AuthContextType {
  user: User | null;
}

interface Props {
  children: ReactNode
}

const AuthContext = createContext<AuthContextType | null>(null)

export const useAuthContext = () => {
  const context = useContext(AuthContext)

  if (!context) {
    throw new Error("useAuth debe estar dentro de de un AuthProvider")
  }

  return context
}

export const AuthProvider = ({ children }: Props) => {
  const [user, setUser] = useState<User | null>(null)

  const fetchUser = async (url: string): Promise<User | undefined> => {
    try {
      const response = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          nombreDeUsuario: "Alvarof260",
          Password: "pepitoDeOro"
        }),
        credentials: "include"
      });

      const data = await response.json();

      if (!data.success) {
        console.error("Error de autenticación:", data.message);
        return;
      }

      setUser(data.data);
    } catch (error) {
      console.error("Error:", error)
    }
  }

  useEffect(() => {
    fetchUser("http://localhost:5093/api/login")
  }, [])

  return (
    <AuthContext.Provider value={{ user }}>
      {children}
    </AuthContext.Provider>
  )
}
