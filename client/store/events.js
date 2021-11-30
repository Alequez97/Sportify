export const state = () => ({
    events: [],
    categories: [],
    countries: [],
    cities: []
});

export const mutations = {
    SET_EVENTS(state, value) {
        state.events = value;
    },
    SET_CATEGORIES(state, value) {
        state.categories = value;
    },
    SET_COUNTRIES(state, value) {
        state.countries = value;
    },
    SET_CITIES(state, value) {
        state.cities = value;
    },
    SET_ISGOING_TRUE(state, value) {
        const rec = state.events.find(e => e.id === value);
        rec.isGoing = true;
        rec.contributors.push({id: this.$auth.$state.user.id, username: this.$auth.$state.user.username});
    },
    SET_ISGOING_FALSE(state, value) {
        const rec = state.events.find(e => e.id === value);
        const userId = this.$auth.$state.user.id;
        rec.isGoing = false;
        rec.contributors = rec.contributors.filter(u => u.id !== userId);
    }
}

export const actions = {
    async fetchEvents({commit}){
        await this.$axios.get('/api/events').then((response) => {
            commit('SET_EVENTS', response.data)
          }).catch((error) => {
            console.log(error);
          });
    },
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
    },
    async createEvent({dispatch, commit}, eventData) {
        await this.$axios.post("/api/event/create", eventData);
        await dispatch('fetchEvents');
        this.$router.push('/events');
    },
    async join({dispatch, commit}, id) {
        try {
            await this.$axios.post('/api/events/join', { eventId: id });
            commit('SET_ISGOING_TRUE', id);
        } catch (err) {
            console.log(err);
        }
    },
    async disjoin({dispatch, commit}, id) {
        try {
            await this.$axios.post('api/events/disjoin', {eventId: id});
            commit('SET_ISGOING_FALSE', id);
        } catch (err) {
            console.log(err);
        }
    },
    async applyFilters({commit}, filterData) {
        try {
            debugger;
            const resp = await this.$axios.get('api/events', { params: {categoryId: filterData.categoryId, countryId: filterData.countryId, cityId: filterData.cityId} });
            debugger;
            commit("SET_EVENTS", resp.data);
        } catch (err) {
            console.log(err);
        }
    }
}

export const getters = {
    getEvents: state => state.events,
    getCategories: state => state.categories,
    getCountries: state => state.countries,
    getCities: state => state.cities
}
