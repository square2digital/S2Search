import { DefaultPlaceholderText } from '../Constants';

export const getConfigValueByKey = (config, key) => {
  for (const item of config) {
    if (item.key === key) {
      return item;
    }
  }
};

export const getPlaceholdersArray = configData => {
  if (!Array.isArray(configData)) {
    console.warn(
      'getPlaceholdersArray: configData is not an array, returning default'
    );
    return DefaultPlaceholderText;
  }

  const arr = [];

  configData.forEach(element => {
    if (element && element.key && element.key.includes('PlaceholderText')) {
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
  if (!Array.isArray(configData)) {
    console.warn(
      'getValuesFromConfigArray: configData is not an array, returning empty array'
    );
    return [];
  }

  const arr = [];

  configData.forEach(element => {
    if (element && element.value !== undefined) {
      arr.push(element.value);
    }
  });

  return arr;
};

export const getKeysFromConfigArray = configData => {
  if (!Array.isArray(configData)) {
    console.warn(
      'getKeysFromConfigArray: configData is not an array, returning empty array'
    );
    return [];
  }

  const arr = [];

  configData.forEach(element => {
    if (element && element.key !== undefined) {
      arr.push(element.key);
    }
  });

  return arr;
};
