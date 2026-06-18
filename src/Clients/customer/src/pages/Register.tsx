import wallet_logo from "@assets/wallet-blue.png";
import { RegisterForm } from "../features/auth/RegisterForm";
import { Link } from "react-router-dom";

export function RegisterPage() {
  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="w-full max-w-md bg-white p-5 shadow-lg">
        <div className="flex justify-center mb-5">
          <img src={wallet_logo} alt="Wallet Logo" className="h-15" />
        </div>
        <h2 className="text-center text-xl font-bold mb-3">
          Sign up for a new account
        </h2>
        <h2 className="text-center text-md mb-3">
          Enter your credentials to create your account.
        </h2>
        <RegisterForm />
        <div className="text-center mt-3 text-sm">
          Already have an account? <Link to="/login" className="text-blue-500">Sign in</Link>
        </div>
      </div>
    </div>
  );
}
