const { genericAPI } = require('../shared/genericAPI');

export default async function handler(req, res, cancellation) {
  return await genericAPI(req, res, 'documentCount', '', true, cancellation);
}
