import axios from "~/plugins/axios";

export const state = () => ({
    token: '',
    user: {}
  });

  export const mutations = {
    SET_TOKEN(state, token) {
      state.token = token;
    },
    SET_USER(state, user) {
      state.user = user;
    }
  };

  export const actions = {
    async login({ commit }, data) {
      try {
        const resp = await this.$axios.post('/api/accounts/login', data);

        if (resp.data.token !== '') {
          commit('SET_TOKEN', resp.data.token);

          try {
            const user = await this.$axios.get('/api/accounts/info');
            commit('SET_USER', user.data);
          } catch (err) {
            console.log(err);
          }
        }
      } catch (err){
        console.log(err);
      }
    },
    async register({commit }, user) {
      try {
        await this.$axios.post('/api/accounts/register', user);
      } catch(err) {
        console.log(err);
      }
    }
  };

  export const getters = {
    getToken: state => state.token,
    isLoggedIn: state => !!state.token,
    getUserName: state => state.user.username
  };
