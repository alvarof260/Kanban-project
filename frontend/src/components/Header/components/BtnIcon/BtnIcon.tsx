import { ReactNode } from "react"

interface Props {
  link?: string
  children: ReactNode
  handleClick?: () => void
}

export const BtnIcon = ({ link, children, handleClick }: Props) => {

  return (
    <a
      className="text-text-100 p-2 hover:not-focus:bg-bg-300 rounded-md cursor-pointer"
      href={link}
      target="_blank"
      referrerPolicy="no-referrer"
      onClick={handleClick}
    >
      {children}
    </a>
  )
}
