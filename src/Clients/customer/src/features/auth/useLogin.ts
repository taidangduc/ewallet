import React from "react";
import { getUserInfo, login } from "./auth.service";
import { useTokenStorage } from "../../hooks/useTokenStorage";
import { useAuth } from "./auth.context";

export function useLogin() {
  const { setUser } = useAuth();
  const { setToken } = useTokenStorage();

  const [loading, setLoading] = React.useState<boolean>(false);

  const signIn = async ({
    username,
    password,
  }: {
    username: string;
    password: string;
  }): Promise<void> => {
    setLoading(true);
    try {
      var response = await login({ username, password });
      setToken(response.data.accessToken);
      const userInfo = await getUserInfo();
      setUser(userInfo.data);
    } finally {
      setLoading(false);
    }
  };

  return { loading, signIn };
}
