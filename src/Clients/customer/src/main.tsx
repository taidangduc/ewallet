import React from "react";
import { createRoot } from "react-dom/client";
import "./style.css";
import { App } from "./App.tsx";
import { HomePage } from "./pages/Home.tsx";
import { NotFoundPage } from "./pages/NotFound.tsx";
import { createBrowserRouter } from "react-router-dom";
import { LoginPage } from "./pages/Login.tsx";
import { AuthGuard } from "./features/auth/AuthGuard.tsx";
import { RegisterPage } from "./pages/Register.tsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: (
      <AuthGuard>
        <HomePage />
      </AuthGuard>
    ),
  },
  {
    path: "/login",
    element: <LoginPage />,
  },
  {
    path: "/register",
    element: <RegisterPage />,
  },
  {
    path: "*",
    element: <NotFoundPage />,
  },
]);

createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <App router={router} />
  </React.StrictMode>,
);
