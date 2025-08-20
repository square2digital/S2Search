const { genericAPI } = require('../shared/genericAPI');

export default async function handler(req, res) {
  return await genericAPI(req, res, 'documentCount', '', true);
}
