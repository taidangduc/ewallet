import { apiClient } from "../../lib/api-client";
import type { AuthenticationModel, LoginModel, UserDTO } from "./auth.type";

export const login = (data: LoginModel) => {
  return apiClient.post<AuthenticationModel>(`/Authentication`, data);
};

export const getUserInfo = () => {
  return apiClient.get<UserDTO>(`/Authentication`);
};
