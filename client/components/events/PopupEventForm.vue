<template>
  <v-dialog @input="onDialogOpen()" v-model="dialog" persistent max-width="600px">
    <template v-if="event == null" #activator="{ on, attrs }">
      <v-btn color="teal" dark v-bind="attrs" v-on="on">
        Create Event
      </v-btn>
    </template>
    <template v-else #activator="{ on, attrs }">
      <v-btn text rounded color="teal" dark v-bind="attrs" v-on="on">
        <v-icon>
          mdi-pencil
        </v-icon>
      </v-btn>
    </template>

    <v-card>
      <v-card-title>
        <span class="text-h5">{{ formTitle }}</span>
      </v-card-title>
      <v-card-text>
        <v-form ref="eventForm">
          <v-text-field
            v-model="title"
            :rules="titleRules"
            label="Title"
            color="teal"
            append-icon="mdi-pencil"
          />
          <v-autocomplete
            v-model="categoryId"
            :items="categories"
            :rules="categoryRules"
            label="Category"
            item-text="name"
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
          <v-autocomplete
            v-model="countryId"
            autocomplete="nope"
            :rules="countryRules"
            :items="countries"
            label="Country"
            item-text="name"
            item-value="id"
            color="teal"
            @change="onCountrySelect()"
          />
          <v-autocomplete
            v-model="cityId"
            autocomplete="nope"
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
            autocomplete="nope"
            :rules="addressRules"
            label="Address"
            color="teal"
            append-icon="mdi-map-marker-outline"
          />
          <template>
            <v-dialog ref="dateDialog" v-model="dateDialog" :return-value.sync="dateOfTheEvent" persistent width="290px">
              <template v-slot:activator="{ on, attrs }">
                <v-text-field v-model="dateOfTheEvent" :rules="[rules.required('Date')]" label="Choose date" append-icon="mdi-calendar" color="teal" readonly v-bind="attrs" v-on="on"></v-text-field>
              </template>
              <v-date-picker v-model="dateOfTheEvent" :min="minDate" color="teal" scrollable>
                <v-spacer></v-spacer>
                <v-btn text color="teal" @click="dateDialog = false"> Cancel </v-btn>
                <v-btn text color="teal" @click="setDate()"> OK </v-btn>
              </v-date-picker>
            </v-dialog>
          </template>
          <template>
            <v-dialog ref="timeDialog" v-model="timeDialog" :return-value.sync="timeOfTheEvent" persistent width="290px">
              <template v-slot:activator="{ on, attrs }">
                <v-text-field v-model="timeOfTheEvent" :rules="[rules.required('Time')]" label="Сhoose time" append-icon="mdi-clock-time-four-outline" color="teal" readonly v-bind="attrs" v-on="on"></v-text-field>
              </template>
              <v-time-picker v-model="timeOfTheEvent" format="24hr" color="teal" full-width>
                <v-spacer></v-spacer>
                <v-btn text color="teal" @click="timeDialog = false"> Cancel </v-btn>
                <v-btn text color="teal" @click="setTime()"> OK </v-btn>
              </v-time-picker>
            </v-dialog>
          </template>
          <!-- <PopupDatePicker :dateOfTheEvent="event != null ? dateOfTheEvent : ''" @bindDate="bindDate" />
          <PopupTimePicker :timeOfTheEvent="event != null ? timeOfTheEvent : ''" @bindTime="bindTime" /> -->
          <v-checkbox
            v-model="isGoing"
            :label="`I'm going`"
            color="teal"
          />
        </v-form>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-btn color="teal" text @click="closeForm()">
          Cancel
        </v-btn>
        <v-btn color="teal" text @click="submitEventForm()">
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
export default {
  name: 'PopupEventForm',
  props: {
    formTitle: {
      type: String,
      default: ""
    },
    event: {
      type: Object,
      default: null
    }
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

      dateOfTheEvent: '',
      timeOfTheEvent: '',

      dateDialog: false,
      timeDialog: false,

      isGoing: false,

      rules: {
        required(fieldName) {
          return v => !!v || `${fieldName} is required`;
        }
      }
    };
  },
  computed: {
    categories() { return this.$store.getters['events/getCategories'] },
    countries() { return this.$store.getters['events/getCountries'] },
    cities() { return this.$store.getters['events/getCitiesForEventForm'] },
    countrySelected() {
      return this.countryId != null;
    },
    minDate() {
      return (new Date(Date.now() - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10);
    }
  },
  methods: {
    async submitEventForm() {
      if (this.$refs.eventForm.validate()) {
        await this.$store.dispatch('googleMap/prepare');

        const country = this.countries.find(c => c.id === this.countryId);
        const city = this.cities.find(c => c.id === this.cityId);
        const fullAddress = country.name + ", " + city.name + ", " + this.address;

        let latLng;
        try {
          latLng = await this.$store.dispatch('googleMap/getGeolocationFromAddressAsync', fullAddress);
        } catch (error) {
          latLng = { lat: undefined, lng: undefined };
        }
        debugger;
        const eventData = {
          id: this.event !== null ? this.event.id : null,
          title: this.title,
          categoryId: this.categoryId,
          briefDesc: this.briefDesc,
          description: this.description,
          date: new Date(this.dateOfTheEvent.replace(/-/g, "/") + ' ' + this.timeOfTheEvent).toISOString(),
          countryId: this.countryId,
          cityId: this.cityId,
          address: this.address,
          lat: latLng.lat,
          lng: latLng.lng,
          isGoing: this.isGoing
        }

        if (this.event == null) {
          await this.$store.dispatch('events/createEvent', eventData);
        } else {
          await this.$store.dispatch("events/editEvent", eventData);
        }

        this.closeForm();
      }
    },
    closeForm() {
      if (this.event == null) {
        this.$refs.eventForm.reset();
        this.isGoing = false;
      }
      this.dialog = false;
    },
    async onCountrySelect() {
      this.cityId = null;
      await this.$store.dispatch('events/fetchCitiesForEventForm', this.countryId);
    },
    setDate() {
      this.$refs.dateDialog.save(this.dateOfTheEvent);
      this.dateDialog = false;
    },
    setTime() {
      this.$refs.timeDialog.save(this.timeOfTheEvent);
    },
    async onDialogOpen() {
      if (this.event != null) {
        this.title = this.event.title;
        this.categoryId = this.event.categoryId;
        this.briefDesc = this.event.briefDesc;
        this.description = this.event.description;
        this.countryId = this.event.venue.countryId;
        this.cityId = this.event.venue.cityId;
        this.address = this.event.venue.address;
        this.isGoing = this.event.isGoing;
        this.dateOfTheEvent = this.event.date.split("T")[0];
        this.timeOfTheEvent = new Date(this.event.date).toLocaleTimeString('en-GB', {hour: '2-digit', minute: '2-digit', hour12: false});

        await this.$store.dispatch('events/fetchCitiesForEventForm', this.countryId);
      }
    }
  }
}
</script>
