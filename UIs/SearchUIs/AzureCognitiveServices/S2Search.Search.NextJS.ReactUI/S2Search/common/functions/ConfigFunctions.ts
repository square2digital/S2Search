import { DefaultPlaceholderText } from '../Constants';

// Interface for configuration items
interface ConfigItem {
  key: string;
  value: string | undefined;
}

export const getConfigValueByKey = (config: ConfigItem[], key: string): ConfigItem | undefined => {
  for (const item of config) {
    if (item.key === key) {
      return item;
    }
  }
  return undefined;
};

export const getPlaceholdersArray = (configData: ConfigItem[]): ConfigItem[] => {
  if (!Array.isArray(configData)) {
    console.warn(
      'getPlaceholdersArray: configData is not an array, returning default'
    );
    return DefaultPlaceholderText;
  }

  const arr: ConfigItem[] = [];

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

export const getValuesFromConfigArray = (configData: ConfigItem[]): string[] => {
  if (!Array.isArray(configData)) {
    console.warn(
      'getValuesFromConfigArray: configData is not an array, returning empty array'
    );
    return [];
  }

  const arr: string[] = [];

  configData.forEach(element => {
    if (element && element.value !== undefined && element.value !== null) {
      arr.push(element.value);
    }
  });

  return arr;
};

export const getKeysFromConfigArray = (configData: ConfigItem[]): string[] => {
  if (!Array.isArray(configData)) {
    console.warn(
      'getKeysFromConfigArray: configData is not an array, returning empty array'
    );
    return [];
  }

  const arr: string[] = [];

  configData.forEach(element => {
    if (element && element.key !== undefined) {
      arr.push(element.key);
    }
  });

  return arr;
};