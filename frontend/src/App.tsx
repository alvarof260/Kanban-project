import { BrowserRouter, Route } from "react-router";
import { SessionProvider } from "./context/session.context";
import { Boards, Login } from "./pages";
import { Routes } from "react-router";
import { PrivateGuard } from "./guard/PrivateGuard";

function App() {
  return (
    <BrowserRouter>
      <SessionProvider>
        <Routes>
          <Route path="/" element={<Login />} />
          <Route element={<PrivateGuard />}>
            <Route path="/boards" element={<Boards />} />
          </Route>
        </Routes>
      </SessionProvider>
    </BrowserRouter>
  );
}

export default App;
