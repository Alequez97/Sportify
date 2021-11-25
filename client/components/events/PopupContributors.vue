<template>
  <v-dialog v-model="dialog" max-width="300px">
    <template #activator="{ on, attrs }">
      <div class="d-inline-block font-weight-bold" v-bind="attrs" v-on="on">
          and {{count}} more
      </div>
    </template>

    <v-card>
      <v-card-title class="justify-center text-h5"> Contributors </v-card-title>
        <v-divider></v-divider>
        <v-card-text class="pa-0">
          <v-list>
            <v-list-item v-for="contr in contributors" :key="contr.id">
              <v-list-item-title>
                <v-avatar size="35" color="teal lighten-2" class="mr-2">
                  <span class="white--text text-subtitle-2 text-uppercase">{{contr.username}}</span>
                </v-avatar>
                {{contr.username}}
              </v-list-item-title>
            </v-list-item>
          </v-list>
        </v-card-text>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: 'PopupContributors',
  props: {
    count: null,
    contributors: []
  },
  data() {
      return {
          dialog: false
      }
  }
};
</script>

<style>
</style>
