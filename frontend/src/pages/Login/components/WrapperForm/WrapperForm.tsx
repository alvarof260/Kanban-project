import { ReactNode } from "react";

interface WrapperFormProps {
  children: ReactNode;
}

export const WrapperForm = ({ children }: WrapperFormProps) => {
  return (
    <section className='bg-background-primary p-6 rounded-md w-96 h-90 border border-accent-dark/30'>
      {children}
    </section>
  );
};
