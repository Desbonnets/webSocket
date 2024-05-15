<script setup lang="ts">
import { ref } from 'vue'

const connection = this.$store.getters.websocket

const sendMessage = (message: string) => {
  console.log(connection.value)
  if (connection.value) {
    connection.value.send(message)
  }
}

// const connectWebSocket = () => {
//   console.log("Connexion au websocket")
//   connection.value = new WebSocket("ws://localhost:6001/ws?name=Gilles");


//   connection.value.onopen = function(event) {
//     console.log(event)
//     console.log("Connexion établie")
//   }

//   connection.value.onmessage = function(event) {
//     console.log(event)
//   }
// }

// connectWebSocket()

const textMessage = ref(null)

const handleMessage = () => {
  if(textMessage.value !== null && textMessage.value != ''){
    sendMessage(textMessage.value);
  }
}

</script>

<template>
  <div>
    <h2>Conversation: ??</h2>
    <div name="conversation_content">
      <div name="your_message"><p>Bonjour!</p></div>
      <div name="other_message"><p>Bonjour!</p></div>
    </div>
    <div>
      <div>
        <label for="message">Message</label>
        <input type="text" name="message" id="message" v-model="textMessage">
      </div>
      <button @click="handleMessage()">Send Message</button>
    </div>
  </div>
</template>

<style>
@media (min-width: 1024px) {
  .about {
    min-height: 100vh;
    display: flex;
    align-items: center;
  }
}
</style>
