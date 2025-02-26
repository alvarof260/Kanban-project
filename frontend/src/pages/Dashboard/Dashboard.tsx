import { useState } from "react";
import { useFetch } from "../../hooks";
import { User } from "../../models";
import { CustomModal } from "../../components";
import { CreateUserForm } from "./components";

export const Dashboard = () => {
  const { data: users, setData: setUsers } = useFetch<User>("http://localhost:5093/api/Usuario");
  const [isOpen, setIsOpen] = useState<"none" | "create" | "edit">("none");

  const handleOpenCreate = () => {
    setIsOpen("create");
  };

  const handleAddUser = (newUser: User) => {
    setUsers([...users, newUser]);
    setIsOpen("none");
  };

  return (
    <>
      <main className="w-screen h-screen bg-background-primary flex flex-col gap-10 p-10">
        <section className="">
          <h2 className="text-text-light text-2xl font-medium">Usuarios</h2>
          <h4 className="text-primary-light text-base font-normal">Maneja los usuarios.</h4>
        </section>
        <section className="flex flex-col justify-start items-center gap-10 w-full h-full">
          <table className="text-sm">
            <thead className="border-b border-b-accent-dark/30">
              <tr className="hover:bg-background-tertiary/50 transition ease-in duration-300">
                <th className="h-10 px-2 text-left align-middle text-primary-light font-medium w-36">Nombre</th>
                <th className="h-10 px-2 text-left align-middle text-primary-light font-medium w-36">Rol</th>
                <th className="h-10 px-2 text-right align-middle text-primary-light font-medium w-36">Acciones</th>
              </tr>
            </thead>
            <tbody>
              {
                users.map((user) => (
                  <tr className="border-b border-b-accent-dark/30 hover:bg-background-tertiary/50 transition ease-in duration-300">
                    <td className="align-middle p-2 text-text-light font-medium">{user.nombreDeUsuario}</td>
                    <td className="align-middle p-2 text-text-light font-medium">{user.rolUsuario === 1 ? "Administrador" : "Operante"}</td>
                    <td className="align-middle text-right p-2">
                      <button className="bg-accent-light py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300">Cambiar Rol</button>
                    </td>
                  </tr>
                ))
              }
            </tbody>
          </table>
          <button
            className="bg-accent-light py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300"
            onClick={handleOpenCreate}
          >
            Crear Usuario
          </button>
        </section>
      </main>
      {
        isOpen === "create" &&
        <CustomModal onModal={() => setIsOpen("none")}>
          <CreateUserForm onAddUser={handleAddUser} />
        </CustomModal>
      }
    </>
  );
};
