<template>
  <v-app>
    <AppBar/>

    <v-main class="deep-purple lighten-5">
      <Nuxt/>
    </v-main>

    <Footer/>
  </v-app>
</template>

<script>
import AppBar from "../components/navigation/AppBar";
import Footer from "../components/navigation/Footer";

export default {
  components: {
    AppBar,
    Footer
  }
};
</script>
