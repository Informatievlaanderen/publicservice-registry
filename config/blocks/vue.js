/**
 * VueJS webpack block
 *
 * @see https://github.com/vuejs/vue-loader
 */

import ExtractTextWebpackPlugin from 'extract-text-webpack-plugin';

const CSS_EXTS = ['css', 'scss', 'less', 'sass', 'stylus'];

function preHook(context) {
  const types = context.fileType.all();
  if ('application/x-vue' in types) {
    return;
  }
  context.fileType.add('application/x-vue', /\.vue$/);
}

function extract(loader) {
  return ExtractTextWebpackPlugin.extract({
    use: loader,
    fallback: 'vue-style-loader',
  });
}

export default function vue(options) {
  const {
    exclude = /\/node_modules\//,
    extractCSS = false,
  } = options || {};

  const opts = ((x) => {
    Object.keys(options || {})
      .filter(k => !['exlude', 'extractCSS'].includes(k))
      .forEach(k => x[k] = options[k]);
    return x;
  })({});

  const extraOptions = () => {
    opts.loaders = opts.loaders || {};
    const foundCSS = CSS_EXTS.filter(e => e in opts.loaders);

    if (foundCSS.length) {
      foundCSS.forEach((ext) => {
        if (ext in opts.loaders) {
          const loader = opts.loaders[ext];
          if (extractCSS) {
            opts.loaders[ext] = extract(loader);
          } else {
            opts.loaders[ext] = `vue-style-loader!${opts.loaders[ext]}`;
          }
        }
      });
    }

    return opts;
  };

  const vueBlock = (context, { addLoader }) => addLoader({
    test: context.fileType('application/x-vue'),
    exclude: Array.isArray(exclude) ? exclude : [exclude],
    loader: 'vue-loader',
    options: extraOptions(),
  });

  return Object.assign(vueBlock, {
    pre: preHook,
  });
}
