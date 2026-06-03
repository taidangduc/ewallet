export function maskCardNumber(last4: string): string {
  return (
    /*
     * Mask card number except for the last 4 digits via tokenization service,
     * You should allow PCI-DSS standard
     * But in this example, we will just mask the card number for demonstration purposes
     * @example:
     * 8668 => **** **** **** 8668
     */
    "**** **** **** " + last4
  );
}
