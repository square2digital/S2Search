import { addDays } from "date-fns";

const DateRangeProvider = (numberOfDays) => {
  const today = new Date();
  const dateFrom = addDays(today, numberOfDays).toUTCString();
  const dateTo = today.toUTCString();

  return { dateFrom, dateTo };
};

export default DateRangeProvider;
