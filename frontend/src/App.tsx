import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Login, SideBar } from './components';
import { AuthProvider } from './context/auth.context';

// Estructura de clases de tailwind:
// Caja - flex/grid - Fuente - Colores - Transiciones

function App() {


  return (
    <>
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route path='/' element={<Login />} />
            <Route path='/home' element={<SideBar />} />
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </>
  );
}

export default App;
