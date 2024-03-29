import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    vus: 500,
    duration: '5m',
  };

export default function () {
  http.get(`http://${__ENV.APP_HOSTNAME}/api/lottonumber/`);  // host.docker.interal is a dns name, that gets created when using docker and k8s
  // http.get('http://host.docker.internal:8081/api/lottonumber')
  sleep(1);
}
