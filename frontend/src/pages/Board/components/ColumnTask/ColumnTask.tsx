import { ReactNode } from "react";

interface ColumnTaskProps {
  children: ReactNode;
}

export const ColumnTask = ({ children }: ColumnTaskProps) => {
  return (
    <article className="flex-1 px-2">
      {children}
    </article>
  );
};
