import React from "react";
import { getUserInfo, login } from "./auth.service";
import { useTokenStorage } from "../../hooks/useTokenStorage";
import { useAuth } from "./auth.context";
import type { LoginRequest } from "./auth.type";
import { getErrorMessage } from "../../lib/problem-details";

export function useLogin() {
  const { setUser } = useAuth();
  const { setToken } = useTokenStorage();

  const [loading, setLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string | null>(null);

  const signIn = async (request: LoginRequest): Promise<void> => {
    setLoading(true);
    setError(null);
    try {
      var response = await login(request);
      setToken(response.data.accessToken);
      const userInfo = await getUserInfo();
      setUser(userInfo.data);
    } catch (err: any) {
      setError(getErrorMessage(err));
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { signIn, loading, error };
}
