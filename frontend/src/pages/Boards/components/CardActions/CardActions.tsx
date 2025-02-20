import { EllipsisHorizontal } from "../../../../icons";

export const CardActions = () => {
  return (

    <button
      className="text-text-light font-medium text-sm w-8 h-8 cursor-pointer flex justify-center items-center rounded-md hover:bg-background-tertiary/70 transition duration-300 ease-in"
    >
      <EllipsisHorizontal />
    </button>
  );
};
