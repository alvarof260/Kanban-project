import { ESTADOS } from "../../../../constants";
import { z } from "zod";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { color } from "../FormTaskCreate/FormTaskCreate";
import { InputForm } from "../../../../components";

interface FormTaskProps {
  stateInitial: number;
  idTask: number;
  isOwnerBoard: boolean;
  onUpdateTask: (newState: TaskUpdateValues, id: number) => void;
}

export const TaskUpdateSchema = z.object({
  name: z.string().max(100, "El nombre de tarea no debe exceder los 100 caracteres."),
  description: z.string().max(255, "La descripcion no debe exceder los 255 caracteres."),
  color: z.string().max(30, "El color de la tarea no debe exceder los 30 caracteres"),
  status: z.number().gte(1).lte(5)
});

export type TaskUpdateValues = z.infer<typeof TaskUpdateSchema>

export const FormTask = ({ stateInitial, idTask, isOwnerBoard, onUpdateTask }: FormTaskProps) => {
  const { control, handleSubmit, formState: { errors } } = useForm<TaskUpdateValues>({
    resolver: zodResolver(TaskUpdateSchema),
    defaultValues: {
      name: "",
      description: "",
      color: color[stateInitial],
      status: stateInitial
    }
  });

  const onSubmit: SubmitHandler<TaskUpdateValues> = async (formData: TaskUpdateValues) => {
    formData.color = color[formData.status];

    const options: RequestInit = {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
      credentials: "include"
    };

    try {
      const response = await fetch(`http://localhost:5093/api/Task/${idTask}`, options);

      if (!response.ok) {
        throw new Error('Error al conectar con el servidor.');
      }

      const data: { success: boolean } = await response.json();

      if (!data.success) {
        return;
      }


      onUpdateTask(formData, idTask);
    } catch (err) {
      console.error("ERROR: ", err);
    }
  };

  return (
    <form className="flex flex-col justify-center w-full gap-4" onSubmit={handleSubmit(onSubmit)}>
      {
        isOwnerBoard &&
        <>
          <InputForm
            name="name"
            label="nombre"
            control={control}
            type="text"
            placeholder="ingrese el nombre de la tarea"
            error={errors.name}
          />
          <InputForm
            name="description"
            label="descripcion"
            control={control}
            type="text"
            placeholder="ingrese la descripcion de la tarea"
            error={errors.description}
          />
        </>
      }
      <section className="flex flex-col justify-start gap-2 h-28 w-full">
        <label className="text-sm font-medium text-text-light" htmlFor="estado">Estado</label>
        <Controller
          name="status"
          control={control}
          render={({ field }) => (
            <select {...field} onChange={(e) => field.onChange(Number(e.target.value))} className="border border-accent-dark/30 bg-transparent rounded-md px-3 py-2 text-sm text-text-muted outline-none focus:border-accent-light">
              {Object.entries(ESTADOS).map(([key, nombre]) => (
                <option className="bg-background-primary" key={key} value={Number(key)}>{nombre}</option>
              ))}
            </select>
          )}
        />
        {errors.status && <p className="text-xs font-medium text-red-500/70 mt-2">{errors.status.message}</p>}
      </section>
      <button
        className="bg-accent-light w-full py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300"
      >
        Enviar
      </button>
    </form>
  );
};
