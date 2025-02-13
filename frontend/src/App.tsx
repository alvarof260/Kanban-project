import { SessionProvider } from "./context/session.context";
import { Login } from "./pages";

function App() {
  return (
    <SessionProvider>
      <div className='w-screen h-screen bg-gray-100 flex justify-center items-center'>
        <Login />
      </div>
    </SessionProvider>
  );
}

export default App;
