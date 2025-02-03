import { Header, SideBar } from './components'
import { AuthProvider } from './context/auth.context'

// Estructura de clases de tailwind:
// Caja - flex/grid - Fuente - Colores - Transiciones



function App() {


  return (
    <>
      <Header />
      <AuthProvider>
        <SideBar />
      </AuthProvider>
    </>
  )
}

export default App
