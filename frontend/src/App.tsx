import { BrowserRouter, Route } from "react-router";
import { SessionProvider } from "./contexts/session.context";
import { BoardKanban, Boards, Dashboard, Login } from "./pages";
import { Routes } from "react-router";
import { AdminGuard, PrivateGuard } from "./guards";

function App() {
  return (
    <BrowserRouter>
      <SessionProvider>
        <Routes>
          <Route path="/" element={<Login />} />
          <Route element={<PrivateGuard />}>
            <Route path="/boards" element={<Boards />} />
            <Route path="/board/:idBoard" element={<BoardKanban />} />
            <Route element={<AdminGuard />} >
              <Route path="/dashboard" element={<Dashboard />} />
            </Route>
          </Route>
        </Routes>
      </SessionProvider>
    </BrowserRouter>
  );
}

export default App;
