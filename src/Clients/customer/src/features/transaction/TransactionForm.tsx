import React from "react";
import { TransactionAmount } from "./TransactionAmount";
import { TransactionCardSelect } from "./TransactionCardSelect";
import {
  TRANSACTION_TYPE,
  TransactionType,
  type TransactionRequest,
} from "./transaction.type";
import { cardTest } from "../../types/card";
import { toast } from "sonner";
import { useCreateTransaction } from "./useCreateTransaction";
import { getErrorMessage } from "../../lib/problem-details";

type DialogProps = {
  open: boolean;
  onClose: () => void;
  formType: TransactionType;
};

export function TransactionForm({ open, onClose, formType }: DialogProps) {
  const { cardId, setCardId, amount, setAmount, create, loading } =
    useCreateTransaction();

  const { label } = TRANSACTION_TYPE[formType];

  const handleSubmitForm = async (event: React.FormEvent) => {
    event.preventDefault();

    const request: TransactionRequest = {
      amount,
      type: formType,
      cardId,
    };

    if (!formType) {
      toast.error("Invalid transaction type");
      return;
    }

    if (amount <= 0) {
      toast.error("Amount must be greater than 0");
      return;
    }

    if (!cardId) {
      toast.error("Card is required");
      return;
    }

    try {
      await create(request);
      toast.success("Transaction created successfully");
      onClose();
    } catch (err) {
      toast.error(getErrorMessage(err));
    }
  };

  return (
    <form className="flex flex-col" onSubmit={handleSubmitForm}>
      <h1 className="text-2xl font-medium">{label}</h1>
      <TransactionAmount amount={amount} currency="USD" onChange={setAmount} />
      <TransactionCardSelect
        data={cardTest}
        cardId={cardId}
        onChange={setCardId}
      />
      <div className="flex gap-4">
        <button
          className={`flex-1 text-white px-4 py-2 mt-4 ${loading ? "cursor-not-allowed bg-blue-100" : "cursor-pointer bg-blue-500"}`}
          type="submit"
          disabled={loading}
        >
          Submit
        </button>
        <button
          disabled={loading}
          className={`flex-1 bg-white outline outline-gray-300 text-black px-4 py-2 mt-4 ${loading ? "cursor-not-allowed" : "cursor-pointer"}`}
          type="button"
          onClick={onClose}
        >
          Cancel
        </button>
      </div>
    </form>
  );
}
