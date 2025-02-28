import { ApiResponse, LoginSchema, LoginValues, User } from "../../../../models";
import { useSessionContext } from "../../../../contexts/session.context";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { InputForm, Toast } from "../../../../components";
import { useState } from "react";


const initialState: LoginValues = {
  username: "",
  password: ""
};

export const LoginForm = () => {
  const { control, handleSubmit, formState: { errors } } = useForm<LoginValues>({
    resolver: zodResolver(LoginSchema),
    defaultValues: initialState
  });
  const [error, setError] = useState<string | null>(null);
  const { login } = useSessionContext();

  const onSubmit: SubmitHandler<LoginValues> = async (formData: LoginValues) => {
    const options: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
      credentials: "include",
    };

    try {
      const response = await fetch("http://localhost:5093/api/login", options);

      const data: ApiResponse<User> = await response.json();
      if (!response.ok) {
        throw new Error(data.message || "Error al conectar con el servidor.");
      }

      if (!data.success || !data.data) {
        setError(data.message || "Error desconocido");
        return;
      }

      setError(null);
      login(data?.data);
    } catch (err) {
      if (err instanceof Error) {
        setError(err.message);
      } else {
        setError("Error inesperado");
      }
    }
  };

  return (
    <>
      <form className="flex flex-col h-full w-full items-center justify-start " onSubmit={handleSubmit(onSubmit)} >
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
          placeholder="ingrese la contraseña"
          error={errors.password}
        />
        <button
          className="bg-accent-light w-full py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300 mt-12"
        >
          Iniciar Sesión
        </button>
      </form>
      {
        error && (
          <Toast error={error} onCloseToast={() => setError(null)} />
        )
      }
    </>
  );
};
