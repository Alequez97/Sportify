import https from 'https';

export default function ({ $axios, store }) {
  $axios.onRequest((config) => {
    if (store.state.auth.token) {
      config.headers.common.Authorization = `Bearer ${store.state.auth.token}`
    }
  })
  $axios.defaults.httpsAgent = new https.Agent({ rejectUnauthorized: false })
}
