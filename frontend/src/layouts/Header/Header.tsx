import { Link } from "react-router";
import { useSessionContext } from "../../contexts/session.context";

export const Header = () => {
  const { user, logout } = useSessionContext();

  const isAdmin = user?.rolUsuario === 1;
  const role = user?.rolUsuario === 1 ? "Administrador" : "Operador";

  return (
    <header className="bg-background-primary border-b border-b-accent-dark/30 border-dashed flex flex-row justify-between items-center p-4">
      <section>
        <button
          className="text-sm text-text-light hover:bg-background-tertiary/70 rounded-md transition ease-in duration-300 px-2 py-1 cursor-pointer"
          onClick={logout}
        >Cerrar sesión</button>
        {
          isAdmin &&
          <Link to={"/dashboard"} className="text-sm text-text-light hover:bg-background-tertiary/70 rounded-md transition ease-in duration-300 px-2 py-1 cursor-pointer">
            dashboard
          </Link>
        }
      </section>

      <section className="text-sm flex flex-row items-center gap-4">
        <section className="text-right">
          <h3 className="text-text-light font-medium">{user?.nombreDeUsuario}</h3>
          <h4 className="text-primary-light font-medium">{role}</h4>
        </section>
        <section className={`w-10 h-10 rounded-full text-text-light text-xl font-bold ${isAdmin ? "bg-blue-600" : "bg-green-600"} flex justify-center items-center `}>
          {user?.nombreDeUsuario[0].toUpperCase()}
        </section>
      </section>
    </header>
  );
};
