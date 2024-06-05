<script setup lang="ts">
  import { ref } from 'vue'
  import websocket from '../websocket'
  import { useRouter } from 'vue-router'

  const router = useRouter()

  const connection = ref<WebSocket | undefined>(undefined)
  let message = ref<Array<string>>([])
  connection.value = websocket.methods?.getWebsocket();

  if(connection.value === undefined){
    router.push({ path: '/' })
  }else{
    connection.value.onmessage = function(event) {
      console.log(event.data)
      message.value.push(event.data)
    }
  }
  const sendMessage = (message: string) => {
    if (connection.value) {
      connection.value.send(message)
    }
  }

  const textMessage = ref(null)

  const handleMessage = () => {
    if(textMessage.value !== null && textMessage.value != ''){
      sendMessage(textMessage.value);
      textMessage.value = null;
    }
  }

</script>

<template>
  <div>
    <h2>Conversation: ??</h2>
    <div name="conversation_content">
      <div name="your_message"><p>Bonjour!</p></div>
      <div name="other_message" v-for="item in message"><p>{{ item }}</p></div>
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
