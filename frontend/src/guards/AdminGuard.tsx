import { Navigate, Outlet } from "react-router";
import { useSessionContext } from "../contexts/session.context";

export const AdminGuard = () => {
  const { user } = useSessionContext();

  return user?.roleUser === 1 ? <Outlet /> : <Navigate to={"/boards"} replace />;
};
