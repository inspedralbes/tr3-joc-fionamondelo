require('dotenv').config();

const express = require('express');
const cors = require('cors');
const mongoose = require('mongoose');
const partidaRoutes = require('./src/routes/partidaRoutes');

const app = express();

app.use(cors());
app.use(express.json());

const mongoUri = process.env.MONGO_URI;

mongoose.connect(mongoUri)
    .then(function () {
        console.log('Connectat correctament a MongoDB (Partides)');
    })
    .catch(function (error) {
        console.error('Error en la connexió a MongoDB:', error);
    });

app.get('/health', function (req, res) {
    res.status(200).json({
        status: 'ok',
        servei: 'partides'
    });
});

app.use('/api/partides', partidaRoutes);

const port = process.env.PORT || 3002;

app.listen(port, function () {
    console.log('Partides funcionant al port ' + port);
});
