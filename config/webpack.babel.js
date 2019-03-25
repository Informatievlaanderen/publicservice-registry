import webpack from 'webpack';
import path from 'path';
import autoprefixer from 'autoprefixer';
import EslintFriendlyFormatter from 'eslint-friendly-formatter';
import fs from 'fs';

import {
  createConfig,
  entryPoint,
  setOutput,
  addPlugins,
  defineConstants,
  sourceMaps,
  resolve,
  env,
  match,
} from '@webpack-blocks/webpack';

import {
  file,
  url,
} from '@webpack-blocks/assets';

import config, { isProduction, devPort } from './config';
import checkVersions from './check-versions';

import devServer from './blocks/dev-server';
import babel from './blocks/babel';
import eslint from './blocks/eslint';
import uglify from './blocks/uglify';
import scss from './blocks/scss';
import optimiseCSS from './blocks/optimise-css';
import clean from './blocks/clean';
import copy from './blocks/copy';
import vue from './blocks/vue';
import node from './blocks/node';
import progressbar from './blocks/progressbar';
import extractText from './blocks/extract-text';
import htmlWebpack from './blocks/html-webpack';
import friendlyErrors from './blocks/friendly-errors';

const environment = isProduction ? 'production' : 'development';

checkVersions();

export default createConfig([
  defineConstants({ 'process.env.NODE_ENV': environment }),
  entryPoint(config.entryPoint),
  progressbar(),
  friendlyErrors(config.projectName, !isProduction),
  clean(config.wwwRoot),
  clean(path.join(config.wwwRoot, config.assetsDirectory)),
  htmlWebpack(config.indexTemplate, config.index),
  scss({ minimize: isProduction, sourceMap: true }),

  match(['*.eot', '*.ttf', '*.woff', '*.woff2'], [file()]),
  match(['*.gif', '*.jpg', '*.jpeg', '*.png', '*.svg', '*.webp'], [url({ limit: 10000 })]),

  resolve({
    extensions: ['.vue', '.js', 'json'],
    alias: config.aliases,
  }),

  babel(),

  vue({
    esModule: true,
    postcss: [autoprefixer()],
    extractCSS: isProduction,
    loaders: {
      css: 'css-loader?sourceMap',
      scss: 'css-loader?sourceMap!sass-loader?sourceMap',
      sass: 'css-loader?sourceMap!sass-loader?indentedSyntax&sourceMap',
    },
    transformToRequire: {
      video: 'src',
      source: 'src',
      img: 'src',
      image: 'xlink:href',
    },
  }),

  node({
    // prevent webpack from injecting useless setImmediate polyfill because Vue
    // source contains it (although only uses it if it's native).
    setImmediate: false,
    // prevent webpack from injecting mocks to Node native modules
    // that does not make sense for the client
    dgram: 'empty',
    fs: 'empty',
    net: 'empty',
    tls: 'empty',
    child_process: 'empty',
  }),

  eslint({ formatter: EslintFriendlyFormatter, emitWarning: !isProduction }),
  sourceMaps('source-map'),

  copy(path.join(config.projectRoot, config.assetsDirectory), config.assetsDirectory),

  env('development', [
    copy(path.join(config.projectRoot, 'config.js'), config.assetsDirectory),
    devServer(
      {
        overlay: true,
        port: devPort,
        https: config.https,
        host: config.host,
        allowedHosts: config.allowedHosts,
      }),
  ]),

  env('production', [
    setOutput(config.output),
    uglify({ sourceMap: true }),
    extractText(config.cssOutputName),
    optimiseCSS({ sourceMap: true }),

    addPlugins([
      new webpack.LoaderOptionsPlugin({
        minimize: true,
        debug: false,
      }),

      // keep module.id stable when vender modules does not change
      new webpack.HashedModuleIdsPlugin(),

      // split vendor js into its own file
      new webpack.optimize.CommonsChunkPlugin({
        name: 'vendor',
        minChunks: module => (
          module.resource &&
          /\.js$/.test(module.resource) &&
          module.resource.indexOf(path.join(__dirname, '../node_modules')) === 0
        ),
      }),

      // extract webpack runtime and module manifest to its own file in order to
      // prevent vendor hash from being updated whenever app bundle is updated
      new webpack.optimize.CommonsChunkPlugin({
        name: 'manifest',
        chunks: ['vendor'],
      }),
    ]),
  ]),
]);
