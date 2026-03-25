const IRepository = require('./IRepository');
const Partida = require('../models/Partida');

class MongoGameRepository extends IRepository {
    async findById(id) {
        return await Partida.findById(id);
    }
    async findByRoomCode(codiSala) {
        return await Partida.findOne({ codiSala: codiSala });
    }
    async findAll() {
        return await Partida.find();
    }
    async create(data) {
        return await Partida.create(data);
    }
    async update(id, data) {
        return await Partida.findByIdAndUpdate(id, data, { new: true });
    }
    async delete(id) {
        return await Partida.findByIdAndDelete(id);
    }
}

module.exports = MongoGameRepository;
