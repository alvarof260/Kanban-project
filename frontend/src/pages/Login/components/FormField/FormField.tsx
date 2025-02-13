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
    <section className="flex flex-col justify-start gap-2 w-full">
      <label className='text-md font-medium text-gray-50' htmlFor={name}>{label}</label>
      <input
        className='bg-gray-600 rounded-xs px-1 py-2'
        id={name}
        name={name}
        type={type}
        maxLength={maxLength}
        placeholder={placeholder}
        value={value}
        onChange={onChangeValue}
      />
      {error && <p className="text-xs font-semibold text-red-500">{error.message}</p>}
    </section>
  );
};
