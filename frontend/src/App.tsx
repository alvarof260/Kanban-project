import { Header, SideBar } from './components';
import { ReactNode } from 'react';

// Estructura de clases de tailwind:
// Caja - flex/grid - Fuente - Colores - Transiciones

interface Props {
  children: ReactNode;
}

function App({ children }: Props) {

  return (
    <>
      <Header />
      <div className='flex flex-row'>
        <SideBar />
        <main>
          {children}
        </main>
      </div>
    </>
  );
}

export default App;
