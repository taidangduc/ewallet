import {
  TRANSACTION_STATUS,
  TRANSACTION_TYPE,
  TransactionStatus,
  TransactionType,
} from "./transaction.type";
import { formatCurrency } from "../../lib/currency";

type TransactionCardProps = {
  type: TransactionType;
  status: TransactionStatus;
  amount: number;
  date: string;
};
export function TransactionCard({
  type,
  status,
  amount,
  date,
}: TransactionCardProps) {
  const { icon, label, sign } = TRANSACTION_TYPE[type];

  const { value, bgColor } = TRANSACTION_STATUS[status];

  const formatAmount = `${sign}${formatCurrency(amount)}`;

  return (
    <div className="p-3 my-4 border-b border-gray-300">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <div className="rounded-full bg-gray-100">
            <img src={icon} alt={label} />
          </div>
          <div>
            <h3 className="text-lg font-medium">{label}</h3>
            <div className="flex items-center gap-2">
              <span className={`min-w-[55px] text-[12px] text-white font-semibold py-[1px] px-[2px] text-center ${bgColor}`}>{value}</span>
              <p className="text-sm text-gray-500">{date}</p>
            </div>
          </div>
        </div>
        <div className="flex flex-col items-end">
          <p className={`text-md font-bold`}>{formatAmount}</p>
        </div>
      </div>
    </div>
  );
}
