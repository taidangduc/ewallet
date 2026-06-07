import arrowUpRight from "@assets/arrow-up-right-bg-black.png";
import arrowDownLeft from "@assets/arrow-down-left-bg-white.png";
import { Dialog } from "../../components/ui/Dialog";
import React from "react";
import { TransactionForm } from "./TransactionForm";
import { TransactionType } from "./transaction.type";

export function TransactionHeader() {
  const [state, setState] = React.useState<boolean>(false);
  const [formType, setFormType] = React.useState<TransactionType | undefined>(
    undefined,
  );

  const handleOpenDialog = (type: TransactionType) => {
    setState(true);
    setFormType(type);
  };

  return (
    <>
      <div className="flex">
        <button
          className="flex-1 bg-blue-500 text-white px-4 py-2 outline-1 outline-gray-300"
          onClick={() => handleOpenDialog(TransactionType.WITHDRAW)}
        >
          <div className="flex items-center justify-center">
            <img src={arrowDownLeft} alt="Arrow Down Left" />
            <span className="text-lg">Withdraw</span>
          </div>
        </button>
        <button
          className="flex-1 bg-white text-black px-4 py-2 ml-4 outline outline-1 outline-gray-300"
          onClick={() => handleOpenDialog(TransactionType.DEPOSIT)}
        >
          <div className="flex items-center justify-center">
            <img src={arrowUpRight} alt="Arrow Up Right" />
            <span className="text-lg">Deposit</span>
          </div>
        </button>
        <Dialog open={state} onClose={() => {}}>
          <TransactionForm
            open={state}
            onClose={() => setState(false)}
            formType={formType}
          />
        </Dialog>
      </div>
    </>
  );
}
