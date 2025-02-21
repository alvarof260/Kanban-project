import { Link } from "react-router";
import { CardActions, CardBody, CardHeader } from "..";
import { useState } from "react";
import { CardFooter } from "../CardFooter/CardFooter";

interface BoardCardProps {
  id: number;
  label: string;
  nombre: string;
  descripcion: string;
}

export const BoardCard = ({ id, label, nombre, descripcion }: BoardCardProps) => {
  const [showMore, setShowMore] = useState<boolean>(false);
  const [actions, setActions] = useState<boolean>(false);

  return (
    <article className={`bg-transparent rounded-md border border-accent-dark/30 p-6 w-86 flex flex-col ${showMore ? "h-60" : "h-50"}`}>
      <CardHeader>
        <Link to={`/board/${id}`} className="cursor-pointer">
          <h2 className="text-2xl font-semibold text-text-light hover:underline">{nombre}</h2>
        </Link >
        <CardActions isOpen={actions} onOpenActions={() => setActions(prevState => !prevState)} />
      </CardHeader >
      <CardBody>
        <section className="overflow-hidden">
          <p className={`text-base font-normal text-text-muted ${showMore ? "" : "line-clamp-2"}`}>{descripcion}</p>
        </section>
      </CardBody>
      <CardFooter>
        <span className="text-xs font-normal text-primary-medium">Creado por {label}</span>
        <button
          disabled={descripcion.length < 100}
          className={`bg-accent-light py-2 px-4 rounded-md text-xs font-medium cursor-pointer hover:bg-primary-light transition ease-in duration-300 disabled:bg-accent-dark`}
          onClick={() => setShowMore(prevState => !prevState)}
        >
          Mostrar Más
        </button>
      </CardFooter>
    </article >
  );
};
