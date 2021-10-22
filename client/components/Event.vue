<template>
  <v-card class="mx-auto my-3 rounded-lg" outlined elevation="24" max-width="576px">
    <v-container>
      <v-row justify="center" align="center">
        <v-col cols="8">
          <v-card-title class="py-0 mb-2">
            <div>{{ event.title }}</div>
          </v-card-title>
          <v-card-text class="py-0">
            <p class="text-justify my-0">
              {{ event.venue.country }}, {{ event.venue.city }}, {{ event.venue.address }}
            </p>
            <p class="text-justify my-0">{{ event.timeOfTheEvent }}</p>
            <p class="mb-2">Creator: {{ event.creator }}</p>
            <p class="text-justify mb-2">{{ event.description }}</p>
            <p class="text-h6 mb-0">Contributors</p>
            <v-avatar v-for="n in 4" :key="n" size="40" color="indigo" class="mb-2 mr-1">
              <img
                src="kot_fleks.jpg"
                alt="John"
              >
            </v-avatar>
          </v-card-text>
          <v-card-actions class="py-0">
            <v-btn nuxt :to="'/events/' + event.id" color="deep-purple" text rounded>Show</v-btn>
            <v-btn color="green" text rounded>Join</v-btn>
          </v-card-actions>
        </v-col>
        <v-col cols="4">
          <v-img lazy-src="lazy-load.png" :src="event.category + '.png'" />
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>

<script>
export default {
  name: "Event",
  props: ["event"]
};
</script>

<style>
a.nuxt-link-exact-active {
  color: #fff;
}

a,
a:visited {
  text-decoration: none;
  color: inherit;
}
</style>
