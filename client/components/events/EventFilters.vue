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
                <v-icon :color="filtersApplied ? 'red' : 'blue'" right>mdi-filter</v-icon>
            </v-btn>
        </v-card-title>
        <div v-if="filtersActivated">
          <v-card-text>
            <v-autocomplete
              v-model="categoryId"
              :items="categories"
              item-text="name"
              item-value="id"
              :change="applyFilters()"
              label="Category"
              clearable
              solo
              hide-details
              class="my-3"
            ></v-autocomplete>
            <v-autocomplete
              v-model="countryId"
              :items="countries"
              item-text="name"
              item-value="id"
              :change="applyFilters()"
              label="Country"
              clearable
              solo
              hide-details
              class="my-3"
            ></v-autocomplete>
            <v-autocomplete
              v-model="cityId"
              :items="cities"
              item-text="name"
              item-value="id"
              :change="applyFilters()"
              label="City"
              clearable
              solo
              hide-details
            ></v-autocomplete>
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
    };
  },
  computed: {
    categories() {
      return this.$store.getters["events/getCategories"];
    },
    countries() {
      return this.$store.getters["events/getCountries"];
    },
    cities() {
      return this.$store.getters["events/getCities"];
    },
    filtersActivated() {
      return this.showFilters;
    },
    filtersApplied() {
      return this.categoryId != null || this.countryId != null || this.cityId != null;
    }
  },
  methods: {
    activateFilters() {
        this.showFilters = !this.showFilters;
    },
    applyFilters() {
        const filterData = {
            categoryId: this.categoryId,
            countryId: this.countryId,
            cityId: this.cityId
        }
        debugger;
        this.$store.dispatch("events/applyFilters", filterData);
    }
  }
};
</script>
