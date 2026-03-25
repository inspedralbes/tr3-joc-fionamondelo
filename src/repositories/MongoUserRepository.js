const IRepository = require('./IRepository');
const Usuari = require('../models/Usuari');

class MongoUserRepository extends IRepository {
    async findById(id) {
        return await Usuari.findById(id);
    }
    async findAll() {
        return await Usuari.find();
    }
    async create(data) {
        return await Usuari.create(data);
    }
    async update(id, data) {
        return await Usuari.findByIdAndUpdate(id, data, { new: true });
    }
    async delete(id) {
        return await Usuari.findByIdAndDelete(id);
    }
}

module.exports = MongoUserRepository;
