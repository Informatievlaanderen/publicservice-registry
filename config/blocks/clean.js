import CleanWebpackPlugin from 'clean-webpack-plugin';

import config from './../config';

export default function clean(pathsToClean) {
  let paths = [];

  if (typeof pathsToClean === 'string') {
    paths = [pathsToClean];
  } else if (Array.isArray(pathsToClean)) {
    paths = pathsToClean;
  }

  const plugins = [
    new CleanWebpackPlugin(paths, {
      root: config.projectRoot,
      verbose: false,
    }),
  ];

  return (context, util) => util.merge({ plugins });
}
