import { useAuthContext } from "../../../../context/auth.context";

export const ProfileIcon = () => {
  const { user } = useAuthContext();

  const getLetter = (name?: string | undefined) => {
    return name ? name[0].toUpperCase() : "?";
  };

  return (
    <article className='w-12 h-12 rounded-md flex justify-center items-center bg-primary-100'>
      <p className='text-accent-100 font-poppins text-xl font-medium'>{getLetter(user?.nombreDeUsuario)}</p>
    </article>
  );
};
