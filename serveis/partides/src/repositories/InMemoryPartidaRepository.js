const IRepository = require('./IRepository');

class InMemoryPartidaRepository extends IRepository {
    constructor() {
        super();
        this.data = [];
        this.nextId = 1;
    }

    async findById(id) {
        return this.data.find(p => p.id === id) || null;
    }

    async findAll() {
        return [...this.data];
    }

    async create(partidaData) {
        const newPartida = {
            ...partidaData,
            id: this.nextId++,
            creat: partidaData.creat || new Date(),
            estat: partidaData.estat || 'esperant',
            jugadors: partidaData.jugadors || []
        };
        this.data.push(newPartida);
        return newPartida;
    }

    async update(id, partidaData) {
        const index = this.data.findIndex(p => p.id === id);
        if (index !== -1) {
            this.data[index] = { ...this.data[index], ...partidaData };
            return this.data[index];
        }
        return null;
    }

    async delete(id) {
        const index = this.data.findIndex(p => p.id === id);
        if (index !== -1) {
            const [deletedPartida] = this.data.splice(index, 1);
            return deletedPartida;
        }
        return null;
    }

    async findByCodiSala(codiSala) {
        return this.data.find(p => p.codiSala === codiSala) || null;
    }
}

module.exports = InMemoryPartidaRepository;
