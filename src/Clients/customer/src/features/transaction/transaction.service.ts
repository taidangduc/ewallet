import type { TransactionDTO, TransactionModel } from "./transaction.type";
import { apiClient } from "../../lib/api-client";

export const createTransaction = (data: TransactionModel) => {
  return apiClient.post<TransactionDTO>(`/Transaction`, data);
};

export const getTransactions = () => {
  return apiClient.get<TransactionDTO[]>(`/Transaction`);
};
