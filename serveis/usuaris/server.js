require('dotenv').config();

const express = require('express');
const cors = require('cors');
const mongoose = require('mongoose');
const usuariRoutes = require('./src/routes/usuariRoutes');
const app = express();

app.use(cors());
app.use(express.json());


const mongoUri = process.env.MONGO_URI;

mongoose.connect(mongoUri)
    .then(function () {
        console.log('Connectat correctament a MongoDB');
    })
    .catch(function (error) {
        console.error('Error en la connexió a MongoDB:', error);
    });


app.get('/health', function (req, res) {
    res.status(200).json({
        status: 'ok',
        servei: 'usuaris'
    });
});

app.use('/api/usuaris', usuariRoutes);

const port = process.env.PORT || 3001;

app.listen(port, function () {
    console.log('El servei d\'usuaris està funcionant al port ' + port);
});
