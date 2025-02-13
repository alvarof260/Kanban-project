import { ReactNode } from "react";

interface WrapperFormProps {
  children: ReactNode;
}

export const WrapperForm = ({ children }: WrapperFormProps) => {
  return (
    <section className='bg-gray-800 p-4 rounded-xs'>
      {children}
    </section>
  );
};
