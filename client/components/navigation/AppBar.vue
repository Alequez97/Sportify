<template>
  <nav>
    <v-app-bar color="teal" dense dark app>
      <v-app-bar-nav-icon @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title class="pl-0" app>
        <NuxtLink to="/">Sportify</NuxtLink>
      </v-toolbar-title>

      <v-spacer />

      <v-btn text>
        <span>Sign Out</span>
        <v-icon right>mdi-logout</v-icon>
      </v-btn>
    </v-app-bar>

    <v-navigation-drawer class="teal lighten-5" disable-resize-watcher v-model="drawer" absolute temporary app>
      <v-container>

        <v-row>
          <v-col col="12" justify="center" align="center" class="mt-5">
            <v-avatar size="100">
              <img src="/kot_fleks.jpg">
            </v-avatar>
            <p color="black" class="mt-1 mb-0">JS</p>
          </v-col>
        </v-row>

        <PopupEventForm />

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
  })
}
</script>
