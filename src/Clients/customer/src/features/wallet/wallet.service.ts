import { apiClient } from "../../lib/api-client";
import type { WalletDTO } from "./wallet.type";

export const getWallet = () => {
  return apiClient.get<WalletDTO>(`/Wallet`);
};
