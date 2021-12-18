<template>
  <v-dialog ref="dialog" v-model="modal" :return-value.sync="time" persistent width="290px">
    <template v-slot:activator="{ on, attrs }">
      <v-text-field v-model="time" :rules="timeRules" label="Сhoose time" append-icon="mdi-clock-time-four-outline" color="teal" readonly v-bind="attrs" v-on="on"></v-text-field>
    </template>
    <v-time-picker v-if="modal" v-model="time"  format="24hr" color="teal" full-width>
      <v-spacer></v-spacer>
      <v-btn text color="teal" @click="modal = false"> Cancel </v-btn>
      <v-btn text color="teal" @click="saveTime()"> OK </v-btn>
    </v-time-picker>
  </v-dialog>
</template>

<script>
export default {};
</script>

<script>
export default {
  name: 'PopupTimePicker',
  props: {
    timeOfTheEvent: {
      type: String,
      default: ''
    },
  },
  data() {
    return {
      time: null,
      timeRules: [
        v => !!v || 'Time is required'
      ],
      modal: false,
    };
  },
  created() {
    if (this.timeOfTheEvent !== '') {
      this.time = this.timeOfTheEvent;
    }
  },
  methods: {
      saveTime() {
        this.$refs.dialog.save(this.time);
        this.$emit('bindTime', this.time);
      }
  }
};
</script>
