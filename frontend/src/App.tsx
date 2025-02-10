import { Header, SideBar } from './components';
import { ReactNode } from 'react';

interface Props {
  children: ReactNode;
}

function App({ children }: Props) {

  return (
    <>
      <Header />
      <div className='flex flex-row h-[calc(100vh - 48px)]'>
        <SideBar />
        <main>
          {children}
        </main>
      </div>
    </>
  );
}

export default App;
