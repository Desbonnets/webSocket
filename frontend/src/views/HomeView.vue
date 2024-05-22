<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import websocket from '../websocket';

const username = ref<string| null>(null)
const connection = ref<WebSocket | undefined>(undefined)
const router = useRouter()

const connectWebSocket = () => {
  console.log("Connexion au websocket")
  if(websocket.methods === undefined){
    console.error("error: websocket")
  }else{
    connection.value = websocket.methods.setUrl("ws://localhost:6001/ws?name=" + username.value)

    connection.value.onopen = function(event) {
      console.log(event)
      console.log("Connexion établie")
      console.log(connection.value)
      // Redirection après la connexion établie
      router.push({ path: 'conversation' })
    }
  }
}

const handleUsername = () => {
  if(username.value !== null && username.value != ''){
    connectWebSocket()
  }
}

</script>

<template>
  <div id="app" v-if="connection === undefined">
    <p>
      <label for="message">Username</label>
      <input type="text" name="message" id="message" v-model="username">
    </p>
    <button @click="handleUsername()">Rejoindre la conversation</button>
  </div>
</template>
