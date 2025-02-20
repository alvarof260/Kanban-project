import { ChangeEvent } from "react";
import { FormErrors } from "..";

interface FormFieldProps {
  label: string;
  name: string;
  type: string;
  maxLength: number;
  placeholder?: string;
  value: string;
  onChangeValue: (e: ChangeEvent<HTMLInputElement>) => void;
  error?: FormErrors[keyof FormErrors];
}

export const FormField = ({ label, name, type, maxLength, placeholder, value, onChangeValue, error }: FormFieldProps) => {

  return (
    <section className="flex flex-col justify-start gap-2 h-28 w-full">
      <label className='text-sm font-medium text-text-light' htmlFor={name}>{label}</label>
      <input
        className='border border-accent-dark/30 bg-transparent rounded-md px-3 py-2 text-sm text-text-muted outline-none focus:border-accent-light'
        id={name}
        name={name}
        type={type}
        maxLength={maxLength}
        placeholder={placeholder}
        value={value}
        onChange={onChangeValue}
      />
      {error && <p className="text-xs font-medium text-red-500/70 mt-2">{error.message}</p>}
    </section>
  );
};
