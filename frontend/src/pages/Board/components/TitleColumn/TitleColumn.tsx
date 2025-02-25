import { Task } from "../../../../models";

interface TitleColumnProps {
  name: string;
  tasks: Task[]
}

export const TitleColumn = ({ name, tasks }: TitleColumnProps) => {
  return (
    <header className="flex flex-row justify-between items-center">
      <h3 className="text-text-light font-medium text-base">{name}</h3>
      <span className="text-text-light text-sm bg-background-tertiary rounded-full w-6 h-6 flex justify-center items-center">{tasks.length}</span>
    </header>
  );
};

