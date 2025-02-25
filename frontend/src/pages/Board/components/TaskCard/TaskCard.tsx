import { ReactNode } from "react";

interface TaskCardProps {
  children: ReactNode;
}

export const TaskCard = ({ children }: TaskCardProps) => {
  return (
    <li className={`bg-background-primary border border-accent-dark/30 rounded-md w-full h-40 p-6 flex flex-col justify-between`}>
      {children}
    </li>
  );
};
