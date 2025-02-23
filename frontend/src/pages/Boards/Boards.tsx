import { useState } from "react";
import { useSessionContext } from "../../contexts/session.context";
import { CustomModal } from "../../components";
import { Board } from "../../models";
import { BoardGroup, BoardCard, ButtonAddBoard, BoardForm, CardActions, CardBody, CardHeader } from "./components";
import { useFetch } from "../../hooks";
import { Link } from "react-router";
import { CardFooter } from "./components/CardFooter/CardFooter";

export type Modals = "create" | "edit" | "delete" | "none";

export const Boards = () => {
  const { user } = useSessionContext();
  const { data: boards, setData: setBoards } = useFetch<Board>(`http://localhost:5093/api/Tablero/${user?.id}`);
  const [isOpen, setIsOpen] = useState<Modals>("none");

  const handleModal = (newState: Modals) => {
    setIsOpen(newState);
  };

  const handleAddBoard = (newBoard: Board, newState: Modals) => {
    const newBoards = [...boards, newBoard];
    setBoards(newBoards);
    setIsOpen(newState);
  };

  const handleDeleteBoard = async (idBoard: number) => {
    const options: RequestInit = {
      method: "DELETE",
      credentials: "include"
    };

    try {
      const response = await fetch(`http://localhost:5093/api/Tablero/${idBoard}`, options);

      if (!response.ok) {
        throw new Error("Error al conectar con el servidor.");
      }

      const newBoards = boards.filter(board => board.id !== idBoard);

      setBoards(newBoards);
    } catch (err) {
      console.error(err);
    }
  };

  if (!user) {
    return;
  }

  return (
    <main className="w-screen h-screen bg-background-primary p-10">
      <BoardGroup>
        {boards.map((board) =>
          <BoardCard key={board.id}>
            <CardHeader>
              <Link to={`/board/${board.id}`} className="cursor-pointer">
                <h2 className="text-2xl font-semibold text-text-light hover:underline">{board.nombre}</h2>
              </Link >
              <CardActions idBoard={board.id} onDeleteBoard={handleDeleteBoard} />
            </CardHeader >
            <CardBody>
              <section className="overflow-hidden">
                <p className={"text-base font-normal text-text-muted"}>{board.descripcion}</p>
              </section>
            </CardBody>
            <CardFooter>
              <span className="text-xs font-normal text-primary-medium">Creado por {board.nombreUsuarioPropietario}</span>
            </CardFooter>
          </BoardCard>)}
        <ButtonAddBoard onModal={handleModal} />
      </BoardGroup>
      {
        isOpen === "create" &&
        <CustomModal onModal={handleModal}>
          <BoardForm onAddBoard={handleAddBoard} />
        </CustomModal>
      }
      {
        isOpen === "edit" &&
        <CustomModal onModal={handleModal}>
          <BoardForm onAddBoard={handleAddBoard} />
        </CustomModal>
      }
    </main >
  );
};
