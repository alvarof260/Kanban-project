import { useRef, useEffect } from "react";
import { EllipsisHorizontal } from "../../../../icons";

interface Props {
  isOpen: boolean;
  onOpenActions: () => void;
}

export const CardActions = ({ isOpen, onOpenActions }: Props) => {
  const actionsRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (actionsRef.current && !actionsRef.current.contains(event.target as Node)) {
        onOpenActions(); // Cierra el menú si se hace clic afuera
      }
    };

    if (isOpen) {
      document.addEventListener("mousedown", handleClickOutside);
    } else {
      document.removeEventListener("mousedown", handleClickOutside);
    }

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [isOpen, onOpenActions]);

  return (
    <div className="relative" ref={actionsRef}>
      <button
        className="text-text-light font-medium text-sm w-8 h-8 cursor-pointer flex justify-center items-center rounded-md hover:bg-background-tertiary/70 transition duration-300 ease-in"
        onClick={onOpenActions}
      >
        <EllipsisHorizontal />
      </button>
      {
        isOpen &&
        <article className="absolute top-full mt-1 z-99 right-1 bg-background-primary border border-accent-dark/30 w-40 flex flex-col gap-2 items-start rounded-md p-1 shadow-lg">
          <section className="border-b border-b-accent-dark/30 w-full pb-1">
            <span className="text-text-light font-medium text-sm px-2 py-1.5">Actions</span>
          </section>
          <section className="flex flex-col justify-start items-start gap-1 w-full">
            <button className="outline-none cursor-pointer text-text-light font-normal text-sm px-2 py-1.5 rounded-md hover:bg-background-tertiary/70 transition duration-300 ease-in w-full flex items-start">Editar tablero</button>
            <button className="outline-none cursor-pointer text-text-light font-normal text-sm px-2 py-1.5 rounded-md hover:bg-background-tertiary/70 transition duration-300 ease-in w-full flex items-start">Eliminar tablero</button>
          </section>
        </article>
      }
    </div >
  );
};
