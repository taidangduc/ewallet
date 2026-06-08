import { apiClient } from "../../lib/api-client";
import type { LoginResponse, LoginRequest, User } from "./auth.type";

export const login = (request: LoginRequest) => {
  return apiClient.post<LoginResponse>(`/Authentication`, request);
};

export const getUserInfo = () => {
  return apiClient.get<User>(`/Authentication`);
};
