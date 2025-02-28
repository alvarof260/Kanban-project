
interface Props {
  error: string;
  onCloseToast: () => void;
}

export const Toast = ({ error, onCloseToast }: Props) => {
  return (
    <article
      className={`absolute bottom-4 right-4 bg-red-800 h-18 p-2 w-90 border border-accent-dark/30 rounded-md shadow-lg flex items-center 
        transition-all duration-300 ease-in-out transform ${error ? "opacity-100 translate-y-0" : "opacity-0 translate-y-4"
        }`}
    >
      <button
        className="absolute top-1 right-2 text-text-muted text-lg cursor-pointer hover:text-text-light transition ease-in duration-100"
        onClick={onCloseToast}
      >
        x
      </button>
      <p className="text-text-light text-sm">{error}</p>
    </article>
  );
};
