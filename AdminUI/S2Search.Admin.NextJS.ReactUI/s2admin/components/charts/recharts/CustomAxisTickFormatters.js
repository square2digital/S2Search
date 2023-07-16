import { format, parseISO, isValid } from "date-fns";

export const CustomAxisTickFormatter = (str) => {
  const isValidDate = isValid(new Date(str));

  if (!isValidDate) {
    return str;
  }

  const date = parseISO(str);
  if (date.getDate() % 1 === 0) {
    return format(date, "do MMM");
  }
  return "";
};

export const SimpleAxisTickFormatter = (number) => `${number}`;
