<script>
import axios from 'axios';
import { ref } from 'vue';

const error= ref<String| null>(null)
const name= ref<String| null>(null)
const date= ref<Date| null>(null)
  
const  addConversation = async() => {
  try {
    const response = await axios.post('https://your-api-url/Conversation', {
      Name: name.value,
      Date: date.value,
    });
    name.value = '';
    date.value = '';

    alert('Conversation added successfully!');
  } catch (e) {
    console.error('Error adding conversation:', e);
    error.value = 'Failed to add conversation.';
  }
}

</script>

<template>
  <div>
    <h1>Ajouter une nouvelle conversation</h1>
    <p v-if="error">{{error}}</p>
    <form @submit.prevent="addConversation">
      <div>
        <label for="name">Nom:</label>
        <input type="text" v-model="name" id="name" required />
      </div>
      <div>
        <label for="date">Date:</label>
        <input type="date" v-model="date" id="date" required />
      </div>
      <button type="submit">Valider</button>
    </form>
  </div>
</template>

<style scoped>
h1 {
  font-weight: 500;
  font-size: 2.6rem;
  position: relative;
  top: -10px;
}

h3 {
  font-size: 1.2rem;
}

.greetings h1,
.greetings h3 {
  text-align: center;
}

@media (min-width: 1024px) {
  .greetings h1,
  .greetings h3 {
    text-align: left;
  }
}
</style> 
