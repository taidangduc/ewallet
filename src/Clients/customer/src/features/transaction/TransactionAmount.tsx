type Props = {
  amount: number;
  currency: string;
  onChange: (value: number) => void;
};

export function TransactionAmount({ amount, currency, onChange }: Props) {
  return (
    <>
      <div className="flex flex-col gap-1 my-2">
        <label htmlFor="input-amount">
          <h1 className="text-lg">Amount</h1>
        </label>
        <div className="flex border focus-within:ring-1 focus-within:ring-gray-500">
          <input
            type="text"
            value={amount}
            id="input-amount"
            className="flex-1 px-3 py-2 outline-none border-0"
            onChange={(e) => onChange(Number(e.target.value))}
          />
          <span className="px-3 py-2 border-l bg-gray-50 font-medium">
            {currency}
          </span>
        </div>
      </div>
    </>
  );
}
