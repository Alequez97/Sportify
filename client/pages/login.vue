<template>
    <v-form>
      <v-text-field v-model="email" label="Email" color="deep-purple" />
    </v-form>
</template>

<script>
export default {
  data() {
    return {
      email: "",
      password: ""
    }
  }
}
</script>

<style>
</style>
