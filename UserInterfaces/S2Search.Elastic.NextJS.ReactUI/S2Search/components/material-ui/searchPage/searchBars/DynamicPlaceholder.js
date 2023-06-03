import { useState, useEffect } from "react";
import { getValuesFromConfigArray } from "../../../../common/functions/ConfigFunctions";

const startString = "Search for:";
const FiveSpaces = "     ";
let defaultPlaceholders = [
  `Ford Red 2019...${FiveSpaces}`,
  `Black suv...${FiveSpaces}`,
  `Convertible Blue...${FiveSpaces}`,
  `Honda Civic grey...${FiveSpaces}`,
  `Porsche Silver...${FiveSpaces}`,
];

const DynamicPlaceholder = (apiPlaceholders) => {
  const [placeholder, setPlaceholder] = useState("");
  const [placeholderIndex, setPlaceholderIndex] = useState(0);
  const [arrayIndex, setArrayIndex] = useState(0);

  useEffect(() => {
    let quote = defaultPlaceholders[arrayIndex];
    const timer = setInterval(() => {
      for (const char of quote.split("")) {
        setPlaceholder(`${startString} ${quote.slice(char, placeholderIndex)}`);
        if (placeholderIndex + 1 > quote.length) {
          setPlaceholderIndex(0);
        } else {
          setPlaceholderIndex(placeholderIndex + 1);
        }
      }
    }, 100);

    if (placeholder === `${startString} ${quote}`) {
      if (arrayIndex + 1 === defaultPlaceholders.length) {
        setArrayIndex(0);
      } else {
        setArrayIndex(arrayIndex + 1);
      }
    }

    return () => {
      clearInterval(timer);
    };
  }, [placeholderIndex, arrayIndex, placeholder]);

  if (apiPlaceholders) {
    defaultPlaceholders = getValuesFromConfigArray(apiPlaceholders);
  }

  return placeholder;
};

export default DynamicPlaceholder;
