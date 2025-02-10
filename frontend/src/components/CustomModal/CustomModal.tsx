import { ReactNode } from "react";

interface Props {
  children: ReactNode;
}

export const CustomModal = ({ children }: Props) => {
  return (
    <section className="fixed w-screen h-screen inset-0 flex justify-center items-center">
      <article className="bg-green-500 px-4 rounded-md w-48 h-48 flex flex-col justify-around items-center">
        {children}
      </article>
    </section>
  );
};
