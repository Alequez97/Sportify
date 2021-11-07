<template>
  <div class="roundedButtonContainer">
    <button :id="id" :style="{color:color}" @click="onClick()">{{ text }}</button>
  </div>
</template>

<script>
export default {
  name: "RoundedButton",
  props: ["id", "text", "jsFunctionOnClick", "color"],
  methods: {
      onClick() {
          this.$emit('btn-click')
      }
  }
};
</script>

<style scoped>
.roundedButtonConteiner {
  margin-top: 100px;
  text-align: center;
}

button {
  display: inline-block;

  font: normal normal 300 1.3em "Open Sans";
  text-decoration: none;

  background-color: transparent;
  border: 1px solid rgba(15,41,18,0.8);
  border-radius: 100px;

  padding: 0.3em 1.2em;
  margin: 5px;

  background-size: 200% 100%;
  background-image: linear-gradient(
    to right,
    transparent 50%,
    rgba(15,41,18,0.8) 50%
  );
  transition: background-position 0.3s cubic-bezier(0.19, 1, 0.22, 1) 0.1s,
    color 0.5s ease 0s, background-color 0.5s ease;
}

button:hover {
  color: rgba(255, 255, 255, 1);
  background-color: rgba(15,41,18,0.8);
  background-position: -100% 100%;
}
</style>
