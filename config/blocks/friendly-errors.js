import chalk from 'chalk';
import path from 'path';
import notifier from 'node-notifier';
import FriendlyErrorsWebpackPlugin from 'friendly-errors-webpack-plugin';

const createNotifierCallback = name => (severity, errors) => {
  if (severity !== 'error') return;

  const error = errors[0];
  const filename = error.file && error.file.split('!').pop();

  notifier.notify({
    title: name,
    message: `${severity} : ${error.name}`,
    subtitle: filename || '',
    icon: path.join(__dirname, 'logo.png'),
  });
};

export default function friendlyErrors(projectName, notifyOnErrors) {
  const plugins = [
    new FriendlyErrorsWebpackPlugin({
      compilationSuccessInfo: {
        messages: [chalk.cyan('Build complete.\n')],
        notes: [chalk.yellow(
          'Tip: built files are meant to be served over an HTTP server.\n' +
          '    Opening index.html over file:// won\'t work.\n\n')],
      },
      onErrors: notifyOnErrors
        ? createNotifierCallback(projectName)
        : undefined,
    }),
  ];

  return (context, util) => util.merge({ plugins });
}
