<template>
  <nav>
    <v-app-bar color="teal" dense dark app>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer" />
      <v-toolbar-title class="pl-0" app>
        <NuxtLink to="/">
          Sportify
        </NuxtLink>
      </v-toolbar-title>

      <v-spacer />

      <div v-if="isAuthenticated">
        <v-btn text @click="logout()">
          <span>Sign Out</span>
          <v-icon right>
            mdi-logout
          </v-icon>
        </v-btn>
      </div>
      <div v-else>
        <v-btn router to="/login" text>
          <span>Sign In</span>
          <v-icon right>
            mdi-login
          </v-icon>
        </v-btn>

        <v-btn router to="/register" text>
          <span>Sign Up</span>
          <v-icon right>
            mdi-account-plus-outline
          </v-icon>
        </v-btn>
      </div>
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" class="teal lighten-5" disable-resize-watcher absolute temporary app>
      <v-container>
        <div v-if="isAuthenticated">
          <v-row class="mt-0">
            <v-col col="12" justify="center" align="center">
              <v-avatar size="100">
                <img src="/kot_fleks.jpg">
              </v-avatar>
              <p color="black" class="mt-2 mb-0">
                {{ userInfo.username }}
              </p>
            </v-col>
          </v-row>

          <PopupEventForm />
        </div>
        <div v-else>
          <v-row justify="center" class="mb-0 mt-3">
            <v-btn color="teal" dark router to="/login">
              Create Event
            </v-btn>
          </v-row>
        </div>
      </v-container>
      <v-list nav dense>
        <v-list-item-group
          active-class="teal--text"
        >
          <v-list-item v-for="link in links" :key="link.text" router :to="link.route">
            <v-list-item-icon>
              <v-icon>{{ link.icon }}</v-icon>
            </v-list-item-icon>
            <v-list-item-title>{{ link.text }}</v-list-item-title>
          </v-list-item>
        </v-list-item-group>
      </v-list>
    </v-navigation-drawer>
  </nav>
</template>

<script>
import PopupEventForm from "../events/PopupEventForm"

export default {
  name: "AppBar",

  components: {
    PopupEventForm
  },

  data: () => ({
    drawer: false,
    links: [
      { icon: 'mdi-soccer', text: 'Events', route: '/events' },
      { icon: 'mdi-dumbbell', text: 'Sports Grounds & Gyms', route: '/map' }
    ]
  }),

  computed: {
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    },
    userInfo() {
      return this.$store.getters.getUserInfo;
    }
  },

  methods: {
    async logout() {
        await this.$auth.logout();
    }
  }
}
</script>
