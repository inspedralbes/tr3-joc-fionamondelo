async function registrarUsuari(repo, nomUsuari, alias, contrasenya) {
    
    if (!nomUsuari || !alias || !contrasenya) {
        throw new Error('El nom usuari, alias i la contrasenya són obligatoris');
    }

    return await repo.create({
        nomUsuari: nomUsuari,
        alias: alias,
        contrasenya: contrasenya
    });
}

async function loginUsuari(repo, nomUsuari, contrasenya) {
    
    const usuari = await repo.findByNomUsuari(nomUsuari);

    if (!usuari) {
        throw new Error('L\'usuari no existeix');
    }
    const esCorrecta = await usuari.compararContrasenya(contrasenya);

    if (!esCorrecta) {
        throw new Error('La contrasenya és incorrecta');
    }

    return usuari;
}

async function getUsuari(repo, id) {
    const usuari = await repo.findById(id);

    if (!usuari) {
        throw new Error('No ni ha cap usuari amb aquesta ID');
    }

    return usuari;
}

module.exports = {
    registrarUsuari: registrarUsuari,
    loginUsuari: loginUsuari,
    getUsuari: getUsuari
};
