<script setup lang="ts">
import { ref } from 'vue'
import { RouterLink, RouterView } from 'vue-router'

const connection = ref<WebSocket | null>(null)

const sendMessage = (message: string) => {
  console.log(connection.value)
  if (connection.value) {
    connection.value.send(message)
  }
}

const connectWebSocket = () => {
  console.log("Connexion au websocket")
  connection.value = new WebSocket("ws://localhost:6001/ws?name=Gilles");


  connection.value.onopen = function(event) {
    console.log(event)
    console.log("Connexion établie")
  }

  connection.value.onmessage = function(event) {
    console.log(event)
  }
}

connectWebSocket()

const textMessage = ref(null)

const handleMessage = () => {
  if(textMessage.value !== null && textMessage.value != ''){
    sendMessage(textMessage.value);
  }
}

</script>

<template>
  <div id="app">
      
    <p>
      <label for="message">Message</label>
      <input type="text" name="message" id="message" v-model="textMessage">
    </p>
    <button @click="handleMessage()">Send Message</button>
    <!-- <input>
    <button @click="sendMessage('hello World')">Send Message</button> -->
  </div>
  <!-- <header>

    <div class="wrapper">
      <nav>
        <RouterLink to="/">Home</RouterLink>
      </nav>
    </div>
  </header>

  <RouterView /> -->
</template>

<style scoped>
header {
  line-height: 1.5;
  max-height: 100vh;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }
}
</style>
