<template>
  <v-container fluid>
    <v-row v-for="event in events" :key="event.id" no-gutters>
      <v-col col="12">
        <Event :event="event" />
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
  async created() {
    // const config = {
    //   headers: {
    //     Accept: 'application/json'
    //   }
    // };
    // this.events = [
    //   {
    //     id: 1,
    //     title: 'Basketball in Ogre',
    //     creator: 'JS',
    //     category: 'basketball',
    //     venue: {
    //       country: 'Latvia',
    //       city: 'Ogre',
    //       address: 'Skolas iela 1'
    //     },
    //     timeOfTheEvent: '30/12/1999',
    //     description: 'We are going to play street basketball 3x3. Wanna play with us?'
    //   },
    //   {
    //     id: 2,
    //     title: 'Basketball in Ogre',
    //     creator: 'JS',
    //     category: 'basketball',
    //     venue: {
    //       country: 'Latvia',
    //       city: 'Ogre',
    //       address: 'Skolas iela 1'
    //     },
    //     timeOfTheEvent: '30/12/1999',
    //     description: 'We are going to play street basketball 3x3. Wanna play with us?'
    //   },
    //   {
    //     id: 3,
    //     title: 'Basketball in Ogre',
    //     creator: 'JS',
    //     category: 'basketball',
    //     venue: {
    //       country: 'Latvia',
    //       city: 'Ogre',
    //       address: 'Skolas iela 1'
    //     },
    //     timeOfTheEvent: '30/12/1999',
    //     description: 'We are going to play street basketball 3x3. Wanna play with us?'
    //   }
    // ];

    await this.$axios.get("https://localhost:44314/api/events").then((response) => {
        this.events = response.data;
      }).catch((error) => {
        console.log(error);
      });
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
  }
};
</script>

<style>
</style>
