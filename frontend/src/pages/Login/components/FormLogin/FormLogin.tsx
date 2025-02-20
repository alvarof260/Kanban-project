import { LoginSchema, LoginValues, User } from "../../../../models";
import { useSessionContext } from "../../../../context/session.context";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { InputForm } from "../../../../components";

interface ResponseFetch {
  success: boolean;
  data: User;
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
  const { login } = useSessionContext();

  const onSubmit: SubmitHandler<LoginValues> = async (formData: LoginValues) => {
    const options: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
      credentials: "include"
    };

    try {
      const response = await fetch("http://localhost:5093/api/login", options);

      if (!response.ok) {
        throw new Error('Error al conectar con el servidor.');
      }

      const data: ResponseFetch = await response.json();

      if (!data.success) {
        return;
      }

      login(data.data);
    } catch (err) {
      console.error("Error:", err);
    }
  };

  return (
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
  );
};
