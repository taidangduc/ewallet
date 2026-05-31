import React from "react";

// ref: https://www.w3schools.com/typescript/typescript_react.php
const AuthContext = React.createContext<{
  user: string | null;
  setUser: React.Dispatch<React.SetStateAction<string | null>>;
  login: () => Promise<void>;
  isAuthenticated: boolean;
} | null>(null);

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [user, setUser] = React.useState<string | null>(null);

  const login = async (): Promise<void> => {
    window.location.href = "/";
  };

  const ctx = React.useMemo(
    () => ({ user, setUser, login, isAuthenticated: !!user }),
    [user],
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
