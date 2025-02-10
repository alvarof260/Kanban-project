import { useState, ChangeEvent } from "react";
import { useAuthContext } from "../../../../context/auth.context";
import { Board } from "../../../../models/Board";

interface Props {
  handleModal: () => void;
  addBoard: () => void
}

interface CreateTableroState {
  idUsuarioPropietario: number;
  nombre: string;
  descripcion: string
}

const emptyState: CreateTableroState = {
  idUsuarioPropietario: 0,
  nombre: "",
  descripcion: ""
};

export const BoardForm = ({ handleModal, addBoard }: Props) => {
  const [formData, setFormData] = useState<CreateTableroState>(emptyState);
  const { user } = useAuthContext();

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prevState => ({ ...prevState, [name]: value }));
  };

  const handleSubmit = async (e: ChangeEvent<HTMLFormElement>) => {
    e.preventDefault();
    formData.idUsuarioPropietario = user?.id || 0;
    console.log(formData);

    try {
      const response = await fetch("http://localhost:5093/api/Tablero", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formData),
        credentials: "include"
      });

      if (!response.ok) {
        console.log("Error del servidor");
        return;
      }

      const data: { success: boolean, data: Board } = await response.json();

      if (!data.success) {
        throw new Error("Error");
      }

      addBoard();
      setFormData(emptyState);
    } catch (err) {
      console.error("ERROR: ", err);
    }
  };

  return (
    <>
      <form onSubmit={handleSubmit}>
        <label htmlFor="nombre">Nombre:</label>
        <input type="text" id="nombre" placeholder="nombre de tablero" name="nombre" value={formData.nombre} onChange={handleChange} required maxLength={50} />
        <label htmlFor="descripcion">Descripcion</label>
        <input type="text" id="descripcion" placeholder="descripcion de tablero" name="descripcion" value={formData.descripcion} onChange={handleChange} required maxLength={255} />
        <button>Enviar</button>
      </form>
      <button onClick={handleModal} className="bg-red-500 px-2 rounded-md cursor-pointer">cerrar</button>
    </>
  );
};
