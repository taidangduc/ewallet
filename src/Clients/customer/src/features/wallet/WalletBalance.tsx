import { formatCurrency } from "../../lib/currency";
import { useGetWallet } from "./useGetWallet";

export function WalletBalance() {
  const { wallet } = useGetWallet();

  return (
    <>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-medium">Balance</h1>
        <div className="text-3xl font-bold">
          {formatCurrency(wallet?.balance ?? 0)}
        </div>
      </div>
    </>
  );
}
