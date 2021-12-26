<template>
  <v-container fluid class="px-0 pt-0">
    <v-tabs
      v-model="tab"
      background-color="teal"
      dark
    >
      <v-tab
        v-for="item in items"
        :key="item.tab"
      >
        {{ item.tab }}
      </v-tab>
    </v-tabs>

    <v-tabs-items v-model="tab" id="custom-tab-items">
      <v-tab-item>
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
      </v-tab-item>
      <v-tab-item>
        <v-row v-for="event in events" :key="event.id" no-gutters>
          <v-col col="12">
            <Event :event="event" @join="join" />
          </v-col>
        </v-row>
      </v-tab-item>
      <v-tab-item></v-tab-item>
    </v-tabs-items>
  </v-container>
</template>

<script>
import Event from "../../../components/events/Event";

export default {
  components: {
    Event
  },
  data() {
    return {
      user: {},

      tab: null,
      items: [
        { tab: "My Profile", content: 'Tab 2 Content' },
        { tab: 'My Events', content: 'Tab 1 Content' },
        { tab: "I'm going", content: 'Tab 2 Content' }
      ]
    }
  },
  computed: {
    events() {
      return this.$store.getters['events/getEvents'].filter(e => e.creatorId === this.$auth.$state.user.id);
    }
  },
  async created() {
    try {
      await this.$axios.get("api/users/" + this.$route.params.username)
      .then((response) => { this.user = response.data });
    } catch (err) {
      console.log(err);
    }

    await this.$store.dispatch('events/fetchEvents');
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
}
</script>

<style>
#custom-tab-items {
    background-color: transparent !important;
}
</style>
