
interface ButtonAddBoardProps {
  onModal: () => void;
}

export const ButtonAddBoard = ({ onModal }: ButtonAddBoardProps) => {
  return (
    <button
      className="bg-green-500 h-20 w-20 rounded-full text-3xl font-bold text-gray-50 flex items-center justify-center"
      onClick={onModal}
    >
      +
    </button>
  );
};
