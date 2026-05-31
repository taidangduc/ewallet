import { formatCurrency } from "../../lib/currency";

export function WalletBalance() {
  return (
    <>
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-medium">Balance</h1>
        <div className="text-3xl font-bold">{formatCurrency(1223)}</div>
      </div>
    </>
  );
}
