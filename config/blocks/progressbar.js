import chalk from 'chalk';
import ProgressBarPlugin from 'progress-bar-webpack-plugin';

export default function progressbar() {
  const plugins = [
    new ProgressBarPlugin({
      format: `${chalk.cyan.bold('  build ')}${chalk.yellow.bold('[:bar]')}${chalk.green.bold(' :percent')} (:elapsed seconds)`,
      clear: false,
    }),
  ];

  return (context, util) => util.merge({ plugins });
}
