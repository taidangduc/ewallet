import { HeaderLayout } from "../components/layout/Header";
import {
  TransactionType,
  type TransactionDTO,
} from "../features/transaction/transaction.type";
import { TransactionCard } from "../features/transaction/TransactionCard";
import { TransactionHeader } from "../features/transaction/TransactionHeader";
import { WalletBalance } from "../features/wallet/WalletBalance";
import { WalletCard } from "../features/wallet/WalletCard";
import { cardTest } from "../types/mock";

export function HomePage() {
  const transactionTest: TransactionDTO[] = [
    {
      id: "1",
      walletId: "wallet1",
      amount: 100,
      type: TransactionType.DEPOSIT,
      date: "2026/30/05 1:00 PM",
    },
    {
      id: "2",
      walletId: "wallet1",
      amount: 100,
      type: TransactionType.WITHDRAW,
      date: "2026/30/05 1:05 PM",
    },
    {
      id: "3",
      walletId: "wallet1",
      amount: 100,
      type: TransactionType.ERROR,
      date: "2026/30/05 1:09 PM",
    },
  ];

  return (
    <>
      <HeaderLayout />
      <div className="max-w-6xl m-auto">
        <div className="grid grid-cols-1 lg:grid-cols-12 gap-4 p-4  ">
          <div className="lg:col-span-4">
            <h1 className="text-2xl font-medium pb-3">Wallet</h1>
            {cardTest.map((card, index) => (
              <WalletCard
                key={index}
                brand={card.brand}
                last4={card.last4}
                expDate={card.exp_date}
              />
            ))}
          </div>
          <div className="lg:col-span-8">
            <WalletBalance />
            <TransactionHeader />
            <div className="pt-2">
              <div className="flex items-center justify-between border-b border-gray-300 my-4">
                <h2 className="text-2xl font-medium">Transactions</h2>
                <a href="" className="text-blue-500 underline">
                  See all
                </a>
              </div>
              <div>
                {transactionTest.map((transaction) => (
                  <TransactionCard
                    key={transaction.id}
                    type={transaction.type}
                    amount={transaction.amount}
                    date={transaction.date}
                  />
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
