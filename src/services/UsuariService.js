function crearUserService(userRepository) {
    return {
        registreUsuari: async function (nomUsuari, alias, contrasenya) {
            if (!nomUsuari || nomUsuari.trim() === "") {
                throw new Error("El nom d'usuari no pot estar buit");
            }
            if (!alias || alias.trim() === "") {
                throw new Error("L'àlies no pot estar buit");
            }
            if (!contrasenya || contrasenya.trim() === "") {
                throw new Error("La contrasenya no pot estar buida");
            }
            return await userRepository.create({ nomUsuari, alias, contrasenya });
        },
        obtenirUsuari: async function (id) {
            if (!id) {
                throw new Error("L'ID de l'usuari és necessari");
            }
            return await userRepository.findById(id);
        }
    };
}

module.exports = crearUserService;
