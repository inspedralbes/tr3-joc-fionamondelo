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
        throw new Error('No s\'ha trobat cap partida amb el codi: ' + codiSala);
    }

    if (partida.estat !== 'esperant') {
        throw new Error('La partida ja ha començat o ha finalitzat');
    }

    if (partida.jugadors.length >= 2) {
        throw new Error('La sala està plena (màxim 2 jugadors)');
    }

    if (partida.jugadors.includes(usuariId)) {
        throw new Error('L\'usuari ja és a la partida');
    }

    const nousJugadors = [...partida.jugadors, usuariId];

    return await repo.update(partida._id || partida.id, {
        jugadors: nousJugadors
    });
}


async function finalitzarPartida(repo, codiSala, guanyadorId) {
    const partida = await repo.findByCodiSala(codiSala);

    if (!partida) {
        throw new Error('No s\'ha trobat cap partida amb el codi: ' + codiSala);
    }

    return await repo.update(partida._id || partida.id, {
        estat: 'finalitzada',
        guanyador: guanyadorId
    });
}

async function getPartida(repo, codiSala) {
    const partida = await repo.findByCodiSala(codiSala);

    if (!partida) {
        throw new Error('No s\'ha trobat cap partida amb el codi: ' + codiSala);
    }

    return partida;
}

module.exports = {
    crearPartida: crearPartida,
    unirsePartida: unirsePartida,
    finalitzarPartida: finalitzarPartida,
    getPartida: getPartida
};
