import { useAuth } from "./auth.context";

/*
 * Why useSession?
 * "read only" from context
 * avoid unnecessary re-renders when only user or isAuthenticated changes
 */
export const useSession = () => {
  const { user, loading } = useAuth();

  return { user, isAuthenticated: !!user, loading };
};
