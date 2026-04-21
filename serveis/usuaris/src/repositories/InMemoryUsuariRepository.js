const IRepository = require('./IRepository');

class InMemoryUsuariRepository extends IRepository {
    constructor() {
        super();
        this.data = [];
        this.nextId = 1;
    }
    async findById(id) {
        return this.data.find(u => u.id === id) || null;
    }
    async findAll() {
        return [...this.data];
    }
    async create(userData) {
        const newUser = {
            ...userData,
            id: this.nextId++,
            creat: userData.creat || new Date()
        };
        this.data.push(newUser);
        return newUser;
    }
    async update(id, userData) {
        const index = this.data.findIndex(u => u.id === id);
        if (index !== -1) {
            this.data[index] = { ...this.data[index], ...userData };
            return this.data[index];
        }
        return null;
    }
    async delete(id) {
        const index = this.data.findIndex(u => u.id === id);
        if (index !== -1) {
            const [deletedUser] = this.data.splice(index, 1);
            return deletedUser;
        }
        return null;
    }
    async findByNomUsuari(nomUsuari) {
        return this.data.find(u => u.nomUsuari === nomUsuari) || null;
    }
}

module.exports = InMemoryUsuariRepository;
