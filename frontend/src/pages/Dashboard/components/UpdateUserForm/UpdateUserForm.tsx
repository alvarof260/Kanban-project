import { z } from "zod";
import { ApiResponse, User } from "../../../../models";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { InputForm } from "../../../../components";

interface Props {
  userSelected: User;
  onUpdateUser: (updatedUser: User, id: number) => void;
}

export const UpdateUserSchema = z.object({
  username: z.string().max(50, "El usuario no debe exceder los 50 caracteres"),
  password: z.string().max(50, "El password no debe exceder los 50 caracteres").optional(),
  roleUser: z.number()
});

export type UpdateUserValues = z.infer<typeof UpdateUserSchema>

export const UpdateUserForm = ({ userSelected, onUpdateUser }: Props) => {
  const { control, handleSubmit, formState: { errors } } = useForm<UpdateUserValues>({
    resolver: zodResolver(UpdateUserSchema),
    defaultValues: {
      username: userSelected.username,
      password: userSelected.password,
      roleUser: userSelected.roleUser
    }
  });

  const onSubmit: SubmitHandler<UpdateUserValues> = async (formData: UpdateUserValues) => {
    const options: RequestInit = {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
      credentials: "include"
    };
    try {
      const response = await fetch(`http://localhost:5093/api/User/${userSelected.id}`, options);

      if (!response.ok) {
        throw new Error("Error al conectar con el servidor.");
      }

      const data: ApiResponse<null> = await response.json();


      if (!data.success) {
        return;
      }

      const updatedUser: User = {
        id: userSelected.id,
        username: formData.username,
        password: formData.password || "",
        roleUser: formData.roleUser
      };

      onUpdateUser(updatedUser, userSelected.id);
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <InputForm
        name="username"
        label="usuario"
        control={control}
        type="text"
        placeholder="ingrese el usuario"
        error={errors.username}
      />
      <InputForm
        name="password"
        label="password"
        control={control}
        type="text"
        placeholder="ingrese la password"
        error={errors.password}
      />
      <section className="flex flex-col justify-start gap-2 h-28 w-full">
        <label className="text-sm font-medium text-text-light" htmlFor="estado">usuarios</label>
        <Controller
          name="roleUser"
          control={control}
          render={({ field }) => (
            <select
              {...field}
              value={field.value || ""}
              onChange={(e) => {
                field.onChange(Number(e.target.value));
              }}
              className="border border-accent-dark/30 bg-transparent rounded-md px-3 py-2 text-sm text-text-muted outline-none focus:border-accent-light"
            >
              <option value="" disabled>Seleccione un Rol</option> {/* Opción vacía */}
              <option className="bg-background-primary" value={1}>
                Administrador
              </option>
              <option className="bg-background-primary" value={2}>
                Operador
              </option>
            </select>
          )}
        />
        {errors.roleUser && <p className="text-xs font-medium text-red-500/70 mt-2">{errors.roleUser.message}</p>}
      </section>
      <button
        className="bg-accent-light w-full py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300"
      >
        Enviar
      </button>
    </form>
  );
};
