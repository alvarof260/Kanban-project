import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

export const CardBody = ({ children }: Props) => {
  return (
    <section className="pt-6">
      {children}
    </section>
  );
};
