import OptimizeCssAssetsPlugin from 'optimize-css-assets-webpack-plugin';

export default function optimiseCSS(options = {}) {
  const sourceMap = Boolean(options.sourceMap);

  const plugins = [
    new OptimizeCssAssetsPlugin({
      cssProcessorOptions: {
        safe: true,
        discardComments: {
          removeAll: true,
        },
        map: sourceMap ? { inline: false } : false,
      },
      canPrint: true,
    }),
  ];

  return (context, util) => util.merge({ plugins });
}
