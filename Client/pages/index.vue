<template>
  <div>
    <h1>Welcome to Sportify!</h1>
    <Event v-for="event in events" :key="event.id" :event="event"/>
  </div>
</template>

<script>
import axios from 'axios';
import Event from '../components/Event';

export default {
  components: {
    Event
  },
  data() {
    return {
      events: []
    };
  },
  async created(){
    // const config = {
    //   headers: {
    //     Accept: 'application/json'
    //   }
    // };
    await axios.get('https://localhost:44314/api/events').then(response => {
      this.events = response.data
    }).catch((error)=>{
      console.log(error)
    });
  },
  head() {
    return {
      title: 'Welcome to Sportify!',
      meta: [
      {
        hid: 'description',
        name: 'description',
        content: "The best place to share and populate sports"
      }
      ]
    }
  }
}
</script>

<style>

</style>
