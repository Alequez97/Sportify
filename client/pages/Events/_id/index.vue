<template>
  <v-card class="mx-auto" max-width="800">
    <v-container>
      <v-row v-if="event && event.venue">
        <v-col class="col-lg-6 col-12 my-auto">
          <v-img :src="'/' + event.categoryName + '.png'" />
        </v-col>

        <v-col class="col-lg-6 col-12 my-auto">
          <v-card-text>
            <div class="text-lg-h4 text-h5 my-0">{{ event.title }}</div>
            <div class="text-lg-h5 text-h5 mb-2">{{ event.categoryName }}</div>

            <div class="text-lg-subtitle-1 font-weight-regular">{{ event.venue.country }}, {{ event.venue.city }},
              {{ event.venue.address }}</div>
            <div class="text-lg-subtitle-1 font-weight-regular mb-2">{{ localDate }}</div>

            <div class="text-lg-h5 text-h6">Contributors</div>
            <v-avatar v-for="n in 4" :key="n" size="45" color="indigo" class="mr-1">
              <img
                src="/kot_fleks.jpg"
                alt="John"
              >
            </v-avatar>
          </v-card-text>
        </v-col>
      </v-row>

      <v-row class="mt-0">
        <v-col class="col-12">
          <v-card-text>
            <div class="text-lg-h4 text-h6">Description</div>
            <div class="text-lg-subtitle-1 font-weight-regular">{{ event.description }}</div>
          </v-card-text>
        </v-col>
      </v-row>

      <v-row>
        <v-col class="col-12">
          <v-img class="pic" src="/map.png" />
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>

<script>
export default {
  data() {
    return {
      event: {}
    }
  },

  computed: {
    localDate() {
      return new Date(this.event.date).toLocaleString('en-GB', {hour12: false});
    }
  },

  async created() {
    try {
      var res = await this.$axios.get(`/api/event/${this.$route.params.id}`);
      this.event = res.data;
    } catch (err) {
      console.log(err);
    }
  }
}
</script>

<style>
</style>
