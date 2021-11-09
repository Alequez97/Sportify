export default async function ({ store }) {
    await store.dispatch('events/fetchCategories')
    await store.dispatch('events/fetchCountries')
    // $axios.onRequest((config) => {
    //   if (store.state.auth.token) {
    //     config.headers.common.Authorization = `Bearer ${store.state.auth.token}`
    //   }
    // })
  }
  