const express = require('express');
const http = require('http');
const { Server } = require('socket.io');
const cors = require('cors');
const socketManager = require('./sockets/socketManager');

const app = express();
const server = http.createServer(app);
const io = new Server(server, {
  cors: {
    origin: "*", // En un entorno real se restringiría
    methods: ["GET", "POST"]
  }
});

app.use(cors());
app.use(express.json());

// Ruta para UnityWebRequest (comprobar estado)
app.get('/status', (req, res) => {
  res.json({ status: 'online', players: io.engine.clientsCount });
});


socketManager(io);

const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
  console.log(`Servidor conectat port ${PORT}`);
});
