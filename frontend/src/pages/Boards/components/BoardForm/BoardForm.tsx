import { useSessionContext } from "../../../../contexts/session.context";
import { Board } from "../../../../models";
import { Modals } from "../../Boards";
import { z } from "zod";
import { SubmitHandler, useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { InputForm } from "../../../../components";

export const CreateBoardSchema = z.object({
  ownerUserId: z.number(),
  name: z.string().min(1, "El nombre es obligatorio").max(100, "El nombre del tablero no debe exceder los 100 caracteres."),
  description: z.string().min(1, "El nombre es obligatorio").max(255, "La descripcion no debe exceder los 255 caracteres.")
});

export type CreateBoardValues = z.infer<typeof CreateBoardSchema>

interface BoardFormProps {
  onAddBoard: (newBoard: Board, newState: Modals) => void;
}

export const BoardForm = ({ onAddBoard }: BoardFormProps) => {
  const { user } = useSessionContext();
  const { control, handleSubmit, formState: { errors } } = useForm<CreateBoardValues>({
    resolver: zodResolver(CreateBoardSchema),
    defaultValues: {
      ownerUserId: user?.id,
      name: "",
      description: ""
    }
  });

  const onSubmit: SubmitHandler<CreateBoardValues> = async (formData: CreateBoardValues) => {
    const options: RequestInit = {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
      credentials: "include"
    };

    try {
      const response = await fetch("http://localhost:5093/api/Board", options);

      if (!response.ok) {
        throw new Error('Error al conectar con el servidor.');
      }

      const data: { success: boolean, data: Board } = await response.json();

      if (!data.success) {
        return;
      }

      onAddBoard(data.data, "none");
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
