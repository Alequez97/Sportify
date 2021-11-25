// import path from 'path'
// import fs from 'fs'

export default {
  // Global page headers: https://go.nuxtjs.dev/config-head
  head: {
    title: 'Sportify',
    htmlAttrs: {
      lang: 'en'
    },
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: '' },
      { name: 'format-detection', content: 'telephone=no' }
    ],
    link: [
      { rel: 'icon', type: 'image/x-icon', href: '/icons/favicon.ico' }
    ]
  },

  //Uncomment this if you want to test on mobile/tablet. As URL use: {yourIPaddress}:8000
  server: {
    // https: {
    //   key: fs.readFileSync(path.resolve('C:/Users/jurij/Desktop/cert/CA', 'localhost.key')),
    //   cert: fs.readFileSync(path.resolve('C:/Users/jurij/Desktop/cert/CA', 'localhost.crt'))
    // },
    // host: '0.0.0.0'
  },

  // Global CSS: https://go.nuxtjs.dev/config-css
  css: [
  ],

  // Plugins to run before rendering page: https://go.nuxtjs.dev/config-plugins
  plugins: [
    '@/plugins/axios',
    '@/plugins/initData.js'
  ],

  // Auto import components: https://go.nuxtjs.dev/config-components
  components: true,

  // Modules for dev and build (recommended): https://go.nuxtjs.dev/config-modules
  buildModules: [
    // https://go.nuxtjs.dev/eslint
    '@nuxtjs/eslint-module',
    '@nuxtjs/vuetify'
  ],

  // Modules: https://go.nuxtjs.dev/config-modules
  modules: [
    // https://go.nuxtjs.dev/axios
    '@nuxtjs/axios',
    '@nuxtjs/auth-next'
  ],

  auth: {
    strategies: {
      local: {
        token: {
          property: "token",
          global: true,
          required: true,
          type: "Bearer"
        },
        user: {
          property: "user",
          autoFetch: true
        },
        endpoints: {
          login: { url: "/api/accounts/login", method: "post" },
          logout: false,
          user: { url: "/api/accounts/info", method: "get" }
        }
      }
    }
  },

  // Axios module configuration: https://go.nuxtjs.dev/config-axios
  // axios: {
  //   baseURL: 'http://192.168.8.166:6000'
  // },

  axios: {
    baseURL: 'https://localhost:44314'
  },

  // Build Configuration: https://go.nuxtjs.dev/config-build
  build: {
  }
}
