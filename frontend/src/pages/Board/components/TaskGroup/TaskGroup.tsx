import { ReactNode } from "react";

interface GroupTaskProps {
  children: ReactNode;
  onOpenModal: () => void;
}

export const GroupTask = ({ children, onOpenModal }: GroupTaskProps) => {
  return (
    <ul className="flex flex-col gap-2 px-2 pt-5 justify-center items-center">
      {children}
      <li className="bg-gray-500 w-full h-20 rounded-xs flex justify-center items-center text-xl font-bold text-gray-50 cursor-pointer" onClick={onOpenModal}>
        +
      </li>
    </ul>
  );
};
