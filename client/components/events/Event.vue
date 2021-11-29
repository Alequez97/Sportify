<template>
  <v-card
    class="mx-auto my-3 rounded-lg"
    outlined
    elevation="24"
    max-width="600px"
  >
    <v-container>
      <v-row justify="center" align="center">
        <v-col cols="8" class="pr-0 ">
          <v-card-title class="py-0 mb-2">
            <p class="text-lg-h5 text-h6 my-0">
              {{ event.title }}
            </p>
          </v-card-title>
          <v-card-text class="py-0">
            <div class="my-0 text-lg-subtitle-1 text-subtitle-2 font-weight-regular">
              {{ event.venue.country }}, {{ event.venue.city }},
              {{ event.venue.address }}
            </div>
            <div class="text-lg-subtitle-1 font-weight-regular my-0">
              {{ localDate }}
            </div>
            <div class="text-lg-subtitle-1 font-weight-regular mb-2">
              Creator: {{ event.creatorName }}
            </div>
            <div class="text-lg-subtitle-1 font-weight-regular mb-2">
              {{ event.description }}
            </div>
            <div class="text-lg-h5 text-h6 mb-0">
              Contributors
            </div>
            <div v-if="contributorsExist > 3" class="mb-0">
              <div class="d-inline-flex">
                <v-tooltip v-for="cont in event.contributors.slice(0,3)" :key="cont.id" top>
                  <template v-slot:activator="{ on, attrs }">
                    <v-avatar v-bind="attrs" v-on="on" size="35" color="teal lighten-2" class="mt-1 mb-2 mr-2">
                      <v-icon dark>
                        mdi-account-circle
                      </v-icon>
                    </v-avatar>
                  </template>
                  <span>{{cont.username}}</span>
                </v-tooltip>
              </div>
              <PopupContributors :count="contributorsExist - 3" :contributors="event.contributors"/>
            </div>
            <div v-else-if="contributorsExist">
              <div>
                <v-tooltip v-for="cont in event.contributors.slice(0,3)" :key="cont.id" top>
                  <template v-slot:activator="{ on, attrs }">
                    <v-avatar v-bind="attrs" v-on="on" size="35" color="teal lighten-2" class="mt-1 mb-2 mr-2">
                      <v-icon dark>
                        mdi-account-circle
                      </v-icon>
                    </v-avatar>
                  </template>
                  <span>{{cont.username}}</span>
                </v-tooltip>
              </div>
            </div>
            <div v-else>
              <div class="text-lg-subtitle-1 font-weight-regular mb-2">Be the first</div>
            </div>
          </v-card-text>
          <v-card-actions class="py-0">
            <v-btn class="text-subtitle-1" nuxt :to="'/events/' + event.id" color="deep-purple" text rounded>
              Show
            </v-btn>
            <v-btn class="text-subtitle-1" text rounded :color="actionText.color" @click="handleJoin(event.id, event.isGoing)">
              {{ actionText.text }}
            </v-btn>
          </v-card-actions>
        </v-col>
        <v-col cols="4">
          <v-img lazy-src="lazy-load.png" :src="event.categoryName + '.png'" />
        </v-col>
      </v-row>
    </v-container>
  </v-card>
</template>

<script>
import PopupContributors from './PopupContributors.vue';

export default {
  name: 'Event',
  props: {
    event: {
      type: Object,
      default: null
    }
  },
  components: {
    PopupContributors
  },
  computed: {
    actionText() {
      if (!this.event.isGoing) {
        return { color: 'green', text: "Join" };
      } else {
        return { color: 'red', text: "Drop" };
      }
    },
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    },
    contributorsExist() {
      return this.event.contributors.length;
    },
    localDate() {
      return new Date(this.event.date).toLocaleString('en-GB', {hour12: false});
    }
  },
  methods: {
    handleJoin(eventId, isGoing) {
      if (this.isAuthenticated) {
        const eventData = {
          eventId,
          isGoing
        }
        this.$emit('join', eventData);
      } else {
        this.$router.push('login');
      }
    }
  }
};
</script>
