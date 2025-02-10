import { useState } from "react";
import { CustomModal } from "../../components";
import { useFetch } from "../../hooks/useFetch";
import { BoardCard, BoardForm } from "./components";
import { Board } from "../../models/Board";

export const Home = () => {
  const { data, fetchData } = useFetch<Board[]>([], "http://localhost:5093/api/Tablero");
  const [isOpen, setIsOpen] = useState<boolean>(false);

  const handleModal = () => {
    setIsOpen(prevState => !prevState);
  };

  const addBoard = () => {
    fetchData();
    setIsOpen(prevState => !prevState);
  };

  return (
    <>
      <section className="p-8 w-full h-full grid grid-cols-6 grid-rows-6 gap-5">
        {data.map((board) => (
          <BoardCard
            key={board.id}
            name={board.nombre}
            description={board.descripcion}
            owner={board.nombreUsuarioPropietario}
          />
        ))}
        <button onClick={handleModal}>Crear Tablero</button>
      </section>
      {isOpen && <CustomModal><BoardForm handleModal={handleModal} addBoard={addBoard} /></CustomModal>}
    </>
  );
};
