/* eslint-disable object-shorthand */
import path from 'path';
import fs from 'fs';
import packageConfig from '../package.json';

const projectRoot = path.resolve(__dirname, '../src/PublicServiceRegistry.UI');
const wwwRoot = path.join(projectRoot, 'wwwroot');

export const isProduction = process.env.NODE_ENV === 'production';

export const devPort = process.env.PORT || 443;

export default {
  projectName: packageConfig.name,
  projectRoot: projectRoot,
  indexTemplate: path.join(projectRoot, 'index.html'),
  entryPoint: {
    publicService: path.join(projectRoot, 'app', 'main.js'),
  },

  output: path.join(wwwRoot, '[name].[hash].js'),
  cssOutputName: '[name].[contenthash:20].css',

  wwwRoot: wwwRoot,
  index: isProduction ? path.join(wwwRoot, 'index.html') : 'index.html',
  assetsDirectory: 'static',

  host: 'dienstverlening-test.basisregisters.vlaanderen',
  allowedHosts: [
    '.basisregisters.vlaanderen'
  ],
  https: {
    pfx: fs.readFileSync(path.join(projectRoot, 'dienstverlening-test.basisregisters.vlaanderen.pfx')),
    passphrase: 'dienstverlening!'
  },

  // Remember to also update jsconfig.json
  aliases: {
    vue: 'vue/dist/vue.esm.js',
    '@': path.join(projectRoot, 'app'),
    app: path.join(projectRoot, 'app'),
    components: path.join(projectRoot, 'app/components'),
    pages: path.join(projectRoot, 'app/pages'),
    services: path.join(projectRoot, 'app/services'),
    store: path.join(projectRoot, 'app/store'),
    strings: path.join(projectRoot, 'app/strings'),
  },
};
