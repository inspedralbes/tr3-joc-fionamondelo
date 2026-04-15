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
            const rawData = data.toString();
            const missatge = JSON.parse(rawData);

            if (missatge.tipus === 'unir_sala') {
                const codiSala = missatge.codiSala;

                if (!sales.has(codiSala)) {
                    sales.set(codiSala, new Set());
                }

                sales.get(codiSala).add(ws);
                ws.codiSala = codiSala;

                const numClients = sales.get(codiSala).size;
                console.log('Client unit a la sala: ' + codiSala + ' (total clients: ' + numClients + ')');
                return;
            }

            if (ws.codiSala && sales.has(ws.codiSala)) {
                const tipusPermesos = ['moure', 'posar_bomba', 'explosio', 'jugador_mort', 'fi_partida', 'spawn_item'];
                if (tipusPermesos.includes(missatge.tipus)) {
                    const clientsEnSala = sales.get(ws.codiSala);
                    let enviats = 0;

                    clientsEnSala.forEach(function (client) {
                        if (client !== ws && client.readyState === 1) {
                            client.send(JSON.stringify(missatge));
                            enviats++;
                        }
                    });

                    if (missatge.tipus !== 'moure') {
                        console.log('Missatge [' + missatge.tipus + '] reenviat a ' + enviats + ' client(s) a sala ' + ws.codiSala);
                    }
                } else {
                    console.log('Tipus de missatge no permès: ' + missatge.tipus);
                }
            } else {
                console.log('Client sense sala envia missatge: ' + missatge.tipus);
            }

        } catch (error) {
            console.error('Error processant el missatge:', error, '| Raw data:', data.toString());
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
