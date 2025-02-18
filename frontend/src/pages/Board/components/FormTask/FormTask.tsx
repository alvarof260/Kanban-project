import { useState, ChangeEvent } from "react";
import { ESTADOS } from "../../../../constants";

interface FormTaskProps {
  stateInitial: number;
  idTask: number;
  onUpdateTask: (newState: number, id: number) => void;
}

interface FormDataValues {
  estado: number;
}

export const FormTask = ({ stateInitial, idTask, onUpdateTask }: FormTaskProps) => {
  const [formData, setFormData] = useState<FormDataValues>({ estado: stateInitial });

  const handleSubmit = async (e: ChangeEvent<HTMLFormElement>) => {
    e.preventDefault();

    console.log(formData);

    const options: RequestInit = {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
      credentials: "include"
    };

    try {
      const response = await fetch(`http://localhost:5093/api/Tarea/${idTask}`, options);

      if (!response.ok) {
        throw new Error('Error al conectar con el servidor.');
      }

      const data: { success: boolean } = await response.json();

      if (!data.success) {
        return;
      }


      onUpdateTask(formData.estado, idTask);
    } catch (err) {
      console.error("ERROR: ", err);
    }
  };

  return (
    <form className="flex flex-col justify-center w-full gap-4" onSubmit={handleSubmit}>
      <label htmlFor="estado">Estado</label>
      <select
        id="estado"
        name="estado"
        value={formData.estado}
        onChange={(e) => setFormData(prevState => ({ ...prevState, [e.target.name]: Number(e.target.value) }))}
      >
        {Object.entries(ESTADOS).map(([key, nombre]) =>
          <option key={key} value={key}>{nombre}</option>)}
      </select>
      <button className="bg-green-500 py-2 rounded-xs">Enviar</button>
    </form>
  );
};
