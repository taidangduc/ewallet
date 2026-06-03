import React, { useEffect } from "react";
import type { UserDTO } from "./auth.type";
import { useTokenStorage } from "../../hooks/useTokenStorage";
import { getUserInfo } from "./auth.service";

// ref: https://www.w3schools.com/typescript/typescript_react.php
const AuthContext = React.createContext<{
  user: UserDTO | null;
  setUser: React.Dispatch<React.SetStateAction<UserDTO | null>>;
  loading: boolean;
} | null>(null);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = React.useState<UserDTO | null>(null);
  const [loading, setLoading] = React.useState<boolean>(true);

  const { getToken, clearToken } = useTokenStorage();

  useEffect(() => {
    const func = async () => {
      try {
        const token = getToken();
        if (!token) {
          return;
        }

        // delay for testing loading state
        await new Promise((x) => setTimeout(x, 2000));

        const { data } = await getUserInfo();
        setUser(data);
      } catch (error) {
        clearToken();
        setUser(null);
      } finally {
        setLoading(false);
      }
    };

    func();
  }, []);

  const ctx = React.useMemo(
    () => ({
      user,
      setUser,
      loading,
    }),
    [user, setUser, loading],
  );

  return <AuthContext.Provider value={ctx}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const context = React.useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
