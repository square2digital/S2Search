const { genericAPI } = require("../shared/genericAPI");
module.exports = async function (req, res, cancellation) {
  return await genericAPI(
    req,
    res,
    "autoSuggest",
    "autoSuggest",
    true,
    cancellation
  );
};
