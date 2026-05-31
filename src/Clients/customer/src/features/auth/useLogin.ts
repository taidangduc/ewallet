import React from "react";

export function useLogin(){
    const [error, setError] = React.useState<string | null>(null);
    const [isLoading, setIsLoading] = React.useState(false);
    const [user, setUser] = React.useState<string | null>(null);
}