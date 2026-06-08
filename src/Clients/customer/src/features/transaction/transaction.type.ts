import deposit_icon from "@assets/arrow-up-right-bg-black.png";
import withdraw_icon from "@assets/arrow-down-left-bg-black.png";
import error_icon from "@assets/error-icon.png";

export type TransactionRequest = {
  amount: number;
  type: TransactionType;
  cardId: string;
};

export type Transaction = {
  id: string;
  walletId: string;
  amount: number;
  type: TransactionType;
  status: TransactionStatus;
  description?: string;
  date: string;
};

export enum TransactionType {
  DEPOSIT = 1,
  WITHDRAW = 2,
}

export enum TransactionStatus {
  PENDING = 1,
  SUCCESS = 2,
  FAILED = 3,
}

export type TransactionTypeValue = {
  label: string;
  sign: string;
  icon: string;
  textColor?: string;
};

export type TransactionStatusValue = {
  label: string;
  value: string;
  bgColor?: string;
};

export const TRANSACTION_STATUS: Record<
  TransactionStatus,
  TransactionStatusValue
> = {
  [TransactionStatus.PENDING]: {
    label: "Pending",
    value: "pending",
    bgColor: "bg-yellow-500",
  },
  [TransactionStatus.SUCCESS]: {
    label: "Success",
    value: "success",
    bgColor: "bg-green-500",
  },
  [TransactionStatus.FAILED]: {
    label: "Failed",
    value: "failed",
    bgColor: "bg-red-500",
  },
};

export const TRANSACTION_TYPE: Record<TransactionType, TransactionTypeValue> = {
  [TransactionType.DEPOSIT]: {
    label: "Deposit",
    sign: "+",
    icon: deposit_icon,
    textColor: "text-green-500",
  },
  [TransactionType.WITHDRAW]: {
    label: "Withdraw",
    sign: "-",
    icon: withdraw_icon,
    textColor: "text-red-500",
  },
};
