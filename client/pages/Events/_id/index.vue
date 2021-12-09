<template>
  <v-card class="mx-auto" max-width="800">
    <v-container>
      <v-row v-if="event && event.venue">
        <v-col class="col-lg-6 col-12">
          <v-img contain max-width="100%" max-height="280px" :src="'/' + event.categoryName + '.png'" />
          <!-- <v-img contain max-height="300px" class="photo" src="/audi.jpg" /> -->
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

      <v-row class="mt-0">
        <v-col class="col-12">
          <div id="google-map-event-details"></div>
        </v-col>
      </v-row>

      <v-row>
        <v-col class="col-12">
          <v-img src="/chatPreview.png"></v-img>
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
      await this.$axios.get(`/api/event/${this.$route.params.id}`).then((response) => {
        this.event = response.data;
      });
    } catch (err) {
      console.log(err);
    }
  },
  updated() {
    this.renderEventLocationIfExists();
  },
  methods: {
    async renderEventLocationIfExists() {
      if (this.event.venue.lat !== undefined && this.event.venue.lng !== undefined) {
        document.getElementById('google-map-event-details').style.height = '300px';

        await this.$store.dispatch('googleMap/prepare');
        const google = this.$store.getters['googleMap/getGoogleObject'];

        const mapOptions = {
          center: {
            lat: this.event.venue.lat,
            lng: this.event.venue.lng
          },
          zoom: 14
        };
        const map = new google.maps.Map(document.getElementById("google-map-event-details"), mapOptions);

        const markerPosition = { lat: this.event.venue.lat, lng: this.event.venue.lng };
        const marker = new google.maps.Marker({
          position: markerPosition
        });
        marker.setMap(map);
      }
    }
  }
}
</script>

<style>
</style>
