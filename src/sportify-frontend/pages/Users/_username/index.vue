<template>
  <v-container fluid class="px-0 pt-0">
    <v-row justify="center" class="my-2">
      <v-col justify="center" align="center" class="col-xl-2">
        <v-avatar size="300">
          <v-img src="/kot_fleks.jpg" />
        </v-avatar>
      </v-col>
      <v-col justify="center" class="col-xl-3">
        <v-card>
          <v-card-title>
            <div class="text-h5">
              Profile Data
            </div>
          </v-card-title>
          <v-card-text>
            <div class="text-subtitle-1 font-weight-regular">
              Username: {{ user.username }}
            </div>
            <div class="text-subtitle-1 font-weight-regular">
              Email: {{ user.email }}
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  data() {
    return {
      user: {}
    }
  },
  async created() {
    try {
      await this.$axios.get("api/accounts/user-info/" + this.$route.params.username)
      .then((response) => { this.user = response.data });
    } catch (err) {
      console.log(err);
    }
  }
}
</script>

<style>
</style>
