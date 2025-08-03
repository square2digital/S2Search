import { AxiosGet } from "./AxiosAPICall";

const ThemeAPI = async () => {
  let URL = "/api/theme";
  return await AxiosGet(URL, true);
};

export default ThemeAPI;
