import { Github, Sun } from "../../icons/"
import { BtnIcon } from "../BtnIcon/BtnIcon"
import { Logo } from "../Logo/Logo"


export const Header = () => {
  return (
    <header className='bg-bg-100 w-screen h-12 flex flex-row items-center justify-between px-5'>
      <Logo />

      <section className="flex flex-row items-center gap-2">
        <BtnIcon link="https://github.com/alvarof260/Kanban-project">
          <Github height="1em" width="1em" />
        </BtnIcon>
        {/* Necesitamos context para cambiar theme */}
        <BtnIcon>
          <Sun height="1em" width="1em" />
        </BtnIcon>
      </section>
    </header>
  )
}
