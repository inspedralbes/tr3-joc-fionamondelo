const mongoose = require('mongoose');

const PartidaSchema = new mongoose.Schema({
    codiSala: {
        type: String,
        unique: true,
        required: true
    },
    estat: {
        type: String,
        enum: ['esperant', 'jugant', 'finalitzada'],
        default: 'esperant'
    },
    jugadors: [{
        type: mongoose.Schema.Types.ObjectId,
        ref: 'Usuari'
    }],
    guanyador: {
        type: mongoose.Schema.Types.ObjectId,
        ref: 'Usuari'
    },

    creat: {
        type: Date,
        default: Date.now
    }
});

module.exports = mongoose.model('Partida', PartidaSchema);
