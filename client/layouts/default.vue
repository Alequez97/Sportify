<template>
  <v-app>
    <AppBar/>

    <v-main>
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
