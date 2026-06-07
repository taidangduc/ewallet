export type TransactionRequest = {
  walletId: string;
  amount: number;
  type: TransactionType;
  cardNumber: string;
};

export type Transaction = {
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
