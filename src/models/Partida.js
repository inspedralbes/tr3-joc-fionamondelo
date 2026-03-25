const mongoose = require('mongoose');

// esquema per a la partida
const partidaSchema = new mongoose.Schema({
    codiSala: {
        type: String,
        required: true,
        unique: true
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
        ref: 'Usuari',
        required: false
    },
    creat: {
        type: Date,
        default: Date.now
    }
});

module.exports = mongoose.model('Partida', partidaSchema);
