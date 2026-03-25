function crearGameService(gameRepository) {

    function generarCodiSala() {
        const caracters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
        let resultat = '';
        for (let i = 0; i < 6; i++) {
            resultat += caracters.charAt(Math.floor(Math.random() * caracters.length));
        }
        return resultat;
    }

    return {

        crearPartida: async function () {
            const codiSala = generarCodiSala();
            return await gameRepository.create({ codiSala: codiSala });
        },

        unirSePartida: async function (codiSala, usuariId) {
            const partida = await gameRepository.findByRoomCode(codiSala);
            if (!partida) {
                throw new Error("La partida no existeix");
            }
            if (partida.estat !== 'esperant') {
                throw new Error("La partida no està en espera");
            }
            if (partida.jugadors.length >= 2) {
                throw new Error("La sala està plena");
            }

            partida.jugadors.push(usuariId);
            return await gameRepository.update(partida._id, { jugadors: partida.jugadors });
        },

        finalitzarPartida: async function (codiSala, guanyadorId) {
            const partida = await gameRepository.findByRoomCode(codiSala);
            if (!partida) {
                throw new Error("La partida no existeix");
            }

            return await gameRepository.update(partida._id, {
                estat: 'finalitzada',
                guanyador: guanyadorId
            });
        },
        
        obtenirPartida: async function (codiSala) {
            const partida = await gameRepository.findByRoomCode(codiSala);
            if (!partida) {
                throw new Error("La partida no existeix");
            }
            return partida;
        }
    };
}

module.exports = crearGameService;
