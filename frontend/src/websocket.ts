import { defineComponent } from "vue";

export default defineComponent({
  data() {
    return {
      websocket: new WebSocket('ws://localhost:6001/ws'),
    };
  },
  methods: {
    setUrl(url: string): WebSocket {
      return this.websocket = new WebSocket(url);
    },
    getWebsocket(): WebSocket {
        return this.websocket;
    }
    
  },
});