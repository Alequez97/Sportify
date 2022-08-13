export default async function ({ store }) {
    await store.dispatch('events/fetchCategories')
    await store.dispatch('events/fetchCountries')
}
