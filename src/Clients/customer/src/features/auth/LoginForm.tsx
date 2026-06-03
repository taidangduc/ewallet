import type React from "react";
import { useLogin } from "./useLogin";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

export function LoginForm() {
  const { signIn, loading } = useLogin();
  const navigate = useNavigate();

  const [error, setError] = useState<string | null>(null);

  const onHandleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const formData = new FormData(e.currentTarget);
    const username = formData.get("username") as string;
    const password = formData.get("password") as string;

    try {
      await signIn({ username, password });
      navigate("/");
    } catch (error) {
      setError("Invalid username or password");
    }
  };

  return (
    <div>
      {error && <div className="mb-4 text-red-500 text-center">{error}</div>}
      <form className="space-y-4" onSubmit={onHandleSubmit}>
        <div>
          <label className="block text-md">Username</label>
          <input
            name="username"
            type="text"
            className="mt-1 block w-full border border-gray-300  p-2"
            placeholder="Enter your username"
          />
        </div>
        <div>
          <label className="block text-md ">Password</label>
          <input
            name="password"
            type="password"
            className="mt-1 block w-full border border-gray-300  p-2"
            placeholder="Enter your password"
          />
        </div>
        <div className="text-sm text-gray-500">
          Tips: For testing, you can use user/User@123
        </div>
        <button
          type="submit"
          disabled={loading}
          className={`w-full text-white py-2 px-4  ${loading ? "cursor-not-allowed bg-blue-100" : "cursor-pointer bg-blue-500"}`}
        >
          Sign in
        </button>
      </form>
    </div>
  );
}
