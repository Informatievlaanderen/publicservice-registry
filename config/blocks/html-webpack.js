import HtmlWebpackPlugin from 'html-webpack-plugin';

export default function htmlWebpack(template, filename) {
  const plugins = [
    new HtmlWebpackPlugin({
      template,
      filename,
      inject: true,
      minify: {
        removeComments: true,
        collapseWhitespace: true,
        removeAttributeQuotes: true,
      },
      // necessary to consistently work with multiple chunks via CommonsChunkPlugin
      chunksSortMode: 'dependency',
    }),
  ];

  return (context, util) => util.merge({ plugins });
}
