<script setup lang="ts">
import { ref } from 'vue'
import { useRouter, RouterView } from 'vue-router'
import { useStore } from 'vuex/types/index.js';

const router = useRouter()
const store = useStore()

const connection = store.state.websocket
const username = ref<string| null>(null);

const handleUsername = () => {
  if(username.value !== null && username.value != ''){
    store.dispatch('connectWebSocket', username.value)
  }
}

</script>

<template>
  <div id="app" v-if="connection === null">
    <p>
      <label for="message">Username</label>
      <input type="text" name="message" id="message" v-model="username">
    </p>
    <button @click="handleUsername()">Rejoindre la conversation</button>
  </div>
  <RouterView />
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
