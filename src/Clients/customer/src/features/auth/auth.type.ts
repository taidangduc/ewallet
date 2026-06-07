export type LoginResponse = {
  accessToken: string;
};

export type LoginRequest = {
  username: string;
  password: string;
};

export type User = {
  id: string;
  username: string;
  roles: string[];
};
