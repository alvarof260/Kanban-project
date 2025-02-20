import { Control, Controller, FieldError, FieldValues, Path } from "react-hook-form";

interface Props<T extends FieldValues> {
  name: Path<T>;
  label: string;
  control: Control<T>;
  type?: string;
  placeholder?: string;
  error?: FieldError;
}

export const InputForm = <T extends FieldValues>({ name, label, control, type, placeholder, error }: Props<T>) => {
  return (
    <section className="flex flex-col justify-start gap-2 h-28 w-full">
      <label className='text-sm font-medium text-text-light' htmlFor={name.toString()}>{label}</label>
      <Controller
        name={name}
        control={control}
        render={({ field }) =>
          <input
            id={name.toString()}
            type={type}
            placeholder={placeholder}
            {...field}
            className="border border-accent-dark/30 bg-transparent rounded-md px-3 py-2 text-sm text-text-muted outline-none focus:border-accent-light"
          />
        }
      />
      {error && <p className="text-xs font-medium text-red-500/70 mt-2">{error.message}</p>}
    </section>
  );
};
