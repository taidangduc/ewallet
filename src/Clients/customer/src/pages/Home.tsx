import { HeaderLayout } from "../components/layout/Header";
import { TransactionCard } from "../features/transaction/TransactionCard";
import { TransactionHeader } from "../features/transaction/TransactionHeader";
import { useGetTransaction } from "../features/transaction/useGetTransaction";
import { WalletBalance } from "../features/wallet/WalletBalance";
import { WalletCard } from "../features/wallet/WalletCard";
import { cardTest } from "../types/card";

export function HomePage() {
  const { transactions } = useGetTransaction();

  return (
    <>
      <HeaderLayout />
      <div className="max-w-6xl m-auto">
        <div className="grid grid-cols-1 lg:grid-cols-12 gap-4 p-4">
          <div className="lg:col-span-4">
            <h1 className="text-2xl font-medium pb-3">Wallet</h1>
            {cardTest.map((card, index) => (
              <WalletCard
                key={index}
                brand={card.brand}
                last4={card.last4}
                expDate={card.expDate}
              />
            ))}
          </div>
          <div className="lg:col-span-8">
            <WalletBalance />
            <TransactionHeader />
            <div className="pt-2">
              <div className="flex items-center justify-between border-b border-gray-300 my-4">
                <h2 className="text-2xl font-medium">Transactions</h2>
                <a href="/" className="tracking-wide text-blue-500 underline">
                  See all
                </a>
              </div>
              <div>
                {transactions.length > 0 ? (
                  transactions.map((transaction) => (
                    <TransactionCard
                      key={transaction.id}
                      type={transaction.type}
                      status={transaction.status}
                      amount={transaction.amount}
                      date={transaction.createdDateTime}
                    />
                  ))
                ) : (
                  <p className="text-sm text-center">Not found data.</p>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
