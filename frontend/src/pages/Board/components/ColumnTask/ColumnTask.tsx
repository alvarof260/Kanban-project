import { ReactNode } from "react";

interface ColumnTaskProps {
  children: ReactNode;
}

export const ColumnTask = ({ children }: ColumnTaskProps) => {
  return (
    <article className="bg-background-secondary flex-1 flex flex-col gap-6 rounded-md border border-accent-dark/30 p-6">
      {children}
    </article>
  );
};
