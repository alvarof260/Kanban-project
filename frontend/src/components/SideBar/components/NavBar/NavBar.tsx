import { ClipBoard, Plus } from "../../../../icons";
import { NavBarItem } from "../NavBarItem/NavBarItem";

export const NavBar = () => {
  return (
    <section className="flex-1">
      <nav>
        <ul className="flex flex-col gap-2">
          <NavBarItem label="Tableros">
            <ClipBoard height="1.5em" width="1.5em" />
          </NavBarItem>
          <NavBarItem label="Crear Tablero">
            <Plus height="1.5em" width="1.5em" />
          </NavBarItem>
        </ul>
      </nav>
    </section>
  );
};
