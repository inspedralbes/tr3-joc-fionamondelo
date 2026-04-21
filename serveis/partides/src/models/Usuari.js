const mongoose = require('mongoose');

const UsuariSchema = new mongoose.Schema({
    nomUsuari: {
        type: String,
        required: true
    },
    alias: {
        type: String
    }
});

module.exports = mongoose.model('Usuari', UsuariSchema, 'usuaris');
