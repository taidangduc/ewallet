import type { Transaction, TransactionRequest } from "./transaction.type";
import { apiClient } from "../../lib/api-client";

export const createTransaction = (request: TransactionRequest) => {
  return apiClient.post<Transaction>(`/Transaction`, request);
};

export const getTransactions = () => {
  return apiClient.get<Transaction[]>(`/Transaction`);
};
