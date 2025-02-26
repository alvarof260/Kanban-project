import { zodResolver } from "@hookform/resolvers/zod";
import { Controller, SubmitHandler, useForm } from "react-hook-form";
import { z } from "zod";
import { InputForm } from "../../../../components";
import { User } from "../../../../models";

export const CreateUserSchema = z.object({
  usuario: z.string().min(1, "El usuario es obligatorio").max(50, "El usuario no debe exceder los 50 caracteres"),
  password: z.string().min(1, "El password es obligatorio").max(50, "El password no debe exceder los 50 caracteres"),
  rolUsuario: z.number()
});

export type CreateUserValues = z.infer<typeof CreateUserSchema>

interface Props {
  onAddUser: (newUser: User) => void;
}

export const CreateUserForm = ({ onAddUser }: Props) => {
  const { control, handleSubmit, formState: { errors } } = useForm<CreateUserValues>({
    resolver: zodResolver(CreateUserSchema),
    defaultValues: {
      usuario: "",
      password: "",
      rolUsuario: 0
    }
  });

  const onSubmit: SubmitHandler<CreateUserValues> = async (formData: CreateUserValues) => {
    const options: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
      credentials: "include"
    };
    try {
      const response = await fetch("http://localhost:5093/api/Usuario", options);

      if (!response.ok) {
        throw new Error("Error al conectar con el servidor.");
      }

      const data: { success: boolean, data: User } = await response.json();

      if (!data.success) {
        return;
      }

      onAddUser(data.data);
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <InputForm
        name="usuario"
        label="usuario"
        control={control}
        type="text"
        placeholder="ingrese el usuario"
        error={errors.usuario}
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
          name="rolUsuario"
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
        {errors.rolUsuario && <p className="text-xs font-medium text-red-500/70 mt-2">{errors.rolUsuario.message}</p>}
      </section>
      <button
        className="bg-accent-light w-full py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300"
      >
        Enviar
      </button>
    </form>
  );
};
