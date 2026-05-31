const formatter = new Intl.NumberFormat("en-US", {
  style: "currency",
  currency: "USD",
});
/*
 * @example
 * formatCurrency(1000) // "$10.00"
 * formatCurrency(6767.6767) // "$67.68"
 */

export function formatCurrency(value: number) {
  return formatter.format(value);
}
