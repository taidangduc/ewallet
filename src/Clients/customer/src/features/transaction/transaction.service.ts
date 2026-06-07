import type { Transaction, TransactionRequest } from "./transaction.type";
import { apiClient } from "../../lib/api-client";

export const createTransaction = (data: TransactionRequest) => {
  return apiClient.post<Transaction>(`/Transaction`, data);
};

export const getTransactions = () => {
  return apiClient.get<Transaction[]>(`/Transaction`);
};
