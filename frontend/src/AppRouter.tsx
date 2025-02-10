import { BrowserRouter, Route, Routes } from "react-router-dom";
import { AuthProvider } from "./context/auth.context";
import { Login } from "./components";
import App from "./App";
import { PrivateGuard } from "./guard/PrivateGuard";
import { Home } from "./pages";

export const AppRouter = () => {

  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Login />} />
          <Route element={<PrivateGuard />}>
            <Route path="/home" element={<App><Home /></App>} />
          </Route>
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
};
