import { useState, ChangeEvent } from "react";
import { useSessionContext } from "../../../../contexts/session.context";
import { Board } from "../../../../models";
import { InputForm } from "..";
import { Modals } from "../../Boards";

interface FormDataValues {
  idUsuarioPropietario: number;
  nombre: string;
  descripcion: string;
}

const initialStateForm: FormDataValues = {
  idUsuarioPropietario: 0,
  nombre: "",
  descripcion: ""
};

interface BoardFormProps {
  onAddBoard: (newBoard: Board, newState: Modals) => void;
}

export const BoardForm = ({ onAddBoard }: BoardFormProps) => {
  const [formData, setFormData] = useState<FormDataValues>(initialStateForm);
  const { user } = useSessionContext();

  const handleChangeInput = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prevState => ({ ...prevState, [name]: value }));
  };

  const handleSubmit = async (e: ChangeEvent<HTMLFormElement>) => {
    e.preventDefault();
    const dataToSend = { ...formData, idUsuarioPropietario: user?.id || 0 };

    console.log(dataToSend);

    const options: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(dataToSend),
      credentials: "include"
    };

    try {
      const response = await fetch("http://localhost:5093/api/Tablero", options);

      if (!response.ok) {
        throw new Error('Error al conectar con el serivdor.');
      }

      const data: { success: boolean, data: Board } = await response.json();

      if (!data.success) {
        return;
      }

      onAddBoard(data.data, "none");
      setFormData(initialStateForm);
    } catch (err) {
      console.error("ERROR: ", err);
    }
  };

  return (
    <form className="flex flex-col justify-center w-full gap-4" onSubmit={handleSubmit}>
      <InputForm label="Nombre:" type="text" name="nombre" placeholder="nombre de tablero" value={formData.nombre} onChange={handleChangeInput} maxLength={50} />
      <InputForm label="Descripcion:" type="text" name="descripcion" placeholder="descripcion de tablero" value={formData.descripcion} onChange={handleChangeInput} maxLength={255} />
      <button
        className="bg-accent-light w-full py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300 mt-12"
      >
        Enviar
      </button>
    </form>
  );
};
