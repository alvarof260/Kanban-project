
import { zodResolver } from "@hookform/resolvers/zod";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { z } from "zod";
import { useFetch } from "../../../../hooks";
import { User } from "../../../../models";

interface Props {
  idUsuarioAsignado: number;
  nombreUsuarioAsignado: string;
  idTask: number;
  onAssignTask: (newState: AssignTaskValues, id: number) => void;
}

export const AssignTaskSchema = z.object({
  idUsuarioAsignado: z.number(),
  nombreUsuarioAsignado: z.string()
});

export type AssignTaskValues = z.infer<typeof AssignTaskSchema>;

export const AssignTaskForm = ({ idUsuarioAsignado, nombreUsuarioAsignado, idTask, onAssignTask }: Props) => {
  const { control, handleSubmit, setValue, formState: { errors } } = useForm<AssignTaskValues>({
    resolver: zodResolver(AssignTaskSchema),
    defaultValues: {
      idUsuarioAsignado: idUsuarioAsignado,
      nombreUsuarioAsignado: nombreUsuarioAsignado
    }
  });

  const { data: users } = useFetch<User>("http://localhost:5093/api/usuario");

  const onSubmit: SubmitHandler<AssignTaskValues> = async (formData: AssignTaskValues) => {
    const options: RequestInit = {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
      credentials: "include"
    };

    try {
      const response = await fetch(`http://localhost:5093/api/Tarea/asignar/${idTask}`, options);

      if (!response.ok) {
        throw new Error('Error al conectar con el servidor.');
      }

      const data: { success: boolean } = await response.json();

      if (!data.success) {
        return;
      }

      onAssignTask(formData, idTask);
    } catch (err) {
      console.error("ERROR: ", err);
    }
  };

  const handleUserChange = (userId: number) => {
    const selectedUser = users.find(user => user.id === userId);
    if (selectedUser) {
      setValue("nombreUsuarioAsignado", selectedUser.nombreDeUsuario);
    }
  };

  return (
    <form className="flex flex-col justify-center w-full gap-4" onSubmit={handleSubmit(onSubmit)}>
      <section className="flex flex-col justify-start gap-2 h-28 w-full">
        <label className="text-sm font-medium text-text-light" htmlFor="estado">usuarios</label>
        <Controller
          name="idUsuarioAsignado"
          control={control}
          rules={{ required: "Debe seleccionar un usuario" }} // Validación opcional
          render={({ field }) => (
            <select
              {...field}
              onChange={(e) => {
                const userId = Number(e.target.value);
                field.onChange(userId);
                handleUserChange(userId); // Actualizar el nombre de usuario
              }}
              value={field.value || ""}
              className="border border-accent-dark/30 bg-transparent rounded-md px-3 py-2 text-sm text-text-muted outline-none focus:border-accent-light"
            >
              <option value="" disabled>Seleccione un usuario</option> {/* Opción vacía */}
              {users.map((user) => (
                <option className="bg-background-primary" key={user.id} value={user.id}>
                  {user.nombreDeUsuario}
                </option>
              ))}
            </select>
          )}
        />
        {errors.idUsuarioAsignado && <p className="text-xs font-medium text-red-500/70 mt-2">{errors.idUsuarioAsignado.message}</p>}
      </section>
      <button
        className="bg-accent-light w-full py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300"
      >
        Enviar
      </button>
    </form>
  );
};
