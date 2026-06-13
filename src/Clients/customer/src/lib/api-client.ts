import axios from "axios";
import { useTokenStorage } from "../hooks/useTokenStorage";
import { toast } from "sonner";

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  headers: {
    "Content-Type": "application/json",
  },
});

apiClient.interceptors.request.use((config) => {
  const { getToken } = useTokenStorage();
  const token = getToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      const { clearToken } = useTokenStorage();
      clearToken();
      toast.error("Session expired. Please log in again.");
    }
    return Promise.reject(error);
  },
);
