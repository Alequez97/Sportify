<template>
  <v-row class="mb-0 mt-3">
    <v-col justify="center" align="center">
      <v-dialog v-model="dialog" persistent max-width="600px">
        <template #activator="{ on, attrs }">
          <v-btn color="teal" dark v-bind="attrs" v-on="on">
            Create Event
          </v-btn>
        </template>

        <v-card>
          <v-card-title>
            <span class="text-h5">Create Event</span>
          </v-card-title>
          <v-card-text>
            <v-form ref="eventForm">
              <v-text-field
                v-model="title"
                :rules="titleRules"
                label="Title" color="teal"
                append-icon="mdi-pencil"
              />
              <v-select
                v-model="categoryId"
                :items="categories"
                :rules="categoryRules"
                label="Category" item-text="name"
                item-value="id"
                color="teal"
              />
              <v-text-field
                v-model="briefDesc"
                :rules="briefDescRules"
                label="Brief Description"
                color="teal"
                append-icon="mdi-information-outline"
              />
              <v-textarea
                v-model="description"
                :rules="descriptionRules"
                label="Description"
                color="teal"
              />
              <v-select
                v-model="countryId"
                :rules="countryRules"
                :items="countries"
                label="Country"
                item-text="name"
                item-value="id"
                color="teal"
                @change="onCountrySelect()"
              />
              <v-select
                v-model="cityId"
                :rules="cityRules"
                :items="cities"
                label="City"
                item-text="name"
                item-value="id"
                color="teal"
                :disabled="!countrySelected"
              />
              <v-text-field
                v-model="address"
                :rules="addressRules"
                label="Address"
                color="teal"
                append-icon="mdi-map-marker-outline"
              />
              <PopupDatePicker @bindDate="bindDate"/>
              <PopupTimePicker @bindTime="bindTime" />
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer />
            <v-btn color="teal" text @click="cancel()">
              Cancel
            </v-btn>
            <v-btn color="teal" text @click="createEvent()">
              Save
            </v-btn>
          </v-card-actions>
        </v-card>
    </v-dialog>
    </v-col>
  </v-row>
</template>

<script>
import PopupDatePicker from './PopupDatePicker.vue';
import PopupTimePicker from './PopupTimePicker.vue';

export default {
  name: 'PopupEventForm',
  components: {
    PopupDatePicker,
    PopupTimePicker
  },
  data() {
    return {
      dialog: false,

      title: '',
      titleRules: [
        v => !!v || 'Title is required',
        v => (v && v.length >= 3) || 'Minimum length is 3 characters'
      ],

      categoryId: null,
      categoryRules: [
        v => v != null || 'Category is required'
      ],

      briefDesc: '',
      briefDescRules: [
        v => !!v || 'Brief description is required',
        v => (v && v.length >= 5) || 'Minimum length is 5 characters',
        v => (v && v.length <= 50) || 'Maximum length is 50 characters'
      ],

      description: '',
      descriptionRules: [
        (v) => {
          if (v) {
            return v.length <= 1000 || 'Maximum length is 1000 characters';
          } else {
            return true;
          }
        }
      ],

      countryId: null,
      countryRules: [
        v => v != null || 'Country is required'
      ],
      cityId: null,
      cityRules: [
        v => v != null || 'City is required'
      ],

      address: '',
      addressRules: [
        v => !!v || 'Address is required'
      ],

      dateOfTheEvent: null,
      dateOfTheEventRules: [
        v => v != null || 'Date is required'
      ],

      timeOfTheEvent: null,
      timeOfTheEventRules: [
        v => v != null || 'Date is required'
      ]
    };
  },
  async created(){
    await this.$store.dispatch('events/fetchCategories');
    await this.$store.dispatch('events/fetchCountries');
  },
  computed: {
    categories() { return this.$store.getters['events/getCategories'] },
    countries() { return this.$store.getters['events/getCountries'] },
    cities() { return this.$store.getters['events/getCities'] },
    countrySelected() {
      return this.countryId != null;
    }
  },
  methods: {
    async createEvent() {
      if (this.$refs.eventForm.validate()) {
        await this.$store.dispatch('googleMap/prepare');
        const latLng = await this.$store.dispatch('googleMap/getGeolocationFromAddressAsync', this.address);
        const eventData = {
          title: this.title,
          categoryId: this.categoryId,
          briefDesc: this.briefDesc,
          description: this.description,
          date: new Date(this.dateOfTheEvent.replace(/-/g, "/") + ' ' + this.timeOfTheEvent).toISOString(),
          countryId: this.countryId,
          cityId: this.cityId,
          address: this.address,
          lat: latLng.lat,
          lng: latLng.lng
        }

        await this.$store.dispatch('events/createEvent', eventData);

        this.$refs.eventForm.reset();
        this.dialog = false;
      }
    },
    async onCountrySelect() {
      await this.$store.dispatch('events/fetchCities', this.countryId);
    },
    cancel() {
      this.$refs.eventForm.reset();
      this.dialog = false;
    },
    bindTime(time) {
      this.timeOfTheEvent = time;
    },
    bindDate(date) {
      this.dateOfTheEvent = date;
    }
  }
}
</script>
