import { Dispatch, SetStateAction, useEffect, useState } from "react";

type DataType<T> = T[];
type ErrorType = TypeError | null;

interface Params<T> {
  data: DataType<T>;
  setData: Dispatch<SetStateAction<DataType<T>>>
  loading: boolean;
  error: ErrorType;
}

interface ResponseFetch<T> {
  success: boolean;
  message: string;
  data?: DataType<T>;
}

export const useFetch = <T>(url: string): Params<T> => {
  const [data, setData] = useState<DataType<T>>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<ErrorType>(null);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);

      const options: RequestInit = {
        method: "GET",
        headers: { "Content-Type": "application/json" },
        credentials: "include"
      };

      try {
        const response = await fetch(url, options);

        if (!response.ok) {
          throw new Error('Error al conectar con el servidor.');
        }

        const data: ResponseFetch<T> = await response.json();

        if (!data.success || !data.data) {
          throw new Error("ERROR: " + data.message);
        }

        setData(data.data);
        setError(null);
      } catch (err) {
        setError(err as Error);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, [url]);

  return { data, setData, loading, error };
};
