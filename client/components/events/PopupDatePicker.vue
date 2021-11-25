<template>
  <v-dialog ref="dialog" v-model="modal" :return-value.sync="date" persistent width="290px">
    <template v-slot:activator="{ on, attrs }">
      <v-text-field v-model="date" :rules="dateRules" label="Choose date" append-icon="mdi-calendar" color="teal" readonly v-bind="attrs" v-on="on"></v-text-field>
    </template>
    <v-date-picker v-model="date" :min="minDate" color="teal" scrollable>
      <v-spacer></v-spacer>
      <v-btn text color="teal" @click="modal = false"> Cancel </v-btn>
      <v-btn text color="teal" @click="saveDate()"> OK </v-btn>
    </v-date-picker>
  </v-dialog>
</template>

<script>
export default {
  name: "PopupDatePicker",
  data: () => ({
    date: null,
    dateRules: [
      v => !!v || 'Date is required'
    ],

    modal: false
  }),
  computed: {
    minDate() {
      return (new Date(Date.now() - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10);
    }
  },
  methods: {
    saveDate() {
      this.$refs.dialog.save(this.date);
      this.$emit('bindDate', this.date);
    }
  }
}
</script>

<style>
</style>
