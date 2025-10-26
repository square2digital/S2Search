const { genericAPI } = require('../shared/genericAPI');

export default async function handler(req, res, cancellation) {
  return await genericAPI(req, res, 'facets', '', false, cancellation);
}
