import { maskCardNumber } from "../../lib/mask-card";
import visa_icon from "@assets/visa-icon.png";
import mastercard_icon from "@assets/mastercard-icon.png";
import { CardBrand, type Card } from "../../types/card";

type CardSelectProps = {
  data: Card[];
  cardId: string;
  onChange: (cardId: string) => void;
};

export function TransactionCardSelect({
  data,
  cardId,
  onChange,
}: CardSelectProps) {
  const getCardIcon = (brand: CardBrand) => {
    if (brand === CardBrand.Visa) {
      return visa_icon;
    } else if (brand === CardBrand.MasterCard) {
      return mastercard_icon;
    } else {
      return undefined;
    }
  };

  return (
    <div className="flex flex-col gap-2">
      <label>Card Credit</label>
      {data.map((card) => (
        <label
          key={card.id}
          className="flex items-center justify-between border p-2 cursor-pointer"
        >
          <div className="flex items-center gap-2">
            <input
              type="radio"
              name="card"
              value={card.id}
              checked={cardId === card.id}
              onChange={() => onChange(card.id)}
            />
            <div className="flex flex-col items-start gap-2 py-1">
              <img
                src={getCardIcon(card.brand)}
                alt={`${card.brand} Icon`}
                className="h-6 w-auto"
              />
              <div className="text-lg">{maskCardNumber(card.last4)}</div>
            </div>
          </div>
        </label>
      ))}
    </div>
  );
}
