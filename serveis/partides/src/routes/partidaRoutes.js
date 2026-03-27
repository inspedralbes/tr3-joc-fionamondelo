const express = require('express');
const router = express.Router();
const MongoPartidaRepository = require('../repositories/MongoPartidaRepository');
const PartidaService = require('../services/PartidaService');
const repo = new MongoPartidaRepository();


router.post('/crear', async function (req, res) {
    try {
        const partida = await PartidaService.crearPartida(repo);
        res.status(201).json(partida);
    } catch (error) {
        console.log('error:', error.message);
        res.status(400).json({ error: error.message });
    }
});

router.post('/unirse', async function (req, res) {
    try {
        const codiSala = req.body.codiSala;
        const usuariId = req.body.usuariId;

        const partida = await PartidaService.unirsePartida(repo, codiSala, usuariId);
        res.status(200).json(partida);
    } catch (error) {
        res.status(400).json({ error: error.message });
    }
});

router.get('/:codiSala', async function (req, res) {
    try {
        const codiSala = req.params.codiSala;
        const partida = await PartidaService.getPartida(repo, codiSala);
        res.status(200).json(partida);
    } catch (error) {
        res.status(404).json({ error: error.message });
    }
});

router.post('/finalitzar', async function (req, res) {
    try {
        const codiSala = req.body.codiSala;
        const guanyadorId = req.body.guanyadorId;

        const partida = await PartidaService.finalitzarPartida(repo, codiSala, guanyadorId);
        res.status(200).json(partida);
    } catch (error) {
        res.status(400).json({ error: error.message });
    }
});

module.exports = router;
