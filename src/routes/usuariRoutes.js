const express = require('express');
const router = express.Router();
const MongoUsuariRepository = require('../repositories/MongoUsuariRepository');
const UsuariService = require('../services/UsuariService');
const repo = new MongoUsuariRepository();


router.post('/registrar', async function (req, res) {
    try {
        const { nomUsuari, alias, contrasenya } = req.body;
        const nouUsuari = await UsuariService.registreUsuari(repo, nomUsuari, alias, contrasenya);
        res.status(201).json(nouUsuari);
    } catch (error) {
        res.status(400).json({ error: error.message });
    }
});

router.post('/login', async function (req, res) {
    try {
        const { nomUsuari, contrasenya } = req.body;
        const usuari = await UsuariService.loginUsuari(repo, nomUsuari, contrasenya);
        res.json(usuari);
    } catch (error) {
        res.status(401).json({ error: error.message });
    }
});

router.get('/:id', async function (req, res) {
    try {
        const usuari = await UsuariService.getUsuari(repo, req.params.id);
        res.json(usuari);
    } catch (error) {
        res.status(404).json({ error: error.message });
    }
});

module.exports = router;
