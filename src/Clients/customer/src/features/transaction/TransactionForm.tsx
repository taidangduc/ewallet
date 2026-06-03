import React from "react";
import { TransactionAmount } from "./TransactionAmount";
import { TransactionCardSelect } from "./TransactionCardSelect";
import type { TransactionType } from "./transaction.type";
import { cardTest } from "../../types/mock";
import { toast } from "sonner";

type DialogProps = {
  open: boolean;
  onClose: () => void;
  formType: TransactionType | undefined;
};

export function TransactionForm({ open, onClose, formType }: DialogProps) {
  const [cardId, setCardId] = React.useState<string>(cardTest[0].id);
  const [amount, setAmount] = React.useState(100);

  const onSubmitForm = (event: React.FormEvent) => {
    event.preventDefault();

    onClose();

    toast.success(`${formType} of $${amount} submitted successfully!`);
  };

  const onCancelForm = (event: React.MouseEvent<HTMLButtonElement>) => {
    event.stopPropagation();
    onClose();
  };

  return (
    <form className="flex flex-col" onSubmit={onSubmitForm}>
      <h1 className="text-2xl font-medium">{formType}</h1>
      <TransactionAmount amount={amount} currency="USD" onChange={setAmount} />
      <TransactionCardSelect
        data={cardTest}
        cardId={cardId}
        onChange={setCardId}
      />
      <div className="flex gap-4">
        <button
          className="flex-1 bg-blue-500 text-white px-4 py-2 mt-4"
          type="submit"
        >
          Submit
        </button>
        <button
          className="flex-1 bg-white outline outline-gray-300 text-black px-4 py-2 mt-4"
          type="button"
          onClick={onCancelForm}
        >
          Cancel
        </button>
      </div>
    </form>
  );
}
