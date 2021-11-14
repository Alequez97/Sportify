<template>
  <v-container fluid>
    <v-row v-for="event in events" :key="event.id" no-gutters>
      <v-col col="12">
        <Event :event="event" @join="join"/>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import Event from "../../components/events/Event";

export default {
  components: {
    Event
  },
  data() {
    return {
      events: []
    };
  },
  head() {
    return {
      title: "Welcome to Sportify!",
      meta: [
        {
          hid: "description",
          name: "description",
          content: "The best place to share and populate sports"
        }
      ]
    };
  },
  async created() {
    await this.$axios.get("https://localhost:44314/api/events").then((response) => {
        this.events = response.data;
      }).catch((error) => {
        console.log(error);
      });
  },
  methods: {
    async join(eventData) {
      if (!eventData.isGoing) {
        await this.$store.dispatch('events/join', eventData.eventId)
        .then(this.events.find(x => x.id === eventData.eventId).isGoing = true)
        .catch(err => console.log(err));
      } else {
        await this.$store.dispatch('events/disjoin', eventData.eventId)
        .then(this.events.find(x => x.id === eventData.eventId).isGoing = false)
        .catch(err => console.log(err));
      }
    }
  }
};
</script>

<style>
</style>
