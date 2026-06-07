import React from "react";
import { cardTest } from "../../types/mock";
import { createTransaction } from "./transaction.service";

export function useCreateTransaction() {
  const [cardId, setCardId] = React.useState<string>(cardTest[0].id);
  const [amount, setAmount] = React.useState(100);

  const create = async () => {
    
    await createTransaction({ cardId, amount });
  }
}
