import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

export const CardBody = ({ children }: Props) => {
  return (
    <section className="flex flex-col justify-between ">
      {children}
    </section>
  );
};
