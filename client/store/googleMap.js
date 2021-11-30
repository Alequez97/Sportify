import { Loader } from '@googlemaps/js-api-loader';

export const state = () => ({
    google: undefined,
    geocoder: undefined
});

export const mutations = {
    SET_GOOGLE_OBJECT(state, value) {
        state.google = value;
    },
    SET_GEOCODER_OBJECT(state, value) {
        state.geocoder = value;
    }
}

export const actions = {
    async prepare({commit, state}) {
        if (state.google === undefined) {
            try {
                const loader = new Loader({
                    apiKey: "AIzaSyD_u7kDh3P6m58MutrCTCDsF2Oiy-jPMf0"
                });
                await loader
                  .load()
                  .then((google) => {
                    commit('SET_GOOGLE_OBJECT', google);
                    const geocoder = new google.maps.Geocoder();
                    commit('SET_GEOCODER_OBJECT', geocoder);
                  });
            } catch (e){
                console.log(e);
            }
        }
    },
    getGeolocationFromAddressAsync({state}, address) {
        return new Promise((resolve, reject) => {
          state.geocoder
            .geocode({ address })
            .then((response) => {
              const result = response.results[0];
              if (result) {
                const location = result.geometry.location;
                const latlng = { lat: location.lat(), lng: location.lng() };
                resolve(latlng)
              } else {
                // TODO: Implement logic if address not found
              }
            })
            .catch(e => reject(e));
        });
    },
    getAddressFromGeolocationAsync({state}, latLng) {
        return new Promise((resolve, reject) => {
          state.geocoder
            .geocode({ location: latLng })
            .then((response) => {
              const result = response.results[0];
              if (result) {
                const addressComponents = result.address_components;
                const fullAddress = {
                  country: addressComponents.filter(c => c.types.includes("country"))[0]?.long_name,
                  city: addressComponents.filter(c => c.types.includes("locality"))[0]?.long_name,
                  district: addressComponents.filter(c => c.types.includes("sublocality"))[0]?.long_name,
                  street: addressComponents.filter(c => c.types.includes("route"))[0]?.long_name,
                  houseNumber: addressComponents.filter(c => c.types.includes("street_number"))[0]?.long_name
                };
                resolve(fullAddress);
              } else {
                const fullAddress = {
                  country: null,
                  city: null,
                  district: null,
                  street: null,
                  houseNumber: null
                };
                resolve(fullAddress);
              }
            })
            .catch(e => reject(e));
        });
    },
    getCurrentLocationAsync() {
        return new Promise((resolve) => {
          if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition((position) => {
              const currentPosition = { lat: position.coords.latitude, lng: position.coords.longitude };
              resolve(currentPosition);
            });
          } else {
            // Implement other logic if geolocation is disabled
            const currentPositionLat = 56.81543608008677;
            const currentPositionLng = 24.599863529284583;
            const currentPosition = { lat: currentPositionLat, lng: currentPositionLng };
            resolve(currentPosition);
          }
        });
    }
}

export const getters = {
    getGoogleObject: state => state.google,
    getGeocoderObject: state => state.geocoder
}
