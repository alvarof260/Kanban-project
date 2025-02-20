import { Link } from "react-router";
import { CardActions, CardBody, CardHeader } from "..";

interface BoardCardProps {
  id: number;
  label: string;
  nombre: string;
  descripcion: string;
}

export const BoardCard = ({ id, label, nombre, descripcion }: BoardCardProps) => {
  return (
    <article className="bg-transparent rounded-md border border-accent-dark/30 p-6 min-w-86 h-40">
      <CardHeader>
        <section>
          <Link to={`/board/${id}`} className="cursor-pointer">
            <h2 className="text-2xl font-semibold text-text-light">{nombre}</h2>
          </Link >
          <span className="text-xs font-normal text-primary-medium">Creado por {label}</span>
        </section>
        <CardActions />
      </CardHeader>
      <CardBody>
        <p className="text-base font-normal text-text-muted">{descripcion}</p>
      </CardBody>
    </article>
  );
};
