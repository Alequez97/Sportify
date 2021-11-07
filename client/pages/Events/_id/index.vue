<template>
  <v-card class="mx-auto" max-width="800">
    <v-container>
      <v-row>
        <v-col class="col-lg-6 col-12 my-auto">
          <v-img src="/audi.jpg" />
        </v-col>

        <v-col class="col-lg-6 col-12 my-auto">
          <v-card-text>
            <div class="text-lg-h4 text-h5 my-0">{{ event.title }}</div>
            <div class="text-lg-h5 text-h5 mb-2">{{ event.categoryName }}</div>

            <div class="text-lg-subtitle-1 font-weight-regular">{{ event.venue.country }}, {{ event.venue.city }}, {{ event.venue.address }}</div>
            <div class="text-lg-subtitle-1 font-weight-regular mb-2">{{ event.timeOfTheEvent }}</div>

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

  created() {
  //   try{
  //     const res = $axios.get(`https://localhost:44314/api/event/${this.$route.params.id}`);
  //   }
  //   catch(err)
  //   {
  //     console.log(err);
  //   }
  // }
    this.event = {
      title: 'Basketball in Ogre',
      categoryName: 'Basketball',
      description: 'Ogres stadions, asfalta basketbola laukums. Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.Ogres stadions, asfalta basketbola laukums.',
      venue: {
        country: 'Latvia',
        city: 'Ogre',
        address: 'Skolas iela 1'
      },
      timeOfTheEvent: '20/11/2021 17:00'
    }
  }
}
</script>

<style scoped>
 .pic{
   height: 400px;
   width: auto;
 }
</style>
