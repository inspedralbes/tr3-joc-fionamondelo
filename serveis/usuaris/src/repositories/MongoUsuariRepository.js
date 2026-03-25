const IRepository = require('./IRepository');
const Usuari = require('../models/Usuari');

class MongoUsuariRepository extends IRepository {
    async findById(id) {
        return await Usuari.findById(id);
    }
    async findAll() {
        return await Usuari.find();
    }
    async create(data) {
        const usuari = new Usuari(data);
        return await usuari.save();
    }
    async update(id, data) {
        return await Usuari.findByIdAndUpdate(id, data, { new: true });
    }

    async delete(id) {
        return await Usuari.findByIdAndDelete(id);
    }
    async findByNomUsuari(nomUsuari) {
        return await Usuari.findOne({ nomUsuari: nomUsuari });
    }
}

module.exports = MongoUsuariRepository;
