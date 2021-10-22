<template>
  <!-- <v-container class="grey lighten-5" fluid> -->
  <v-container fluid>
    <v-row v-for="event in events" :key="event.id" no-gutters>
      <v-col sm="12">
        <Event :event="event" />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import axios from "axios";
import Event from "../../components/Event";

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
    this.events = [
      {
        id: 1,
        title: 'Basketball in Ogre',
        creator: 'JS',
        category: 'basketball',
        venue: {
          country: 'Latvia',
          city: 'Ogre',
          address: 'Skolas iela 1'
        },
        timeOfTheEvent: '30/12/1999',
        description: 'We are going to play street basketball 3x3. Wanna play with us?',
      },
      {
        id: 2,
        title: 'Basketball in Ogre',
        creator: 'JS',
        category: 'basketball',
        venue: {
          country: 'Latvia',
          city: 'Ogre',
          address: 'Skolas iela 1'
        },
        timeOfTheEvent: '30/12/1999',
        description: 'We are going to play street basketball 3x3. Wanna play with us?'
      },
      {
        id: 3,
        title: 'Basketball in Ogre',
        creator: 'JS',
        category: 'basketball',
        venue: {
          country: 'Latvia',
          city: 'Ogre',
          address: 'Skolas iela 1'
        },
        timeOfTheEvent: '30/12/1999',
        description: 'We are going to play street basketball 3x3. Wanna play with us?'
      }
    ];

    await axios
      .get("https://localhost:44314/api/events")
      .then((response) => {
        this.events = response.data;
      })
      .catch((error) => {
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
