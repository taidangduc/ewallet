type DialogProps = {
  open: boolean;
  onClose: () => void;
  children: React.ReactNode;
};
export function Dialog({ open, onClose, children }: DialogProps) {
  if (!open) return null;

  return (
    /*
     * If you want to close the dialog when clicking outside of it,
     * you can add an onClick handler with onClose param
     */
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black/50"
      onClick={onClose}
    >
      <div
        className="w-full max-w-md bg-white p-5 shadow-lg"
        onClick={(e) => e.stopPropagation()}
      >
        {children}
      </div>
    </div>
  );
}
