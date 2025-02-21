import { useState } from "react";
import { useSessionContext } from "../../contexts/session.context";
import { CustomModal } from "../../components";
import { Board } from "../../models";
import { BoardGroup, BoardCard, ButtonAddBoard, BoardForm } from "./components";
import { getBoards } from "../../services";
import { useFetch } from "../../hooks";

export type Modals = "create" | "edit" | "delete" | "none";

export const Boards = () => {
  const { user } = useSessionContext();
  const { data: boards, setData: setBoards } = useFetch<Board[]>(() => getBoards(user.id));
  const [isOpen, setIsOpen] = useState<Modals>("none");

  const handleModal = (newState: Modals) => {
    setIsOpen(newState);
  };

  const handleAddBoard = (newBoard: Board, newState: Modals) => {
    const newBoards = [...(boards ?? []), newBoard];
    setBoards(newBoards);
    setIsOpen(newState);
  };

  return (
    <main className="w-screen h-screen bg-background-primary p-10">
      <BoardGroup>
        {(boards ?? []).map((board) =>
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
