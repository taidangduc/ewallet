import { apiClient } from "../../lib/api-client";
import type { LoginResponse, LoginRequest, User } from "./auth.type";

export const login = (data: LoginRequest) => {
  return apiClient.post<LoginResponse>(`/Authentication`, data);
};

export const getUserInfo = () => {
  return apiClient.get<User>(`/Authentication`);
};
