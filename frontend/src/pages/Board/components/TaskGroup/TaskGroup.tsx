import { ReactNode } from "react";

interface GroupTaskProps {
  children: ReactNode;
  isOwnerBoard: boolean;
  onOpenModal: () => void;
}

export const GroupTask = ({ children, isOwnerBoard, onOpenModal }: GroupTaskProps) => {
  return (
    <ul className="flex flex-col gap-2 justify-center items-center">
      {children}

      {
        isOwnerBoard &&
        <li className="bg-background-primary w-full h-20 rounded-md border border-accent-dark/30 p-6 flex justify-center items-center text-2xl font-medium text-text-light cursor-pointer hover:bg-background-tertiary/70 transition ease-in duration-300" onClick={onOpenModal}>
          +
        </li>
      }
    </ul>
  );
};
