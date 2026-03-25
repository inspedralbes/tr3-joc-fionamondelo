const IRepository = require('./IRepository');
const Partida = require('../models/Partida');

class MongoPartidaRepository extends IRepository {
    async findById(id) {
        return await Partida.findById(id);
    }
    async findAll() {
        return await Partida.find();
    }
    async create(data) {
        const partida = new Partida(data);
        return await partida.save();
    }
    async update(id, data) {
        return await Partida.findByIdAndUpdate(id, data, { new: true });
    }
    async delete(id) {
        return await Partida.findByIdAndDelete(id);
    }
    async findByCodiSala(codiSala) {
        return await Partida.findOne({ codiSala: codiSala });
    }
}

module.exports = MongoPartidaRepository;
