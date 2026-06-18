export type LoginResponse = {
  accessToken: string;
};

export type LoginRequest = {
  username: string;
  password: string;
};

export type RegisterRequest = {
  username: string;
  password: string;
  email: string;
};

export type User = {
  id: string;
  username: string;
  roles: string[];
};
