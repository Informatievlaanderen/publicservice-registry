import fileSaver from 'file-saver';

export default {
  export(services) {
    const file = new Blob([services], {
      type: 'text/plain;charset=utf-8',
    });

    fileSaver.saveAs(file, 'dienstverleningen.csv');
  },
};
