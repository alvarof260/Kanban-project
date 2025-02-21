import { Plus } from "../../../../icons";
import { Modals } from "../../Boards";

interface ButtonAddBoardProps {
  onModal: (newState: Modals) => void;
}

export const ButtonAddBoard = ({ onModal }: ButtonAddBoardProps) => {
  return (
    <div className="relative flex justify-center">
      <button
        className="bg-transparent rounded-md border border-accent-dark/30 p-6 min-w-86 h-50 text-text-light flex justify-center items-center font-medium text-2xl cursor-pointer hover:bg-background-tertiary/70 relative group"
        onClick={() => onModal("create")}
      >
        <Plus />
        {/* Tooltip */}
        <span className="absolute top-full mt-2 left-1/2 -translate-x-1/2 bg-text-light text-background-primary text-sm font-medium px-3 py-1 rounded-md opacity-0 group-hover:opacity-100 transition-opacity duration-300 whitespace-nowrap shadow-lg">
          Agregar Tablero
        </span>
      </button>
    </div>
  );
};
