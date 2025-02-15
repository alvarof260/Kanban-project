
interface BoardCardProps {
  id: number;
  label: string;
  nombre: string;
  descripcion: string;
}

export const BoardCard = ({ id, label, nombre, descripcion }: BoardCardProps) => {
  return (
    <article className="bg-gray-800 p-3 rounded-xs h-52 w-64">
      <h2 className="text-2xl font-bold text-gray-50">{nombre}</h2>
      <p className="text-md font-medium text-gray-400">{descripcion}</p>
      <span className="text-xs font-light text-gray-50">{label}</span>
    </article>
  );
};
