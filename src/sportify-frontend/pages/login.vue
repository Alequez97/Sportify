<template>
  <v-container fluid fill-height class="background">
    <v-row justify="center" align="center">
      <v-col>
        <v-card class="mx-auto" outlined elevation="24" max-width="400px">
          <v-card-text class="text-center pt-0 mt-5">
            <div class="text-lg-h5 text-h5">
              Welcome back
            </div>
          </v-card-text>

          <v-card-text class="pt-0">
            <v-form ref="loginForm" class="px-6" @submit.prevent="login">
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
                  Sign In
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
      username: "",
      usernameRules: [
        v => !!v || "Username is required"
      ],

      password: "",
      passwordRules: [
        v => !!v || "Password is required"
      ]
    };
  },
  methods: {
    async login() {
      if (this.$refs.loginForm.validate()) {
        try {
          const loginData = {
            username: this.username,
            password: this.password
          };
          const response = await this.$auth.loginWith("local", {
            data: loginData
          });
          this.$router.replace("/events");
          console.log(response);
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
