import { Logo } from "../Logo/Logo"
import { Button } from "../Button/Button"


export const Navbar = () => {
  return (
    <header className='bg-bg-100 w-screen h-12 flex flex-row items-center justify-between px-5'>
      <Logo />

      <section>
        <Button type="btn-icon">github</Button>
        <Button type="btn-icon">theme</Button>
      </section>
    </header>
  )
}
