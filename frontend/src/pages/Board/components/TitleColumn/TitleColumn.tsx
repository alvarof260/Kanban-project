
interface TitleColumnProps {
  name: string;
}

export const TitleColumn = ({ name }: TitleColumnProps) => {
  return (
    <h3 className="bg-gray-300  py-2 font-bold text-center text-red-500">{name}</h3>
  );
};

