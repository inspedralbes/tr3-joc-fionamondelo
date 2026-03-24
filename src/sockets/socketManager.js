module.exports = (io) => {
    const players = {};

    io.on('connection', (socket) => {
        console.log(`Nuevo jugador conectado: ${socket.id}`);

        // Cuando un jugador se une al juego
        socket.on('playerJoin', (data) => {
            // Guardamos la información del jugador
            players[socket.id] = {
                id: socket.id,
                name: data.name || 'Invitado',
                x: data.x || 0,
                y: data.y || 0,
                color: data.color || '#FFFFFF'
            };

            // Notificamos a todos (incluyendo al nuevo) sobre el estado actual
            io.emit('currentPlayers', players);

            // Notificamos a los demás que alguien se ha unido
            socket.broadcast.emit('playerJoined', players[socket.id]);
        });

        // Cuando un jugador se mueve
        socket.on('move', (data) => {
            if (players[socket.id]) {
                players[socket.id].x = data.x;
                players[socket.id].y = data.y;

                // Broadcast de la nueva posición
                socket.broadcast.emit('playerMoved', {
                    id: socket.id,
                    x: data.x,
                    y: data.y
                });
            }
        });

        // Cuando se pone una bomba
        socket.on('placeBomb', (data) => {
            // Reenviamos el evento de bomba a todos
            io.emit('bombPlaced', {
                id: socket.id,
                x: data.x,
                y: data.y,
                timer: data.timer || 3000
            });
        });

        // Desconexión
        socket.on('disconnect', () => {
            console.log(`Jugador desconectado: ${socket.id}`);
            delete players[socket.id];
            io.emit('playerDisconnected', socket.id);
        });
    });
};
