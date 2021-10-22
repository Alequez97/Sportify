<template>
  <v-card>
    <v-app-bar color="deep-purple" dense fixed dark app>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title class="pl-0" app><NuxtLink to="/">Sportify</NuxtLink></v-toolbar-title>
      <v-toolbar-items>
        <v-btn nuxt to="/events" text>Events</v-btn>
      </v-toolbar-items>

      <v-spacer/>

      <!-- <v-toolbar-items>
        <v-btn nuxt to="/events" text>Events</v-btn>
      </v-toolbar-items> -->
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" absolute temporary app>
      <v-list nav dense>
        <v-list-item-group
          v-model="group"
          active-class="deep-purple--text text--accent-4"
        >
          <v-list-item>
            <v-list-item-title @click="$router({path: '/events'})">EVENTS</v-list-item-title>
          </v-list-item>
        </v-list-item-group>
      </v-list>
    </v-navigation-drawer>
  </v-card>
</template>

<script>
export default {
  data: () => ({
    drawer: false,
    group: null
  }),

  watch: {
    group() {
      this.drawer = false;
    }
  }
};
</script>
