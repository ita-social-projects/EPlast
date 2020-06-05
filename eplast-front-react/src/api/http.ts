import axios from 'axios';
import BASE_URL from '../config';

interface HttpResponse {
  headers: any;
  data: any;
}

const get = async (url: string, data?: any, options: any = {}): Promise<HttpResponse> => {
  const response = await axios.get(BASE_URL + url, {
    ...options,
    params: data,
  });
  return response;
};

export default { get };
