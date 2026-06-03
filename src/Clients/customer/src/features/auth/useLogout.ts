import { useTokenStorage } from "../../hooks/useTokenStorage";
import { useAuth } from "./auth.context";

export function useLogout() {
  const { setUser } = useAuth();
  const { clearToken } = useTokenStorage();

  const logout = () => {
    clearToken();
    setUser(null);
  };

  return { logout };
}
