<template>
  <v-container fluid fill-height class="background">
    <v-row justify="center" align="center">
      <v-col>
        <v-card class="mx-auto" outlined elevation="24" max-width="400px">
          <v-card-text class="text-center pt-0 mt-5">
            <div class="text-lg-h5 text-h5">
              Sign up to Sportify
            </div>
          </v-card-text>

          <v-card-text class="pt-0">
            <v-form class="px-6">
              <v-text-field v-model="email" label="Email" color="teal" />
              <v-text-field v-model="username" label="Username" color="teal" />
              <v-text-field
                v-model="password"
                type="password"
                label="Password"
                color="teal"
              />
            </v-form>
          </v-card-text>

          <v-card-actions class="px-10 mb-5">
            <v-btn color="teal" block outlined @click="register()">
              Sign Up
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  data() {
    return {
      email: '',
      username: '',
      password: ''
    }
  },
  methods: {
    async register() {
      try {
        const registerData = {
          username: this.username,
          email: this.email,
          password: this.password
        }

        const user = await this.$axios.$post("/api/accounts/register", registerData);
        console.log(user);
        this.$router.replace('/');
      } catch (err) {
        console.log(err);
      }
    }
  }
};
</script>

<style>
</style>
