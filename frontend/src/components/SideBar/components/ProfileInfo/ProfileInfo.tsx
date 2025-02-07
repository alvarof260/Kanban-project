import { useAuthContext } from "../../../../context/auth.context";

export const ProfileInfo = () => {
  const { user } = useAuthContext();

  return (
    <section>
      <h3 className='text-text-100 text-md font-poppins font-medium'>{user?.nombreDeUsuario ? user.nombreDeUsuario : "Unknown"}</h3>
      <p className='text-text-200 text-xs font-poppins font-light'>{user?.rolUsuario == 1 ? "Administrador" : "Operador"}</p>
    </section>
  );
};
