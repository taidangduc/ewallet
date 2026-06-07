import { LoginForm } from "../features/auth/LoginForm";
import wallet_logo from "@assets/wallet-blue.png";

export function LoginPage() {
  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="w-full max-w-md bg-white p-5 shadow-lg">
        <div className="flex justify-center mb-5">
          <img src={wallet_logo} alt="Wallet Logo" className="h-15" />
        </div>
        <h2 className="text-center text-xl font-bold mb-3">
          Sign in to your account
        </h2>
        <h2 className="text-center text-md mb-3">
          Enter your credentials to access your account.
        </h2>
        <LoginForm/>
      </div>
    </div>
  );
}
