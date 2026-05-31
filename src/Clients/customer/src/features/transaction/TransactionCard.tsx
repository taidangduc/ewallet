import deposit_icon from "@assets/arrow-up-right-bg-black.png";
import withdraw_icon from "@assets/arrow-down-left-bg-black.png";
import error_icon from "@assets/error-icon.png";
import { TransactionType } from "./transaction.type";

type TransactionCardProps = {
  type: TransactionType;
  amount: number;
  date: string;
};
export function TransactionCard({ type, amount, date }: TransactionCardProps) {
  const { icon, label, value, color } = (() => {
    switch (type) {
      case TransactionType.DEPOSIT:
        return {
          icon: deposit_icon,
          label: "Deposit",
          value: `+ $${amount.toFixed(2)}`,
          color: "text-green-500",
        };

      case TransactionType.WITHDRAW:
        return {
          icon: withdraw_icon,
          label: "Withdraw",
          value: `- $${amount.toFixed(2)}`,
          color: "text-red-500",
        };

      default:
        return {
          icon: error_icon,
          label: "Error",
          value: "NaN",
          color: "text-black",
        };
    }
  })();

  return (
    <div className="p-3 my-4 border-b border-gray-300">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <div className="rounded-full bg-gray-100">
            <img src={icon} alt={label} />
          </div>
          <div>
            <h3 className="text-lg font-medium">{label}</h3>
            <p className="text-sm text-gray-500">{date}</p>
          </div>
        </div>
        <div>
          <p className={`text-lg font-bold ${color}`}>{value}</p>
        </div>
      </div>
    </div>
  );
}
