import { apiClient } from "../../lib/api-client";
import type { Wallet } from "./wallet.type";

export const getWallet = () => {
  return apiClient.get<Wallet>(`/Wallet`);
};
