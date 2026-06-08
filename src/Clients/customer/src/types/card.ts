export type Card = {
  id: string;
  brand: CardBrand;
  last4: string;
  expDate: string;
};

export enum CardBrand {
  Visa = "Visa",
  MasterCard = "MasterCard",
}

export const cardTest: Card[] = [
  {
    id: "card_98765abcdef",
    brand: CardBrand.Visa,
    last4: "4242",
    expDate: "12/2026",
  },
  {
    id: "card_12345abcdef",
    brand: CardBrand.MasterCard,
    last4: "0002",
    expDate: "11/2024",
  },
];
