module.exports = function (res, message, statusCode) {
  res.headers = {
    'Content-Type': 'application/json',
  };
  res.body = `${message}`;
  res.status = statusCode;
  res.isRaw = true;
};
