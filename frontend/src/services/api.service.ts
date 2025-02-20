import { LoginValues } from "../models";

export const loginUser = async (formData: LoginValues) => {
  const options: RequestInit = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(formData),
    credentials: "include",
  };

  const response = await fetch("http://localhost:5093/api/login", options);

  return response;
};
