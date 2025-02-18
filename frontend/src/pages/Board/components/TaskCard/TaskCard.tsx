
interface TaskCardProps {
  nombre: string;
  id: number;
  state: number;
  onDeleteTask: (id: number) => void;
  onChangeState: (state: number, id: number) => void;
}

export const TaskCard = ({ nombre, id, state, onDeleteTask, onChangeState }: TaskCardProps) => {
  return (
    <li className="bg-gray-300 w-full h-20 rounded-xs flex flex-row justify-between items-center p-1">
      <p>{nombre}</p>
      <div className="flex flex-col justify-between h-full">
        <button className="bg-amber-400 p-1 rounded-xs" onClick={() => onChangeState(state, id)}>Editar</button>
        <button className="bg-red-400 p-1 rounded-xs" onClick={() => onDeleteTask(id)}>Borrar</button>
      </div>
    </li>
  );
};
