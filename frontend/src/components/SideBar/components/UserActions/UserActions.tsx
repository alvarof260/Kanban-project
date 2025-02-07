import { useNavigate } from "react-router-dom";
import { LogOut, Setting } from "../../../../icons";
import { NavBarItem } from "../NavBarItem/NavBarItem";
import { useAuthContext } from "../../../../context/auth.context";

export const UserActions = () => {
  const navigate = useNavigate();
  const { setUser } = useAuthContext();

  const handleLogOut = async () => {
    try {
      const response = await fetch("http://localhost:5093/api/login/logout", {
        method: "POST",
        credentials: "include"
      });

      const jsonData = await response.json();

      if (!jsonData.success) {
        console.log("Error al cerrar sesión");
      }

      setUser(null);

      localStorage.removeItem("user");
      navigate("/");
    } catch (err) {
      console.error("Error al cerrar sesión:", err);
    }
  };

  return (
    <section className="border-t border-text-200 pt-4">
      <ul className="flex flex-col gap-2">
        <NavBarItem label="Configuracion">
          <Setting height="1.5em" width="1.5em" />
        </NavBarItem>
        <NavBarItem label="Salir" onClick={handleLogOut}>
          <LogOut height="1.5em" width="1.5em" />
        </NavBarItem>
      </ul>
    </section>
  );
};
