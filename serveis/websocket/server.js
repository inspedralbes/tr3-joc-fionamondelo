require('dotenv').config({ path: '../../.env' });
const { WebSocketServer } = require('ws');
const port = process.env.PORT || 3003;
const wss = new WebSocketServer({ port: port });

const sales = new Map();

console.log('Servidor WebSocket funcionant al port ' + port);

wss.on('connection', function (ws) {
    console.log('Nou client connectat');

    ws.on('message', function (data) {
        try {
            const missatge = JSON.parse(data);

            if (missatge.tipus === 'unir_sala') {
                const codiSala = missatge.codiSala;

                if (!sales.has(codiSala)) {
                    sales.set(codiSala, new Set());
                }

                sales.get(codiSala).add(ws);
                ws.codiSala = codiSala;

                console.log('Client unit a la sala: ' + codiSala);
                return;
            }

            if (ws.codiSala && sales.has(ws.codiSala)) {
                const tipusPermesos = ['moure', 'posar_bomba', 'explosio', 'jugador_mort', 'fi_partida'];

                if (tipusPermesos.includes(missatge.tipus)) {
                    const clientsEnSala = sales.get(ws.codiSala);

                    clientsEnSala.forEach(function (client) {
                        if (client !== ws && client.readyState === 1) {
                            client.send(JSON.stringify(missatge));
                        }
                    });
                }
            }

        } catch (error) {
            console.error('Error processant el missatge:', error);
        }
    });

    ws.on('close', function () {
        console.log('Client desconnectat');
        if (ws.codiSala && sales.has(ws.codiSala)) {
            const clientsEnSala = sales.get(ws.codiSala);
            clientsEnSala.delete(ws);

            if (clientsEnSala.size === 0) {
                sales.delete(ws.codiSala);
                console.log('Sala ' + ws.codiSala + ' eliminada perquè està buida');
            }
        }
    });

    ws.on('error', function (error) {
        console.error('Error en el WebSocket:', error);
    });
});
