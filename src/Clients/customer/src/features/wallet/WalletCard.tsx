import visa_icon from "@assets/visa-icon.png";
import mastercard_icon from "@assets/mastercard-icon.png";
import { maskCardNumber } from "../../lib/mask-card";
import { CardBrand } from "../../types/card";

type Props = {
  brand: CardBrand;
  last4: string;
  expDate: string;
};

export function WalletCard(data: Props) {
  /*
   * Function to mask the card number,
   * showing only the last 4 digits and replacing the rest with "*".
   * WARN: real world implementation should comply with PCI-DSS standard
   */
  const getCardIcon = () => {
    if (data.brand === CardBrand.Visa) {
      return {
        icon: visa_icon,
      };
    } else if (data.brand === CardBrand.MasterCard) {
      return {
        icon: mastercard_icon,
      };
    } else {
      return {
        icon: undefined,
      };
    }
  };

  return (
    <div className="outline outline-1 outline-gray-300 p-6 flex flex-col items-start my-4">
      <img
        src={getCardIcon().icon}
        alt={`${data.brand} Icon`}
        className="h-6 w-auto mb-2"
      />
      <div className="text-lg">{maskCardNumber(data.last4)}</div>
      <div className="text-sm">Exp Date: {data.expDate}</div>
    </div>
  );
}
