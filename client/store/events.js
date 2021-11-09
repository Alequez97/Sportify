export const state = () => ({
    categories: [],
    countries: [],
    cities: []
});

export const mutations = {
    SET_CATEGORIES(state, value) {
        state.categories = value;
    },
    SET_COUNTRIES(state, value) {
        state.countries = value;
    },
    SET_CITIES(state, value) {
        state.cities = value;
    }
}

export const actions = {
    async fetchCategories({commit}) {
        try {
            const resp = await this.$axios.get('/api/event-categories');
            commit('SET_CATEGORIES', resp.data)
        } catch (err) {
            console.log(err);
        }
    },
    async fetchCountries({commit}) {
        try {
            const resp = await this.$axios.get('/api/countries');
            commit('SET_COUNTRIES', resp.data);
        } catch (err) {
            console.log(err);
        }
    },
    async fetchCities({commit}, countryId) {
        try {
            const resp = await this.$axios.get('/api/cities/' + countryId);
            commit('SET_CITIES', resp.data);
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
