<template>
  <v-row>
    <v-col col="12">
      <v-card
        class="mx-auto my-3 rounded-lg"
        outlined
        elevation="24"
        max-width="600px"
      >
        <v-card-title class="pa-0">
          <v-btn block large @click="activateFilters()">
            Filters
            <v-icon :color="filtersApplied ? 'red' : 'blue'" right>
              mdi-filter
            </v-icon>
          </v-btn>
        </v-card-title>
        <div v-if="filtersActivated">
          <v-card-text>
            <v-autocomplete
              v-model="categoryId"
              autocomplete="nope"
              :items="categories"
              item-text="name"
              item-value="id"
              label="Category"
              clearable
              solo
              hide-details
              class="my-3"
              @change="applyFilters()"
            />
            <v-autocomplete
              v-model="countryId"
              autocomplete="nope"
              :items="countries"
              item-text="name"
              item-value="id"
              label="Country"
              clearable
              solo
              hide-details
              class="my-3"
              @change="applyFiltersForCountry()"
            />
            <v-autocomplete
              v-model="cityId"
              autocomplete="nope"
              :items="cities"
              item-text="name"
              item-value="id"
              label="City"
              :disabled="!countrySelected"
              clearable
              solo
              hide-details
              @change="applyFilters()"
            />
          </v-card-text>
        </div>
      </v-card>
    </v-col>
  </v-row>
</template>

<script>
export default {
  data() {
    return {
      showFilters: false,
      categoryId: null,
      countryId: null,
      cityId: null
    }
  },
  computed: {
    categories() {
      return this.$store.getters["events/getCategories"];
    },
    countries() {
      return this.$store.getters["events/getCountries"];
    },
    cities() {
      return this.$store.getters["events/getCitiesForEventFilters"];
    },
    filtersActivated() {
      return this.showFilters;
    },
    filtersApplied() {
      return (
        this.categoryId != null || this.countryId != null || this.cityId != null
      );
    },
    countrySelected() {
      return this.countryId != null;
    }
  },
  methods: {
    activateFilters() {
      this.showFilters = !this.showFilters;
    },
    async applyFilters() {
      const filterData = {
        categoryId: this.categoryId,
        countryId: this.countryId,
        cityId: this.cityId
      }
      await this.$store.dispatch("events/applyFilters", filterData);
    },
    async applyFiltersForCountry() {
      if (this.countryId == null) {
        this.cityId = null;
      }

      await this.$store.dispatch(
        "events/fetchCitiesForEventFilters",
        this.countryId
      );
      this.applyFilters();
    }
  }
};
</script>
