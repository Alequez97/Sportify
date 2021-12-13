<template>
  <v-container fluid>
    <EventFilters />
    <v-row v-for="event in events" :key="event.id" no-gutters>
      <v-col col="12">
        <Event :event="event" @join="join" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import Event from "../../components/events/Event";
import EventFilters from "../../components/events/EventFilters"

export default {
  components: {
    Event,
    EventFilters
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
  computed: {
    events() {
      return this.$store.getters['events/getEvents'];
    }
  },
  async created() {
    await this.$store.dispatch('events/fetchEvents');
    await this.$store.dispatch('events/fetchCategories');
    await this.$store.dispatch('events/fetchCountries');
  },
  methods: {
    async join(eventData) {
      if (!eventData.isGoing) {
        await this.$store.dispatch('events/join', eventData.eventId);
      } else {
        await this.$store.dispatch('events/disjoin', eventData.eventId);
      }
    }
  }
};
</script>

<style>
</style>
