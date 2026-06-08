import React from "react";
import { cardTest } from "../../types/card";
import type { TransactionRequest } from "./transaction.type";
import { createTransaction } from "./transaction.service";

export function useCreateTransaction() {
  const [cardId, setCardId] = React.useState<string>(cardTest[0].id);
  const [amount, setAmount] = React.useState(100);
  const [loading, setLoading] = React.useState<boolean>(false);

  const create = async (request: TransactionRequest) => {
    setLoading(true);
    try {
      await new Promise((x) => setTimeout(x, 1000));
      await createTransaction(request);
    } finally {
      setLoading(false);
    }
  };

  return { cardId, setCardId, amount, setAmount, create, loading };
}
