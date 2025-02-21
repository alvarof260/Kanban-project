import { LoginSchema, LoginValues, User } from "../../../../models";
import { useSessionContext } from "../../../../contexts/session.context";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { InputForm } from "../../../../components";
import { loginUser } from "../../../../services";
import { useState } from "react";

interface ResponseFetch {
  success: boolean;
  message: string;
  data?: User;
}

const initialState: LoginValues = {
  nombreDeUsuario: "",
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
    try {
      const response = await loginUser(formData);

      const data: ResponseFetch = await response.json();
      if (!response.ok) {
        throw new Error(data.message || "Error al conectar con el servidor.");
      }

      if (!data.success || !data?.data) {
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
          name="nombreDeUsuario"
          label="usuario"
          control={control}
          type="text"
          placeholder="ingrese el usuario"
          error={errors.nombreDeUsuario}
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
          <article
            className={`absolute bottom-4 right-4 bg-background-primary h-auto px-6 py-3 border border-accent-dark/30 rounded-md shadow-lg flex items-center 
        transition-all duration-300 ease-in-out transform ${error ? "opacity-100 translate-y-0" : "opacity-0 translate-y-4"
              }`}
          >
            <button
              className="absolute top-2 right-2 text-accent-dark text-lg cursor-pointer hover:text-text-light transition ease-in duration-100"
              onClick={() => setError(null)}
            >
              x
            </button>
            <p className="text-text-light text-sm">{error}</p>
          </article>
        )
      }
    </>
  );
};
