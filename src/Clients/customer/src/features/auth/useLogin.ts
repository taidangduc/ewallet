import React from "react";
import { getUserInfo, login } from "./auth.service";
import { useTokenStorage } from "../../hooks/useTokenStorage";
import { useAuth } from "./auth.context";
import type { LoginRequest } from "./auth.type";

export function useLogin() {
  const { setUser } = useAuth();
  const { setToken } = useTokenStorage();

  const [loading, setLoading] = React.useState<boolean>(false);

  const signIn = async (request: LoginRequest): Promise<void> => {
    setLoading(true);
    try {
      var response = await login(request);
      setToken(response.data.accessToken);
      const userInfo = await getUserInfo();
      setUser(userInfo.data);
    } finally {
      setLoading(false);
    }
  };

  return { loading, signIn };
}
