import { Navigate, Outlet } from "react-router-dom";
import { useAuthContext } from "../context/auth.context";

export const PrivateGuard = () => {
  const { user } = useAuthContext();

  return user ? <Outlet /> : <Navigate to={"/"} replace />;
};
