import { useCallback, useEffect, useState } from "react";

export const useFetch = <T>(initialValue: T, url: string) => {
  const [data, setData] = useState<T>(initialValue);

  const fetchData = useCallback(async () => {
    try {
      const response = await fetch(url, {
        credentials: "include"
      });

      if (!response.ok) {
        console.log("Error en el fetching de datos");
        return;
      }

      const dataJson: { success: boolean, message?: string, data?: T } = await response.json();

      if (dataJson?.data) {
        setData(dataJson.data);
      }
    } catch (err) {
      console.error(err);
    }
  }, [url]);
  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return { data, setData, fetchData };
};
