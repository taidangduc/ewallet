import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { AuthProvider } from "./features/auth/auth.context";
import { Toaster } from "./components/ui/Sonner";

type AppProps = { router: ReturnType<typeof createBrowserRouter> };
export function App({ router }: AppProps) {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
      <Toaster />
    </AuthProvider>
  );
}
