import { zodResolver } from "@hookform/resolvers/zod";
import { useForm, SubmitHandler } from "react-hook-form";
import { LoginValues, schema, User } from "../../../../models";
import { InputForm } from "../";
import { useAuthContext } from "../../../../context/auth.context";
import { useNavigate } from "react-router-dom";

export const LoginForm = () => {
  const { setUser } = useAuthContext();
  const navigate = useNavigate();

  const { control, handleSubmit, formState: { errors } } = useForm<LoginValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      nombreDeUsuario: "",
      password: ""
    }
  });

  const onSubmit: SubmitHandler<LoginValues> = async (data) => {
    try {
      const response = await fetch("http://localhost:5093/api/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data),
        credentials: "include"
      });

      const jsonData: { success: boolean, data?: User, message?: string } = await response.json(); // mejorar el backend con las respuestas consistentes

      if (!jsonData.success || !jsonData.data) {
        console.error("Error de autenticación:", jsonData.message);
        return;
      }

      setUser(jsonData.data);
      localStorage.setItem("user", JSON.stringify(jsonData.data));
      navigate("/home");
    } catch (err) {
      console.error("Error:", err);
    }
  };

  return (
    <form className="flex flex-col items-center gap-4" onSubmit={handleSubmit(onSubmit)}>
      <InputForm
        name="nombreDeUsuario"
        label="nombre de usuario"
        control={control}
        type="text"
        error={errors.nombreDeUsuario}
      />
      <InputForm
        name="password"
        label="password"
        control={control}
        type="text"
        error={errors.password}
      />
      <button className="bg-bg-100 rounded-md font-poppins text-text-100 px-4 py-2" type="submit">Iniciar Sesión</button>
    </form >
  );
};
