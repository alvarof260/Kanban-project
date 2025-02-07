
interface Props {
  name: string;
  description: string;
  owner: string | undefined;
}

export const BoardCard = ({ name, description, owner }: Props) => {
  return (
    <article
      className="bg-bg-200 px-2 py-4 rounded-md flex flex-col justify-between items-start"
    >
      <section>
        <h3 className="font-poppins text-xl font-semibold text-text-100">{name}</h3>
        <p className="font-poppins font-medium text-md text-text-200">{description}</p>
      </section>
      <p className="bg-primary-100 px-2 py-1 rounded-md text-primary-300 w-max font-poppins text-xs font-light">{owner}</p>
    </article>
  );
};
