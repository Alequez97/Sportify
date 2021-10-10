<template>
  <div>
      <p>{{ event.title }}</p>
      <p>{{ event.description }}</p>
      <p>{{ event.venue.country }}</p>
      <p>{{ event.venue.city }}</p>
      <p>{{ event.venue.address }}</p>
      <p>{{ event.timeOfTheEvent }}</p>
      <hr />
  </div>
</template>

<script>
export default {
    name: 'Event',
    props: ['event']
}
</script>

<style>

</style>