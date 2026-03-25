const mongoose = require('mongoose');
const bcrypt = require('bcrypt');

// squema per a l'usuari
const usuariSchema = new mongoose.Schema({
    nomUsuari: {
        type: String,
        required: true,
        unique: true
    },
    alias: {
        type: String,
        required: true
    },
    contrasenya: {
        type: String,
        required: true
    },
    creat: {
        type: Date,
        default: Date.now
    }
});

usuariSchema.pre('save', async function (next) {
    const usuari = this;
    if (!usuari.isModified('contrasenya')) return next();

    try {
        const hash = await bcrypt.hash(usuari.contrasenya, 10);
        usuari.contrasenya = hash;
        next();
    } catch (error) {
        return next(error);
    }
});

usuariSchema.methods.compararContrasenya = async function (contrasenyaCandidata) {
    try {
        return await bcrypt.compare(contrasenyaCandidata, this.contrasenya);
    } catch (error) {
        return false;
    }
};

module.exports = mongoose.model('Usuari', usuariSchema);
