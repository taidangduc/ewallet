import type React from "react";
import { useLogin } from "./useLogin";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import type { LoginRequest } from "./auth.type";

export function LoginForm() {
  /*
   * You can move logic in LoginPage if this component is reused in other places,
   * or page is orchestrate logic and components receive props,
   * @example:
   * <LoginForm onSubmit={handleSubmit} loading={loading} error={error} />
   * <LoginForm onSuccess={() => navigate("/")} />
   * but for now, I think it's better to keep it here since it's only used in LoginPage
   */
  const { signIn, loading } = useLogin();
  const navigate = useNavigate();

  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    /*
     * You can use "React Hook Form" + "Zod" to handle form state and validation
     * But for now, I use FormData to keep it simple
     */

    const formData = new FormData(e.currentTarget);

    const request: LoginRequest = {
      username: (formData.get("username") as string) ?? "",
      password: (formData.get("password") as string) ?? "",
    };

    if (!request.username || !request.password) {
      setError("Username and password are required");
      return;
    }

    try {
      await signIn(request);
      navigate("/");
    } catch (error) {
      setError("Invalid username or password");
    }
  };

  return (
    <div>
      {error && <div className="mb-4 text-red-500 text-center">{error}</div>}
      <form className="space-y-4" onSubmit={handleSubmit}>
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
