import { useState, ChangeEvent, FormEvent } from "react";
import { FormField } from "../";
import { User } from "../../../../models";
import { useSessionContext } from "../../../../context/session.context";

interface FormData {
  nombreDeUsuario: string;
  password: string;
}

const initialStateFormData: FormData = {
  nombreDeUsuario: "",
  password: ""
};

interface ResponseFetch {
  success: boolean;
  data: User;
}

export interface FormErrors {
  nombreDeUsuario?: { message: string };
  password?: { message: string };
}

function validateForm(values: FormData) {
  const errors: FormErrors = {};

  if (values.nombreDeUsuario === "") {
    errors["nombreDeUsuario"] = { message: 'Debe ingresar el nombre de usuario' };
  }

  if (typeof values.nombreDeUsuario !== 'string') {
    errors["nombreDeUsuario"] = { message: 'Debe ser del tipo string' };
  }

  if (values.nombreDeUsuario.length > 50) {
    errors["nombreDeUsuario"] = { message: 'Debe tener menos de 50 caracteres' };
  }

  if (values.password === "") {
    errors["password"] = { message: 'Debe ingresar la contraseña' };
  }

  if (typeof values.password !== 'string') {
    errors["password"] = { message: 'Debe ser del tipo string' };
  }

  if (values.password.length > 50) {
    errors["password"] = { message: 'Debe tener menos de 50 caracteres' };
  }

  return errors;
}

export const LoginForm = () => {
  const [formData, setFormData] = useState<FormData>(initialStateFormData);
  const [error, setError] = useState<FormErrors>({});
  const { login } = useSessionContext();

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prevState => ({ ...prevState, [name]: value }));
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const validationErrors = validateForm(formData);

    if (Object.keys(validationErrors).length > 0) {
      setError(validationErrors);
      setFormData(initialStateFormData);
      return;
    }

    setError({});

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

      setFormData(initialStateFormData);
      login(data.data);
    } catch (err) {
      console.error("Error:", err);
    }
  };

  return (
    <form className="flex flex-col h-full w-full items-center justify-start " onSubmit={handleSubmit} >
      <FormField
        label="usuario"
        name="nombreDeUsuario"
        type="text"
        maxLength={50}
        placeholder="Ingrese el nombre de usuario"
        value={formData.nombreDeUsuario}
        onChangeValue={handleChange}
        error={error.nombreDeUsuario}
      />
      <FormField
        label="password"
        name="password"
        type="password"
        maxLength={50}
        placeholder="Ingrese la contraseña"
        value={formData.password}
        onChangeValue={handleChange}
        error={error.password}
      />
      <button
        className="bg-accent-light w-full py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300 mt-12"
      >
        Iniciar Sesión
      </button>
    </form>
  );
};
