import { ReactNode } from "react";

interface BoardGroupProps {
  children: ReactNode;
}

export const BoardGroup = ({ children }: BoardGroupProps) => {
  return (
    <section className="w-full flex flex-row gap-4 flex-wrap justify-start items-start">
      {children}
    </section>
  );
};
