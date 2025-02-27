import { BrowserRouter, Route } from "react-router";
import { SessionProvider } from "./contexts/session.context";
import { BoardKanban, Boards, Dashboard, Login } from "./pages";
import { Routes } from "react-router";
import { AdminGuard, PrivateGuard } from "./guards";
import { Header } from "./layouts";

function App() {
  return (
    <BrowserRouter>
      <SessionProvider>
        <Routes>
          <Route path="/" element={<Login />} />
          <Route element={<PrivateGuard />}>
            <Route path="/boards" element={
              <main className="flex flex-col">
                <Header />
                <Boards />
              </main>
            } />
            <Route path="/board/:idBoard" element={
              <main className="flex flex-col">
                <Header />
                <BoardKanban />
              </main>
            } />
            <Route element={<AdminGuard />} >
              <Route path="/dashboard" element={
                <main className="flex flex-col">
                  <Header />
                  <Dashboard />
                </main>
              } />
            </Route>
          </Route>
        </Routes>
      </SessionProvider>
    </BrowserRouter>
  );
}

export default App;
