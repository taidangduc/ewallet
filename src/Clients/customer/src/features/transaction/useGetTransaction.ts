import React from "react";
import type { Transaction } from "./transaction.type";
import { getTransactions } from "./transaction.service";

export function useGetTransaction() {
  const [transactions, setTransactions] = React.useState<Transaction[]>([]);
  const [loading, setLoading] = React.useState<boolean>(true);
  const [error, setError] = React.useState<string | null>(null);

  const fetch = async () => {
    try {
      const { data } = await getTransactions();
      setTransactions(data);
    } catch (error) {
      setError((error as Error).message);
    } finally {
      setLoading(false);
    }
  };

  const refresh = async () => {
    setLoading(true);
    setError(null);
    await fetch();
  };

  React.useEffect(() => {
    fetch();
  }, []);

  return { transactions, loading, error, refresh };
}
