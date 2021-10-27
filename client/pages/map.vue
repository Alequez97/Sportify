<template>
    <div>
        <GoogleMap />
        <div class="centered-div">
            <div id="add-new-location-container">
                <button id="add-new-location-button" onclick="addNewLocationMarker()">Add new location</button>
            </div>
            <div id="cancel-new-location-container" style="position:relative; bottom:130px;">
                <RoundedButton
                  id="save-new-location-button"
                  text="Save"
                  @btn-click="methodName()"
                  color="#00FF00"
                />
                <RoundedButton
                  id="cancel-new-location-button"
                  text="Cancel"
                  color="#FF0000"
                />
            </div>
        </div>
    </div>
</template>

<script>
import GoogleMap from "../components/GoogleMap";
import RoundedButton from "../components/buttons/RoundedButton";

export default {
  components: {
    GoogleMap,
    RoundedButton
  },
  methods: {
    methodName() {
      console.log("method name test");
    }
  }
}
</script>

<style>
    .centered-div {
        margin: 0 auto;
        display: table;
    }

    #cancel-new-location-container {
        display: none;
    }
</style>
