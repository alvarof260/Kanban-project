import { z } from "zod";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { InputForm } from "../../../../components/";

export const BoardUpdateSchema = z.object(({
  name: z.string().max(100, "El nombre del tablero no debe exceder los 100 caracteres."),
  description: z.string().max(255, "La descripcion no debe exceder los 255 caracteres.")
}));

export type BoardUpdateValues = z.infer<typeof BoardUpdateSchema>

interface Props {
  idBoard: number;
  onUpdateBoard: (updateBoard: BoardUpdateValues, idBoard: number) => void
}

export const BoardUpdateForm = ({ idBoard, onUpdateBoard }: Props) => {
  const { control, handleSubmit, formState: { errors } } = useForm<BoardUpdateValues>({
    resolver: zodResolver(BoardUpdateSchema),
    defaultValues: {
      name: "",
      description: ""
    }
  });

  const onSubmit: SubmitHandler<BoardUpdateValues> = async (formData: BoardUpdateValues) => {
    const options: RequestInit = {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
      credentials: "include"
    };

    try {
      const response = await fetch(`http://localhost:5093/api/Board/${idBoard}`, options);

      if (!response.ok) {
        throw new Error("Error al conectar con el servidor.");
      }

      const data: { success: boolean } = await response.json();

      if (!data.success) {
        return;
      }

      onUpdateBoard(formData, idBoard);
    } catch (err) {
      console.error("ERROR: ", err);
    }

  };

  return (
    <form className="flex flex-col justify-center w-full gap-2" onSubmit={handleSubmit(onSubmit)}>
      <InputForm
        name="name"
        label="nombre"
        control={control}
        type="text"
        placeholder="ingrese el nombre del tablero"
        error={errors.name}
      />
      <InputForm
        name="description"
        label="descripcion"
        control={control}
        type="text"
        placeholder="ingrese la descripcion del tablero"
        error={errors.description}
      />
      <button
        className="bg-accent-light w-full py-2 px-4 rounded-md text-sm font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300"
      >
        Enviar
      </button>
    </form>
  );
};
