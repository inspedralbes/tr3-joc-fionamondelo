const IRepository = require('./IRepository');

class InMemoryGameRepository extends IRepository {
    constructor() {
        super();
        this.partides = [];
    }
    async findById(id) {
        return this.partides.find(p => p.id === id || p._id === id);
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
        const index = this.partides.findIndex(p => p.id === id || p._id === id);
        if (index !== -1) {
            this.partides[index] = { ...this.partides[index], ...data };
            return this.partides[index];
        }
        return null;
    }
    async delete(id) {
        const index = this.partides.findIndex(p => p.id === id || p._id === id);
        if (index !== -1) {
            const eliminada = this.partides.splice(index, 1);
            return eliminada[0];
        }
        return null;
    }
}

module.exports = InMemoryGameRepository;
