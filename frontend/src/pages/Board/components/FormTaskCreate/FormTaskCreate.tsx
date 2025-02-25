import { Task } from "../../../../models";
import { z } from "zod";
import { useParams } from "react-router";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { InputForm } from "../../../../components";

interface FormTaskCreateProps {
  state: number;
  onAddTask: (newTask: Task) => void;
}

export const TaskCreateSchema = z.object({
  nombre: z.string().min(1, "El nombre de la tarea es obligatorio").max(100, "El nombre de tarea no debe exceder los 100 caracteres."),
  descripcion: z.string().min(1, "La descripcion de la tarea es obligatorio").max(255, "La descripcion no debe exceder los 255 caracteres."),
  color: z.string().max(30, "El color de la tarea no debe exceder los 30 caracteres"),
  estado: z.number().gte(1).lte(5)
});

export type TaskCreateValues = z.infer<typeof TaskCreateSchema>

export const color: Record<number, string> = {
  1: "bg-gray-200",
  2: "bg-blue-300",
  3: "bg-yellow-300",
  4: "bg-purple-300",
  5: "bg-green-300"
};

export const FormTaskCreate = ({ state, onAddTask }: FormTaskCreateProps) => {
  const { control, handleSubmit, formState: { errors } } = useForm<TaskCreateValues>({
    resolver: zodResolver(TaskCreateSchema),
    defaultValues: {
      nombre: "",
      descripcion: "",
      color: color[state],
      estado: state
    }
  });
  const { idBoard } = useParams();

  const onSubmit: SubmitHandler<TaskCreateValues> = async (formData: TaskCreateValues) => {
    const options: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
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
    } catch (err) {
      console.error("ERROR: ", err);
    }
  };

  return (
    <form className="flex flex-col justify-center w-full gap-4" onSubmit={handleSubmit(onSubmit)}>
      <InputForm
        name="nombre"
        label="nombre"
        control={control}
        type="text"
        placeholder="ingrese el nombre de la tarea"
        error={errors.nombre}
      />
      <InputForm
        name="descripcion"
        label="descripcion"
        control={control}
        type="text"
        placeholder="ingrese la descripcion de la tarea"
        error={errors.descripcion}
      />
      <button
        className="bg-accent-light w-full py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300"
      >
        Enviar
      </button>
    </form>
  );
};
