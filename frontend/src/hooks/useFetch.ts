import { Dispatch, SetStateAction, useEffect, useState } from "react";

type DataType<T> = T | null;
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
  data?: T;
}

export const useFetch = <T>(fetchFunction: () => Promise<Response>): Params<T> => {
  const [data, setData] = useState<DataType<T>>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<ErrorType>(null);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        const response = await fetchFunction();

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
  }, [fetchFunction]);

  return { data, setData, loading, error };
};
