const express = require('express');
const router = express.Router();
const MongoUsuariRepository = require('../repositories/MongoUsuariRepository');
const UsuariService = require('../services/UsuariService');
const repo = new MongoUsuariRepository();


router.post('/registrar', async function (req, res) {
    try {
        const nomUsuari = req.body.nomUsuari;
        const alias = req.body.alias;
        const contrasenya = req.body.contrasenya;

        const usuariCreat = await UsuariService.registrarUsuari(repo, nomUsuari, alias, contrasenya);

        res.status(201).json(usuariCreat);
    } catch (error) {
        res.status(400).json({ error: error.message });
    }
});

router.post('/login', async function (req, res) {
    try {
        const nomUsuari = req.body.nomUsuari;
        const contrasenya = req.body.contrasenya;

        const usuari = await UsuariService.loginUsuari(repo, nomUsuari, contrasenya);

        res.status(200).json(usuari);
    } catch (error) {
        res.status(401).json({ error: error.message });
    }
});

router.get('/:id', async function (req, res) {
    try {
        const id = req.params.id;
        const usuari = await UsuariService.getUsuari(repo, id);

        res.status(200).json(usuari);
    } catch (error) {
        res.status(404).json({ error: error.message });
    }
});

module.exports = router;
