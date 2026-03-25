const IRepository = require('./IRepository');

// El repositori en memòria torna a ser una classe
class InMemoryGameRepository extends IRepository {
    constructor() {
        super();
        this.partides = [];
    }

    async findById(id) {
        return this.partides.find(function (p) {
            return p.id === id || p._id === id;
        });
    }

    async findAll() {
        return this.partides;
    }

    async create(data) {
        const novaPartida = { ...data, _id: Date.now().toString() };
        this.partides.push(novaPartida);
        return novaPartida;
    }

    async update(id, data) {
        const index = this.partides.findIndex(function (p) {
            return p.id === id || p._id === id;
        });
        if (index !== -1) {
            this.partides[index] = { ...this.partides[index], ...data };
            return this.partides[index];
        }
        return null;
    }

    async delete(id) {
        const index = this.partides.findIndex(function (p) {
            return p.id === id || p._id === id;
        });
        if (index !== -1) {
            return this.partides.splice(index, 1)[0];
        }
        return null;
    }
}

module.exports = InMemoryGameRepository;
