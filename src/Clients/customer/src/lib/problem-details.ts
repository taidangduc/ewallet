import axios from "axios";
import type { ValidationProblemDetails } from "../types/common";

export function getErrorMessage(error: unknown): string {
  if (axios.isAxiosError<ValidationProblemDetails>(error)) {
    const problem = error.response?.data;

    const firstValidationError =
      problem?.errors && Object.values(problem.errors)[0]?.[0];

    return (
      firstValidationError ??
      problem?.detail ??
      problem?.title ??
      "Unexpected error"
    );
  }

  return "Unexpected error";
}
