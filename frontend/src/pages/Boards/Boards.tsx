import { useEffect, useState } from "react";
import { useSessionContext } from "../../context/session.context";
import { CustomModal } from "../../components";
import { Board } from "../../models";
import { BoardGroup, BoardCard, ButtonAddBoard, BoardForm } from "./components";
import { getBoards } from "../../services";

export type Modals = "create" | "edit" | "delete" | "none";

export const Boards = () => {
  const [boards, setBoards] = useState<Board[]>([]);
  const [isOpen, setIsOpen] = useState<Modals>("none");
  const { user } = useSessionContext();

  useEffect(() => {
    const fetchBoards = async () => {
      if (!user?.id) {
        return;
      }

      try {
        const response = await getBoards(user?.id);

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

  const handleModal = (newState: Modals) => {
    setIsOpen(newState);
  };

  const handleAddBoard = (newBoard: Board, newState: Modals) => {
    const newBoards = [...boards, newBoard];
    setBoards(newBoards);
    setIsOpen(newState);
  };

  if (!user) {
    return;
  }

  return (
    <main className="w-screen h-screen bg-background-primary p-10">
      <BoardGroup>
        {boards.map((board) =>
          <BoardCard
            key={board.id}
            id={board.id}
            label={board.nombreUsuarioPropietario || user?.nombreDeUsuario}
            nombre={board.nombre}
            descripcion={board.descripcion}
          />)}
        <ButtonAddBoard onModal={handleModal} />
      </BoardGroup>
      {isOpen === "create" &&
        <CustomModal onModal={handleModal}>
          <BoardForm onAddBoard={handleAddBoard} />
        </CustomModal>
      }
    </main>
  );
};
