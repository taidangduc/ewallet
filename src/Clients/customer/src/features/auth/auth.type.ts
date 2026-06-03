export type AuthenticationModel = {
  accessToken: string;
};

export type LoginModel = {
  username: string;
  password: string;
};

export type UserDTO = {
  id: string;
  username: string;
  roles: string[];
};
