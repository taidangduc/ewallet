import type React from "react";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import type { RegisterRequest } from "./auth.type";
import { useRegister } from "./useRegister";
import { TextField } from "../../components/ui/TextField";

type FieldError = {
  username?: string;
  email?: string;
  password?: string;
};

/*
 * In this component, I just validate empty fields
 * You can use library react-hook-form, zod or any library you want to validate form data
 */

export function RegisterForm() {
  const { signUp, loading, error } = useRegister();
  const navigate = useNavigate();

  const [fieldError, setFieldError] = useState<FieldError>({});

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const formData = new FormData(e.currentTarget);

    const request: RegisterRequest = {
      username: (formData.get("username") as string) ?? "",
      email: (formData.get("email") as string) ?? "",
      password: (formData.get("password") as string) ?? "",
    };

    const errors: FieldError = {};

    if (!request.username.trim()) {
      errors.username = "This field is required";
    }

    if (!request.email.trim()) {
      errors.email = "This field is required";
    }

    if (!request.password.trim()) {
      errors.password = "This field is required";
    }

    setFieldError(errors);

    if (Object.keys(errors).length > 0) {
      return;
    }

    try {
      await signUp(request);
      navigate("/login");
    } catch {}
  };

  return (
    <div>
      {error ? (
        <div className="mb-4 text-red-500 text-center">{error}</div>
      ) : null}
      <form className="space-y-4" onSubmit={handleSubmit}>
        <TextField
          label="Username"
          name="username"
          placeholder="Enter your username"
          errorMessage={fieldError.username}
        />
        <TextField
          label="Email"
          name="email"
          type="email"
          placeholder="Enter your email"
          errorMessage={fieldError.email}
        />
        <TextField
          label="Password"
          name="password"
          type="password"
          placeholder="Enter your password"
          errorMessage={fieldError.password}
        />
        <button
          type="submit"
          disabled={loading}
          className={`w-full text-white py-2 px-4 mt-2 ${loading ? "cursor-not-allowed bg-blue-100" : "cursor-pointer bg-blue-500"}`}
        >
          Sign up
        </button>
      </form>
    </div>
  );
}
