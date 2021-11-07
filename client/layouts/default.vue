<template>
  <v-app>
    <AppBar/>

    <v-main class="deep-purple lighten-5">
      <Nuxt/>
    </v-main>

  </v-app>
</template>

<script>
import AppBar from "../components/navigation/AppBar";

export default {
  components: {
    AppBar
  }
};
</script>
