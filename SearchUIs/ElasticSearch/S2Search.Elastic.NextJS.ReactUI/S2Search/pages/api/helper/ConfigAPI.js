import { AxiosGet } from "./AxiosAPICall";

const ConfigAPI = async () => {
  let URL = `api/configuration`;
  return await AxiosGet(URL, true);
};

export default ConfigAPI;
