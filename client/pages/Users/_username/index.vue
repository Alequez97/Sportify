<template>
  <v-card max-width="800px" class="mx-auto">
    <v-container v-if="user">
      <v-row>
        <v-col class="col-6" align-self="center">
            <v-avatar size="300">
              <v-img src="/kot_fleks.jpg" />
            </v-avatar>
        </v-col>
        <v-col class="col-6">
            <v-card-title class="text-h4">
              Profile
            </v-card-title>
            <div>
              {{user.username}}
            </div>
            <div>
              {{user.email}}
            </div>
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>

<script>
export default {
  data() {
    return {
      user: {}
    }
  },
  async created() {
    // debugger;
    try {
      await this.$axios.get("api/users/" + this.$route.params.username)
      .then((response) => { this.user = response.data });
    } catch (err) {
      console.log(err);
    }
  }
}
</script>

<style>

</style>
