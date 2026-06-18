import React from "react";
import type { RegisterRequest } from "./auth.type";
import { register } from "./auth.service";
import { getErrorMessage } from "../../lib/problem-details";

export function useRegister() {
  const [loading, setLoading] = React.useState<boolean>(false);
  const [error, setError] = React.useState<string | null>(null);

  const signUp = async (request: RegisterRequest): Promise<void> => {
    setLoading(true);
    setError(null);
    try {
      await register(request);
    } catch (err: any) {
      setError(getErrorMessage(err));
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return { signUp, loading, error };
}
