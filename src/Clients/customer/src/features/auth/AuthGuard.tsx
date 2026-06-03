import { Navigate } from "react-router-dom";
import { useSession } from "./useSession";
import { Spinner } from "../../components/ui/Spinner";

type Props = {
  children: React.ReactNode;
};

export function AuthGuard({ children }: Props) {
  const { loading, isAuthenticated } = useSession();

  /*
   * NOTE: Check loading state before isAuthenticated
   * If you don't have to check loading state before checking isAuthenticated,
   * It will cause app will redirect to login page before checking token and user info
   * Because of isAuthenticated is false.
   */

  if (loading) {
    return (
      <div className="flex flex-col min-h-screen justify-center items-center">
        <Spinner />
      </div>
    );
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return children;
}
