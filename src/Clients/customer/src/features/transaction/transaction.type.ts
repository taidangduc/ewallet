export type TransactionModel = {
  walletId: string;
  amount: number;
  type: TransactionType;
  cardNumber: string;
};

export type TransactionDTO = {
  id: string;
  walletId: string;
  amount: number;
  type: TransactionType;
  description?: string;
  date: string;
};

export enum TransactionType {
  DEPOSIT = "DEPOSIT",
  WITHDRAW = "WITHDRAW",
  ERROR = "ERROR",
}
