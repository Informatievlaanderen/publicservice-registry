/**
 * UglifyJS webpack block.
 *
 * @see https://github.com/webpack-contrib/uglifyjs-webpack-plugin
 */

// tslint:disable-next-line:variable-name
import UglifyJSPlugin from 'uglifyjs-webpack-plugin';

/**
 * @param {object} [options] UglifyJS options
 * @return {Function}
 */
export default function uglify(options = {}) {
  const mergedOptions = Object.assign({
    parallel: true,
    cache: true,
    uglifyOptions: {
      compress: { warnings: false },
    },
  }, options);

  const postHook = (context, util) => {
    const plugin = new UglifyJSPlugin(mergedOptions);
    return util.addPlugin(plugin);
  };

  return Object.assign(
    () => prevConfig => prevConfig,
    { post: postHook },
  );
}
