<template>
  <div id="app">
    <img alt="Vue logo" src="./assets/logo.png">
    <button v-on:click="sendMessage('hello World')">Send Message</button>
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

<script setup lang="ts">
import { ref } from 'vue'

const connection = ref<WebSocket | null>(null)

const sendMessage = (message: string) => {
  console.log(connection.value)
  if (connection.value) {
    connection.value.send(message)
  }
}

const connectWebSocket = () => {
  console.log("Connexion au websocket")
  connection.value = new WebSocket("ws://localhost:6969/ws");


  connection.value.onopen = function(event) {
    console.log(event)
    console.log("Connexion établie")
  }

  connection.value.onmessage = function(event) {
    console.log(event)
  }
}

connectWebSocket()
</script>

<!-- <script setup lang="ts">
// export default {
//     name: 'App',
//     data: function(){
//         return {
//         connection: null
//         },
//         methods: {
//         sendMessage: const = (message: any) => {
//             console.log(this.connection);
//             this.connection.send(message);
//         }
//         }
//         created: const = () => {
//         console.log("connection au websocket")
//         this.connection = new WebSocket("wss://echo.websocket.org")

//         this.connection.onopen = function(event: any) {
//             console.log(event)
//             console.log("connection ok")
//         }

//         this.connection.onmessage = function(event: any) {
//             console.log(event)
//         }
//         }
//     }
// }
// import { RouterLink, RouterView } from 'vue-router'

</script> -->

<style scoped>
/* header {
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
} */
</style>
