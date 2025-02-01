import { ReactNode } from "react";

type typeButtons = "btn-icon" | "btn-primary" | "btn-secundary";

interface Props {
  type: typeButtons
  children: ReactNode
}

export const Button = ({ type, children }: Props) => {
  const buttons = {
    "btn-icon": "bg-red-500",
    "btn-primary": "",
    "btn-secundary": ""
  }

  return (
    <button className={`${buttons[type]}`}>
      {children}
    </button>
  )
}
