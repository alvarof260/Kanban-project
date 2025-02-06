import { z } from "zod";

export const schema = z.object(({
  nombreDeUsuario: z.string().min(1, "El nombre del usuario es requerido").max(50, "El nombre de usuario debe tener menos de 50 caracteres"),
  password: z.string().min(1, "La contraseña es requerida")
}));

export type LoginValues = z.infer<typeof schema>
