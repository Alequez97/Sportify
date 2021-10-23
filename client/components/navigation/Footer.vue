<template>
  <v-footer color="deep-purple" padless absolute app>
    <v-row justify="center" no-gutters>
      <v-btn to="/about" color="white" text rounded class="my-2">About</v-btn>
      <v-col class="text-center white--text" cols="12">
        {{ new Date().getFullYear() }} — <strong>Sportify</strong>
      </v-col>
    </v-row>
  </v-footer>
</template>

<script>
export default{
  name: 'Footer'
}
</script>