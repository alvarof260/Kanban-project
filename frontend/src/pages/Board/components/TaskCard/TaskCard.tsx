
interface TaskCardProps {
  nombre: string;
  id: number;
  state: number;
  color: string;
  idUsuarioAsignado: number;
  onDeleteTask: (id: number) => void;
  onChangeState: (state: number, id: number) => void;
  onAssignTask: (id: number, idUsuarioAsignado: number) => void;
}

export const TaskCard = ({ nombre, id, state, color, idUsuarioAsignado, onDeleteTask, onChangeState, onAssignTask }: TaskCardProps) => {
  return (
    <li className={`bg-gray-300 w-full h-20 rounded-xs flex flex-row justify-between items-center p-6 border-t-4 ${color}`}>
      <p>{nombre}</p>
      <label>{idUsuarioAsignado}</label>
      <div className="flex justify-between ">
        <button className="bg-amber-400 p-1 rounded-xs" onClick={() => onChangeState(state, id)}>Editar</button>
        <button className="bg-red-400 p-1 rounded-xs" onClick={() => onDeleteTask(id)}>Borrar</button>
        <button className="bg-blue-500 p-1 rounded-xs" onClick={() => onAssignTask(id, idUsuarioAsignado)}>Asignar</button>
      </div>
    </li>
  );
};
