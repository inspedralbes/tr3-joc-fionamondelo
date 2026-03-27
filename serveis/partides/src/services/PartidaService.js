function generarCodiSala() {
    const caracters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'; //caracteres aleatorios 
    let resultat = '';
    for (let i = 0; i < 6; i++) {
        const randomIndex = Math.floor(Math.random() * caracters.length);
        resultat += caracters.charAt(randomIndex);
    }
    return resultat;
}

async function crearPartida(repo) {
    const codiSala = generarCodiSala();
    const novaPartida = {
        codiSala: codiSala,
        estat: 'esperant',
        jugadors: []
    };
    return await repo.create(novaPartida);
}

async function unirsePartida(repo, codiSala, usuariId) {
    const partida = await repo.findByCodiSala(codiSala);

    if (!partida) {
        throw new Error('No existeix cap partida amb aquest codi');
    }

    if (partida.estat !== 'esperant') {
        throw new Error('No et pots unir si la partida està en espera');
    }

    if (partida.jugadors.length >= 1) {
        throw new Error('Màxim de jugadors )');
    }

    const nousJugadors = [...partida.jugadors, usuariId];

    return await repo.update(partida._id || partida.id, {
        jugadors: nousJugadors
    });
}

async function finalitzarPartida(repo, codiSala, guanyadorId) {
    const partida = await repo.findByCodiSala(codiSala);

    if (!partida) {
        throw new Error('No existeix cap partida amb aquest codi');
    }

    return await repo.update(partida._id || partida.id, {
        estat: 'finalitzada',
        guanyador: guanyadorId
    });
}

// Obté la informació d'una partida pel seu codi
async function getPartida(repo, codiSala) {
    const partida = await repo.findByCodiSala(codiSala);

    if (!partida) {
        throw new Error('No trobada la partida');
    }

    return partida;
}

module.exports = {
    crearPartida: crearPartida,
    unirsePartida: unirsePartida,
    finalitzarPartida: finalitzarPartida,
    getPartida: getPartida
};
