import { Navigate, Outlet } from "react-router";
import { useSessionContext } from "../contexts/session.context";

export const PrivateGuard = () => {
  const { user } = useSessionContext();

  return user ? <Outlet /> : <Navigate to={"/"} replace />;
};
