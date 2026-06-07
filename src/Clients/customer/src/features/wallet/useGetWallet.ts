import React from "react";
import { getWallet } from "./wallet.service";
import type { Wallet } from "./wallet.type";
import { useSession } from "../auth/useSession";

export function useGetWallet() {
  const { isAuthenticated } = useSession();

  const [wallet, setWallet] = React.useState<Wallet | null>(null);
  const [loading, setLoading] = React.useState<boolean>(true);
  const [error, setError] = React.useState<string | null>(null);

  const fetch = async () => {
    if (!isAuthenticated) return;
    try {
      const { data } = await getWallet();
      setWallet(data);
    } catch {
      setError("Failed to fetch wallet data");
    } finally {
      setLoading(false);
    }
  };

  const refresh = () => {
    setLoading(true);
    setError(null);
    fetch();
  };

  React.useEffect(() => {
    fetch();
  }, []);

  return { wallet, loading, error, refresh };
}
