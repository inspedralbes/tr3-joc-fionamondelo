const mongoose = require('mongoose');

const ResultatSchema = new mongoose.Schema({
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

module.exports = mongoose.model('Resultat', ResultatSchema);
