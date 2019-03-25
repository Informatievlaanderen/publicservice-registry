const path = require('path');

module.exports = {
  rootDir: path.resolve(__dirname, '../../../'),
  moduleFileExtensions: [
    'js',
    'json',
    'vue',
  ],
  moduleNameMapper: {
    '^@/(.*)$': '<rootDir>/src/PublicServiceRegistry.UI/app/$1',
    '^components/(.*)$': '<rootDir>/src/PublicServiceRegistry.UI/app/components/$1',
    '^services/(.*)$': '<rootDir>/src/PublicServiceRegistry.UI/app/services/$1',
    '^store/(.*)$': '<rootDir>/src/PublicServiceRegistry.UI/app/store/$1',
    '^pages/(.*)$': '<rootDir>/src/PublicServiceRegistry.UI/app/pages/$1',
  },
  transform: {
    '^.+\\.js$': '<rootDir>/node_modules/babel-jest',
    '.*\\.(vue)$': '<rootDir>/node_modules/vue-jest',
  },
  testPathIgnorePatterns: [
    'ui\\e2e',
    'PublicServiceRegistry.UI.Tests',
  ],
  snapshotSerializers: ['<rootDir>/node_modules/jest-serializer-vue'],
  setupFiles: ['<rootDir>/test/ui/unit/setup'],
};
