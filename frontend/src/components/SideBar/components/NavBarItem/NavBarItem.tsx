import { ReactNode } from "react";

interface Props {
  children: ReactNode
  label: string
}

export const NavBarItem = ({ children, label }: Props) => {
  return (
    <li
      className='w-full p-2 rounded-md hover:not-focus:bg-bg-300 cursor-pointer 
          flex flex-row gap-1 items-center text-text-100 transition-all duration-200 ease-in-out'>
      {children}
      <p className='text-lg font-poppins'>{label}</p>
    </li>
  );
};
