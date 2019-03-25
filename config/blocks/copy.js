import CopyWebpackPlugin from 'copy-webpack-plugin';

export default function copy(from, to) {
  const plugins = [
    new CopyWebpackPlugin([
      {
        from,
        to,
        ignore: ['.*'],
      },
    ]),
  ];

  return (context, util) => util.merge({ plugins });
}
