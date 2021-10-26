<template>
  <nav>
    <v-app-bar color="deep-purple" dense dark app>
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

    <v-navigation-drawer class="deep-purple lighten-5" disable-resize-watcher v-model="drawer" absolute temporary app>
      <v-list nav dense>
        <v-list-item-group
          active-class="deep-purple--text text--accent-4"
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

export default {
  name: "AppBar",
  data: () => ({
    drawer: false,
    links: [
      { icon: 'mdi-soccer', text: 'Events', route: '/events' },
      { icon: 'mdi-dumbbell', text: 'Sports Grounds & Gyms', route: '/map' }
    ]
  })
}
</script>
