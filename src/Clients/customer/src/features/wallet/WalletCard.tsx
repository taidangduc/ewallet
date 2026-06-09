import visa_icon from "@assets/visa-icon.png";
import mastercard_icon from "@assets/mastercard-icon.png";
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
    <div className="relative h-52 my-4 overflow-hidden text-white p-6 bg-gradient-to-br from-gray-500 via-slate-400 to-gray-900">
      <div className="absolute -bottom-12 -left-12 h-40 w-40 rounded-full bg-white/10" />
      <div className="absolute -right-16 -top-16 h-56 w-56 rounded-full bg-white/10" />

      <div className="relative flex h-full flex-col">
        <div className="flex justify-end">
          <img src={getCardIcon().icon} alt={data.brand} className="h-8" />
        </div>

        <div className="mt-auto">
          <div className="mb-4 text-2xl tracking-widest">
            **** **** **** {data.last4}
          </div>

          <div className="flex justify-between text-sm text-white/90">
            <span>{data.brand}</span>
            <span>Exp {data.expDate}</span>
          </div>
        </div>
      </div>
    </div>
  );
}
