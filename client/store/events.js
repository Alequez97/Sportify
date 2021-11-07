export const state = () => ({
    categories: [],
    countries: [],
    cities: []
});

export const mutations = {
    SET_CATEGORIES() {

    },
    SET_COUNTRIES() {

    },
    SET_CITIES() {

    }
}

export const actions = {
    async fetchCategories() {
        try {
            const resp = await this.$axios.get('/api/categories');
            return resp.data;
        } catch (err) {
            console.log(err);
        }
    },
    async fetchCountries() {
        try {
            const resp = await this.$axios.get('/api/countries');
            return resp.data;
        } catch (err) {
            console.log(err);
        }
    },
    async fetchCities({ commit }, countryId) {
        try {
            debugger;
            const resp = await this.$axios.get('/api/cities/' + countryId);
            return resp.data;
        } catch (err) {
            console.log(err);
        }
    }

}

export const getters = {
    getCategories: state => state.categories,
    getCountries: state => state.countries,
    getCities: state => state.cities
}
