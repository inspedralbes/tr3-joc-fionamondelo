const mongoose = require('mongoose');
const bcrypt = require('bcrypt');

const UsuariSchema = new mongoose.Schema({
    nomUsuari: {
        type: String,
        unique: true,
        required: true
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

UsuariSchema.pre('save', async function () {
    if (!this.isModified('contrasenya')) {
        return;
    }

    const hash = await bcrypt.hash(this.contrasenya, 10);
    this.contrasenya = hash;
});


UsuariSchema.methods.compararContrasenya = async function (contrasenyaCandidata) {
    return await bcrypt.compare(contrasenyaCandidata, this.contrasenya);
};

module.exports = mongoose.model('Usuari', UsuariSchema);
