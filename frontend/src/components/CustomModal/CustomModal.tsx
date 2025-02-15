
interface CustomModalProps {
  children: ReactNode;
  onModal: () => void;
}

export const CustomModal = ({ children, onModal }: CustomModalProps) => {
  return (
    <section className="fixed w-screen h-screen inset-0 flex justify-center items-center bg-black/75">
      <article className="bg-gray-800 px-4 py-6 rounded-xs w-98 h-88 flex flex-col justify-around items-center">
        {children}
        <button
          className="bg-red-500 px-2 rounded-xs cursor-pointer w-full py-2"
          onClick={onModal}
        >
          cerrar
        </button>
      </article>
    </section>
  );
};
