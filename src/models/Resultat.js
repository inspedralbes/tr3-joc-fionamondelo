const mongoose = require('mongoose');

// esquema per al resultat partida
const resultatSchema = new mongoose.Schema({
    partidaId: {
        type: mongoose.Schema.Types.ObjectId,
        ref: 'Partida',
        required: true
    },
    usuariId: {
        type: mongoose.Schema.Types.ObjectId,
        ref: 'Usuari',
        required: true
    },
    puntuacio: {
        type: Number,
        default: 0
    },
    sobreviscut: {
        type: Boolean,
        required: true
    },
    duradaSegons: {
        type: Number,
        required: true
    }
});

module.exports = mongoose.model('Resultat', resultatSchema);
