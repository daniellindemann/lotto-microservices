const { env } = require('process');

let target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:5004';

// check if tye and rebind
const webServiceCheckString = 'SERVICE__WEB';
const webServiceEnvVariables = Object.keys(env).filter(k => k.startsWith(webServiceCheckString));
if(env.APP_INSTANCE && webServiceEnvVariables.length > 0) {
  console.log('is running via tye');
  target = env[webServiceCheckString + '__HTTPS__PROTOCOL'] +
    '://' +
    env[webServiceCheckString + '__HTTPS__HOST'] +
    ':' +
    env[webServiceCheckString + '__HTTPS__PORT'];
}
console.log('angular proxy target url: ' + target);

const PROXY_CONFIG = [
  {
    context: [
      "/api",
    ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
