import { DefaultPlaceholderText } from '../Constants';

export const getConfigValueByKey = (config, key) => {
  for (const item of config) {
    if (item.key === key) {
      return item;
    }
  }
};

export const getPlaceholdersArray = configData => {
  const arr = [];

  configData.forEach(element => {
    if (element.key.includes('PlaceholderText')) {
      if (element) {
        arr.push(element);
      }
    }
  });

  if (arr.length === 0) {
    return DefaultPlaceholderText;
  }

  return arr;
};

export const getValuesFromConfigArray = configData => {
  const arr = [];

  configData.forEach(element => {
    arr.push(element.value);
  });

  return arr;
};

export const getKeysFromConfigArray = configData => {
  const arr = [];

  configData.forEach(element => {
    arr.push(element.key);
  });

  return arr;
};
