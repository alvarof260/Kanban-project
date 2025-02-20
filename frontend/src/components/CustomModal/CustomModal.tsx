import { ReactNode } from "react";
import { XMark } from "../../icons";

interface CustomModalProps {
  children: ReactNode;
  onModal: () => void;
}

export const CustomModal = ({ children, onModal }: CustomModalProps) => {
  return (
    <section className="fixed w-screen h-screen inset-0 flex justify-center items-center bg-black/75">
      <article className="relative bg-background-secondary border border-accent-dark/30 rounded-md p-6 w-98 h-88 flex flex-col">
        <button
          className="absolute top-3 right-3 h-8 w-8 text-accent-dark/70 text-sm cursor-pointer flex justify-center items-center hover:text-text-light transition duration-300 ease-in "
          onClick={onModal}
        >
          <XMark />
        </button>
        {children}
      </article>
    </section>
  );
};
