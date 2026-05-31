import { apiClient } from "../../lib/api-client";
import type { AuthenticationModel, LoginModel } from "./auth.type";

export const login = (data: LoginModel) => {
  return apiClient.post<AuthenticationModel>(`/Authentication`, data);
};

export const getUserInfo = () => {
  return apiClient.get<string>(`/Authentication`);
};
