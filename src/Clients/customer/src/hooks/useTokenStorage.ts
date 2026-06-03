/*
 *For purpose fast demo,
 *I will use localStorage to store the token,
 *but in production, you should save in memory and refresh it when page reloads,
 *or use cookie with httpOnly flag.
 */
export const useTokenStorage = () => {
  const setToken = (token: string) => {
    localStorage.setItem("AspNetCore.Identity.Token", token);
  };

  const getToken = (): string | null => {
    return localStorage.getItem("AspNetCore.Identity.Token");
  };

  const clearToken = () => {
    localStorage.removeItem("AspNetCore.Identity.Token");
  };

  return {
    setToken,
    getToken,
    clearToken,
  };
};
