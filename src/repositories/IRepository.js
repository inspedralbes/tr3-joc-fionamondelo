const IRepository = {
    findById: async (id) => {
        throw new Error('Mètode findById no implementat');
    },
    findAll: async () => {
        throw new Error('Mètode findAll no implementat');
    },
    create: async (data) => {
        throw new Error('Mètode create no implementat');
    },
    update: async (id, data) => {
        throw new Error('Mètode update no implementat');
    },
    delete: async (id) => {
        throw new Error('Mètode delete no implementat');
    }
};

module.exports = IRepository;
