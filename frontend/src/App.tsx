import { BrowserRouter, Route } from "react-router";
import { SessionProvider } from "./contexts/session.context";
import { BoardKanban, Boards, Login } from "./pages";
import { Routes } from "react-router";
import { PrivateGuard } from "./guards/PrivateGuard";

function App() {
  return (
    <BrowserRouter>
      <SessionProvider>
        <Routes>
          <Route path="/" element={<Login />} />
          <Route element={<PrivateGuard />}>
            <Route path="/boards" element={<Boards />} />
            <Route path="/board/:idBoard" element={<BoardKanban />} />
          </Route>
        </Routes>
      </SessionProvider>
    </BrowserRouter>
  );
}

export default App;
