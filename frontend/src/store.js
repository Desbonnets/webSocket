// store.js
import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    websocket: null
  },
  mutations: {
    setWebSocket(state, websocket) {
      state.websocket = websocket
    }
  },
  actions: {
    connectWebSocket({ commit }, username) {
      console.log("Connexion au websocket")
      const websocket = new WebSocket("ws://localhost:6001/ws?name=" + username);

      websocket.onopen = function(event) {
        console.log(event)
        console.log("Connexion établie")
        // Redirection après la connexion établie
        // Vous devrez probablement accéder à router différemment ici
        router.push({ path: 'conversation' })
      }

      websocket.onmessage = function(event) {
        console.log(event)
      }

      // Enregistrer la connexion WebSocket dans le store
      commit('setWebSocket', websocket)
    }
  },
  getters: {
    websocket: state => state.websocket
  }
})
