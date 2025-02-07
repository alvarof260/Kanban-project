import { useEffect, useState } from "react";
import { useAuthContext } from "../../context/auth.context";
import { BoardCard } from "./components";

interface Board {
  id: number;
  nombreUsuarioPropietario: string;
  nombre: string;
  descripcion: string;
}

export const Boards = () => {
  const [boards, setBoards] = useState<Board[]>([]);
  const { user } = useAuthContext();

  const fetchBoard = async (id: number) => {
    if (!id) return;

    try {
      const response = await fetch(`http://localhost:5093/api/tablero/${id}`, {
        credentials: "include"
      });

      const jsonData: { success: boolean, data: Board[] | null } = await response.json();

      if (!jsonData.success) {
        throw new Error("Error en response");
      }

      setBoards(jsonData.data || []);
    } catch (err) {
      console.log(err);
    }
  };

  useEffect(() => {
    if (user?.id) {
      fetchBoard(user?.id);
    }
  }, [user?.id]);

  return (
    <>
      <section className="p-8 w-full h-full grid grid-cols-6 grid-rows-6 gap-5">
        {boards.map((board) => (
          <BoardCard
            key={board.id}
            name={board.nombre}
            description={board.descripcion}
            owner={board.nombreUsuarioPropietario}
          />
        ))}
      </section>
    </>
  );
};
