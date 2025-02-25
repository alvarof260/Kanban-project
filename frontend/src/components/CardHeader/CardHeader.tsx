import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

export const CardHeader = ({ children }: Props) => {
  return (
    <section className="flex flex-row h-auto w-full justify-between">
      {children}
    </section>
  );
};
