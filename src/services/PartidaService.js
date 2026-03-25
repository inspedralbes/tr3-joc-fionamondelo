function generarCodiSala() {
    const caracters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    let resultat = '';
    for (let i = 0; i < 6; i++) {
        resultat += caracters.charAt(Math.floor(Math.random() * caracters.length));
    }
    return resultat;
}

async function crearPartida(repo) {
    const codiSala = generarCodiSala();
    return await repo.create({ codiSala: codiSala });
}

async function unirsePartida(repo, codiSala, usuariId) {
    const partida = await repo.findByRoomCode(codiSala);
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
    return await repo.update(partida._id, { jugadors: partida.jugadors });
}

async function obtenirPartida(repo, codiSala) {
    const partida = await repo.findByRoomCode(codiSala);
    if (!partida) {
        throw new Error("La partida no existeix");
    }
    return partida;
}

async function finalitzarPartida(repo, codiSala, guanyadorId) {
    const partida = await repo.findByRoomCode(codiSala);
    if (!partida) {
        throw new Error("La partida no existeix");
    }

    return await repo.update(partida._id, {
        estat: 'finalitzada',
        guanyador: guanyadorId
    });
}

module.exports = {
    crearPartida,
    unirsePartida,
    obtenirPartida,
    finalitzarPartida
};
