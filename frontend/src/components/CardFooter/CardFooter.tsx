import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}


export const CardFooter = ({ children }: Props) => {
  return (
    <section className="flex flex-row justify-between items-center">
      {children}
    </section>
  );
};
