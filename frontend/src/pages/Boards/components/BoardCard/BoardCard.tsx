import { ReactNode } from "react";


interface Props {
  children: ReactNode;
}

export const BoardCard = ({ children }: Props) => {

  return (
    <article className={"bg-transparent rounded-md border border-accent-dark/30 p-6 w-86 flex flex-col justify-between h-60 "}>
      {children}
    </article >
  );
};
