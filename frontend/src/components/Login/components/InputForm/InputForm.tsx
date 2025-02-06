import { Control, Controller, FieldError } from "react-hook-form";
import { LoginValues } from "../../../../models";

interface Props {
  name: keyof LoginValues;
  label: string;
  control: Control<LoginValues>;
  type?: string;
  error?: FieldError;
}

export const InputForm = ({ name, label, control, type, error }: Props) => {
  return (
    <section className="flex flex-col gap-2 w-full h-24">
      <label className="text-text-100 font-poppins text-xs" htmlFor={name}>{label}</label>
      <Controller
        name={name}
        control={control}
        render={({ field }) => <input id={name} type={type} {...field} className="border-b-2 border-b-primary-200 outline-0 rounded-md text-text-200 text-md font-poppins p-1" />}
      />
      {error && <p className="text-red-500 font-poppins text-sm font-bold">{error.message}</p>}
    </section>
  );
};
