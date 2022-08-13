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
            <v-form ref="registerForm" class="px-6" @submit.prevent="register">
              <v-text-field
                v-model="email"
                :rules="emailRules"
                label="Email"
                color="teal"
              />
              <v-text-field
                v-model="username"
                :rules="usernameRules"
                label="Username"
                color="teal"
              />
              <v-text-field
                v-model="password"
                :rules="passwordRules"
                type="password"
                label="Password"
                color="teal"
              />
              <v-card-actions class="px-10">
                <v-btn color="teal" block outlined type="submit">
                  Sign Up
                </v-btn>
              </v-card-actions>
            </v-form>
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
      email: '',
      emailRules: [
        v => !!v || "Email is required",
        v => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(v) || "Email is not valid"
      ],

      username: '',
      usernameRules: [
        v => !!v || "Username is required"
      ],

      password: '',
      passwordRules: [
        v => !!v || "Password is required",
        v => (v && v.length >= 6) || "Minimum length is 6 characters",
        v => /\d/.test(v) || "Digit is required",
        v => /\W/.test(v) || "Non-Alphanumeric is required",
        v => /[A-Z]/.test(v) || "Upper is required",
        v => /[a-z]/.test(v) || "Lower is required"
      ]
    }
  },
  methods: {
    async register() {
      if (this.$refs.registerForm.validate()){
        try {
          const registerData = {
            username: this.username,
            email: this.email,
            password: this.password
          }

          const user = await this.$axios.$post("/api/accounts/register", registerData);
          console.log(user);
          this.$router.replace('/events');
        } catch (err) {
          console.log(err);
        }
      }
    }
  }
};
</script>

<style>
</style>
