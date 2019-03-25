export default function node(options = {}) {
  return context => prevConfig => Object.assign(prevConfig, {
    node: Object.assign(prevConfig.node || {}, options),
  });
}
