import { Link } from "react-router-dom";
import wallet_logo from "@assets/wallet-white.png";

export function HeaderLayout() {
  return (
    <header className="bg-blue-500 shadow py-4 mb-2">
      <div className="flex items-center justify-between max-w-6xl mx-auto px-4">
        <div className="flex items-center space-x-2">
          <img src={wallet_logo} alt="Wallet Logo" className="h-10" />
          <h1 className="text-2xl font-medium text-white">EWallet</h1>
        </div>
        <div>
          <Link
            to="/login"
            className="bg-white text-black font-medium px-4 py-2 "
          >
            Sign in
          </Link>
        </div>
      </div>
    </header>
  );
}
