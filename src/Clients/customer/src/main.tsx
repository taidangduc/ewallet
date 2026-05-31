import React from "react";
import { createRoot } from "react-dom/client";
import "./style.css";
import { App } from "./App.tsx";
import { HomePage } from "./pages/Home.tsx";
import { NotFoundPage } from "./pages/NotFound.tsx";
import { createBrowserRouter } from "react-router-dom";
import { LoginPage } from "./pages/Login.tsx";

const router = createBrowserRouter([
  {
    path: "/",
    element: <HomePage />,
  },
  {
    path: "/login",
    element: <LoginPage />,
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
