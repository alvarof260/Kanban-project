import { z } from "zod";

export const LoginSchema = z.object({
  username: z.string().min(1, "Debe ingresar el usuario.").max(50, "El usuario no debe exceder los 50 caracteres."),
  password: z.string().min(1, "Debe ingresar la contraseña.").max(50, "La contraseña no debe superar los 50 caracteres.")
});

export type LoginValues = z.infer<typeof LoginSchema>
