import { apiClient } from "../../lib/api-client";
import type { LoginResponse, LoginRequest, User, RegisterRequest } from "./auth.type";

export const login = (request: LoginRequest) => {
  return apiClient.post<LoginResponse>(`/Authentication`, request);
};

export const getUserInfo = () => {
  return apiClient.get<User>(`/Authentication`);
};

export const register = (request: RegisterRequest) => {
  return apiClient.post(`/Authentication/register`, request);
};
