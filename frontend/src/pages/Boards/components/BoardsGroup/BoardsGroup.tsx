import { ReactNode } from "react";

interface BoardGroupProps {
  children: ReactNode;
}

export const BoardGroup = ({ children }: BoardGroupProps) => {
  return (
    <section className="flex flex-row gap-8 items-center justify-start">
      {children}
    </section>
  );
};
