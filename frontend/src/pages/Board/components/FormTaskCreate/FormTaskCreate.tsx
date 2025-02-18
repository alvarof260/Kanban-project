import { useState, ChangeEvent } from "react";
import { Task } from "../../../../models";

interface FormTaskCreateValues {
  nombre: string;
  descripcion: string;
  color: string;
  estado: number;
}
const initialState: FormTaskCreateValues = {
  nombre: "",
  descripcion: "",
  color: "",
  estado: 0
};

interface FormTaskCreateProps {
  state: number;
  idBoard: number;
  onAddTask: (newTask: Task) => void;
}

export const FormTaskCreate = ({ state, idBoard, onAddTask }: FormTaskCreateProps) => {
  const [formData, setFormData] = useState<FormTaskCreateValues>(initialState);

  const onChangeInput = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prevState => ({ ...prevState, [name]: value }));
  };

  const handleSubmit = async (e: ChangeEvent<HTMLFormElement>) => {
    e.preventDefault();
    const dataToSend = { ...formData, estado: state };

    console.log(dataToSend);

    const options: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(dataToSend),
      credentials: "include"
    };

    try {
      const response = await fetch(`http://localhost:5093/api/Tarea/${idBoard}`, options);

      if (!response.ok) {
        throw new Error('Error al conectar con el servidor.');
      }

      const data: { success: boolean, data: Task } = await response.json();

      if (!data.success) {
        return;
      }

      console.log(data.data);

      onAddTask(data.data);
      setFormData(initialState);
    } catch (err) {
      console.error("ERROR: ", err);
    }
  };

  return (
    <form className="flex flex-col justify-center w-full gap-4" onSubmit={handleSubmit}>
      <label className="text-gray-50" htmlFor="nombre">Nombre</label>
      <input className="text-gray-50" type="text" name="nombre" id="nombre" placeholder="ingrese un nombre" value={formData.nombre} onChange={onChangeInput} required maxLength={50} />
      <label className="text-gray-50" htmlFor="descripcion">Descripcion</label>
      <input className="text-gray-50" type="text" name="descripcion" id="descripcion" placeholder="ingrese una descripcion" value={formData.descripcion} onChange={onChangeInput} required maxLength={255} />
      <label className="text-gray-50" htmlFor="color">Color</label>
      <input className="text-gray-50" type="text" name="color" id="color" placeholder="ingrese un color" value={formData.color} onChange={onChangeInput} required />
      <button className="bg-green-500 py-2 rounded-xs">Enviar</button>
    </form>
  );
};
