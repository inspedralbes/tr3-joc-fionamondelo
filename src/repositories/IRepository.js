class IRepository {
    async findById(id) {
        throw new Error('Mètode findById no implementat');
    }
    async findAll() {
        throw new Error('Mètode findAll no implementat');
    }

    async create(data) {
        throw new Error('Mètode create no implementat');
    }
    async update(id, data) {
        throw new Error('Mètode update no implementat');
    }
    async delete(id) {
        throw new Error('Mètode delete no implementat');
    }
}

module.exports = IRepository;
