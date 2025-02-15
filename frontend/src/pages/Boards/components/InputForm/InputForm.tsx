import { ChangeEvent } from "react";

interface InputFormProps {
  label: string;
  type: string;
  placeholder?: string;
  name: string;
  value: string;
  onChange: (e: ChangeEvent<HTMLInputElement>) => void;
  maxLength?: number;
}

export const InputForm = ({ label, type, placeholder, name, value, onChange, maxLength }: InputFormProps) => {
  return (
    <>
      <label className="text-gray-50" htmlFor={name}>{label}</label>
      <input className="text-gray-50" type={type} id={name} placeholder={placeholder} name={name} value={value} onChange={onChange} required maxLength={maxLength} />
    </>
  );
};
