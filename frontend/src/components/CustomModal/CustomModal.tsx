import { ReactNode } from "react";
import { XMark } from "../../icons";
import { Modals } from "../../pages";

interface CustomModalProps {
  children: ReactNode;
  onModal: (newState: Modals) => void;
}

export const CustomModal = ({ children, onModal }: CustomModalProps) => {
  return (
    <section className="fixed w-screen h-screen inset-0 flex justify-center items-center bg-black/75">
      <article className="relative bg-background-secondary border border-accent-dark/30 rounded-md p-6 w-98 flex flex-col">
        <button
          className="absolute top-3 right-3 h-8 w-8 text-accent-dark/70 text-sm cursor-pointer flex justify-center items-center hover:text-text-light transition duration-300 ease-in "
          onClick={() => onModal("none")}
        >
          <XMark />
        </button>
        <h2></h2>
        {children}
      </article>
    </section>
  );
};
