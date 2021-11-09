<template>
  <v-container>
    <v-row justify="center" align="center">
      <v-col class="col-12">
        <v-card
          class="mx-auto rounded-lg"
          outlined
          elevation="24"
          max-width="600px"
        >
          <v-card-title class="justify-center">
            <div class="text-lg-h4">
              Welcome Back
            </div>
          </v-card-title>

          <v-card-text>
            <v-form>
              <v-text-field v-model="email" label="Email" color="teal" />
              <v-text-field v-model="password" type="password" label="Password" color="teal" />
            </v-form>
          </v-card-text>

          <v-card-actions>
            <v-btn color="teal" block outlined @click="login()">
              Login
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
      password: ''
    }
  },
  methods: {
    async login() {
      const data = {
        username: this.email,
        password: this.password
      };

      await this.$store.dispatch("auth/login", data)
      .then(this.$router.push('/'))
      .catch(err => console.log(err));
    }
  }
}
</script>

<style>
</style>
