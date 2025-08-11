import { useState, useEffect, useMemo } from 'react';
import { getValuesFromConfigArray } from '../../../../common/functions/ConfigFunctions';

const startString = 'Search for:';
const FiveSpaces = '     ';
const fallbackPlaceholders = [
  `Ford Red 2019...${FiveSpaces}`,
  `Black suv...${FiveSpaces}`,
  `Convertible Blue...${FiveSpaces}`,
  `Honda Civic grey...${FiveSpaces}`,
  `Porsche Silver...${FiveSpaces}`,
];

const DynamicPlaceholder = apiPlaceholders => {
  const [placeholder, setPlaceholder] = useState('');
  const [placeholderIndex, setPlaceholderIndex] = useState(0);
  const [arrayIndex, setArrayIndex] = useState(0);

  // Memoize the placeholders array to prevent unnecessary re-renders
  const placeholders = useMemo(() => {
    if (
      apiPlaceholders &&
      Array.isArray(apiPlaceholders) &&
      apiPlaceholders.length > 0
    ) {
      const apiValues = getValuesFromConfigArray(apiPlaceholders);
      return apiValues.length > 0 ? apiValues : fallbackPlaceholders;
    }
    return fallbackPlaceholders;
  }, [apiPlaceholders]);

  useEffect(() => {
    // Ensure we have valid placeholders and arrayIndex is within bounds
    if (!placeholders || placeholders.length === 0) {
      setPlaceholder(`${startString} `);
      return;
    }

    const safeArrayIndex = arrayIndex % placeholders.length;
    const quote = placeholders[safeArrayIndex];

    // Ensure quote is a string
    if (!quote || typeof quote !== 'string') {
      setPlaceholder(`${startString} `);
      return;
    }

    const timer = setInterval(() => {
      for (const char of quote.split('')) {
        setPlaceholder(`${startString} ${quote.slice(0, placeholderIndex)}`);
        if (placeholderIndex + 1 > quote.length) {
          setPlaceholderIndex(0);
        } else {
          setPlaceholderIndex(placeholderIndex + 1);
        }
      }
    }, 100);

    if (placeholder === `${startString} ${quote}`) {
      if (arrayIndex + 1 >= placeholders.length) {
        setArrayIndex(0);
      } else {
        setArrayIndex(arrayIndex + 1);
      }
    }

    return () => {
      clearInterval(timer);
    };
  }, [placeholderIndex, arrayIndex, placeholder, placeholders]);

  return placeholder;
};

export default DynamicPlaceholder;
