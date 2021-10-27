<template>
  <div id="google-map" class="align-center" />
</template>

<script>
export default {
  name: "GoogleMap",
  props: ["geolocations"],
  head: {
    script: [
      {
        src: "/js/google-maps.js",
        body: true
      },
      {
        src: "https://unpkg.com/axios/dist/axios.min.js",
        body: true
      },
      {
        src: "https://maps.googleapis.com/maps/api/js?key=AIzaSyBN1KwhF7F_Fap613-64jjEd2iABt488kM&callback=initMap&libraries=&v=weekly",
        body: true,
        async: true
      }
    ]
  }
};
</script>

<style scoped>
    #google-map {
        width: 80%;
        height: 400px;
    }

    .align-center {
        margin: 0 auto;
    }
</style>
