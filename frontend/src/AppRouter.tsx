import { BrowserRouter, Route, Routes } from "react-router-dom";
import { AuthProvider } from "./context/auth.context";
import { Boards, Login } from "./components";
import App from "./App";
import { PrivateGuard } from "./guard/PrivateGuard";

export const AppRouter = () => {

  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Login />} />
          <Route element={<PrivateGuard />}>
            <Route path="/home" element={<App><Boards /></App>} />
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
};
