import { subDays } from "date-fns";

const randomMakes = [
  "BMW",
  "Audi",
  "Ford",
  "Vauxhall",
  "Kia",
  "Nissan",
  "Volkswagen",
  "Alfa Romeo",
  "Hyundai",
  "Abarth",
  "Citroen",
  "Dacia",
  "DS",
  "Fiat",
  "Honda",
  "Infiniti",
  "Jaguar",
  "Jeep",
  "Land Rover",
  "Lexus",
  "Mazda",
  "Mercedes-Benz",
  "MG Motor UK",
  "Mini",
  "Mitsubishi",
  "Peugeot",
  "Porsche",
  "Renault",
  "Seat",
  "Skoda",
  "Smart",
  "Suzuki",
  "Tesla",
  "Toyota",
  "Volvo",
];
const randomNumbers = [2342, 4324, 5531, 1133, 6323, 8232];
const randomIndex = Math.floor(Math.random() * randomNumbers.length);

const GenerateData = (numberOfDays) => {
  const numDays = Math.abs(numberOfDays);
  const randomData = [];
  for (let num = numDays; num >= 0; num--) {
    let mockDataPoint = randomMakes[num];

    if (!mockDataPoint) {
      mockDataPoint = "Other";
    }

    randomData.push({
      dataCategory: "Text Queries",
      dataPoint: mockDataPoint,
      count: 1 + Math.random() * randomNumbers[randomIndex],
      date: subDays(new Date(), num).toISOString().substring(0, 10),
    });
  }

  return randomData;
};

export default GenerateData;
