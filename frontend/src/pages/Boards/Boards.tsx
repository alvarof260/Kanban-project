import { useEffect, useState } from "react";
import { useSessionContext } from "../../context/session.context";
import { CustomModal } from "../../components";
import { Board } from "../../models";
import { BoardGroup, BoardCard, ButtonAddBoard, BoardForm } from "./components";

export const Boards = () => {
  const [boards, setBoards] = useState<Board[]>([]);
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const { user } = useSessionContext();

  useEffect(() => {
    const fetchBoards = async () => {
      const options: RequestInit = {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include"
      };

      try {
        const response = await fetch(`http://localhost:5093/api/Tablero/${user?.id}`, options);

        if (!response.ok) {
          throw new Error('Error al conectar con el serivdor.');
        }

        const data: { success: boolean, data: Board[] } = await response.json();

        if (!data.success) {
          return;
        }

        console.log(data.data);

        setBoards(data.data);
      } catch (err) {
        console.error("Error:", err);
      }
    };
    fetchBoards();
  }, [user?.id]);

  const handleModal = () => {
    setIsOpen(prevState => !prevState);
  };

  const handleAddBoard = (newBoard: Board) => {
    const newBoards = [...boards, newBoard];
    setBoards(newBoards);
    setIsOpen(prevState => !prevState);
  };

  return (
    <main className="w-screen h-screen bg-gray-100 p-10">
      <BoardGroup>
        {boards.map((board) =>
          <BoardCard
            key={board.id}
            id={board.id}
            label={board.nombreUsuarioPropietario}
            nombre={board.nombre}
            descripcion={board.descripcion}
          />)}
        <ButtonAddBoard onModal={handleModal} />
      </BoardGroup>
      {isOpen &&
        <CustomModal onModal={handleModal}>
          <BoardForm onAddBoard={handleAddBoard} />
        </CustomModal>
      }
    </main>
  );
};
