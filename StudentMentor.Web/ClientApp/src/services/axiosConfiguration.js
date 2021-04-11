import axios from "axios";
import { history } from "../utils/BrowserHistoryWrapper";

const newToken = (token) =>
  axios.get(`api/Account/RefreshToken?token=${token}`);

const configureAxios = () => {
  axios.defaults.headers.post["Content-Type"] = "application/json";

  axios.interceptors.request.use((config) => {
    const token = `Bearer ${localStorage.getItem("token")}`;
    config.headers["Authorization"] = token;

    return config;
  });

  const handleRedirectToLogin = () => {
    localStorage.removeItem("token");
    history.push("/login");
  };

  axios.interceptors.response.use(
    (response) => response,
    (error) => {
      let originalRequest = error.config;
      const token = localStorage.getItem("token");
      if (error.response && error.response.status === 401 && !token) {
        history.push("/login");

        return Promise.reject(error);
      } else if (
        error.response &&
        error.response.status === 401 &&
        originalRequest &&
        !originalRequest._isRetryRequest &&
        token
      ) {
        originalRequest._isRetryRequest = true;

        return newToken(token).then(({ data }) => {
          if (!data) {
            handleRedirectToLogin();

            return;
          }
          localStorage.setItem("token", data);

          return axios(originalRequest);
        });
      } else if (
        error.response &&
        error.response.status === 401 &&
        originalRequest &&
        originalRequest._isRetryRequest
      ) {
        handleRedirectToLogin();
      }

      return Promise.reject(error);
    }
  );
};

export default configureAxios;
