import { LogOut, Setting } from "../../../../icons"
import { NavBarItem } from "../NavBarItem/NavBarItem"

export const UserActions = () => {
  return (
    <section className="border-t border-text-200 pt-4">
      <ul className="flex flex-col gap-2">
        <NavBarItem label="Configuracion">
          <Setting height="1.5em" width="1.5em" />
        </NavBarItem>
        <NavBarItem label="Salir">
          <LogOut height="1.5em" width="1.5em" />
        </NavBarItem>
      </ul>
    </section>
  )
}
