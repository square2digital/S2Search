import http from "k6/http";
import { sleep } from "k6";

import {
  jUnit,
  textSummary,
} from "https://jslib.k6.io/k6-summary/0.0.1/index.js";

const makesArr = [
  "Alfa Romeo",
  "Aston Martin",
  "Audi",
  "Bentley",
  "BMW",
  "Alpina",
  "Caterham",
  "Citroen",
  "Daihatsu",
  "Dodge",
  "Ferrari ",
  "Ferrari",
  "Fiat",
  "Ford",
  "Ford ",
  "Honda",
  "Hyundai",
  "Infiniti",
  "Jaguar",
  "Jaguar ",
  "Jeep",
  "Kia  ",
  "Kia",
  "Lamborghini",
  "Lancia",
  "Land Rover",
  "Lexus",
  "Lexus ",
  "Lotus",
  "Maserati",
  "Mercedes-Benz",
  "Mini ",
  "Mitsubishi",
  "Nissan",
  "Peugeot",
  "Porsche",
  "Porsche ",
  "Saab",
  "Seat",
  "Subaru",
  "Suzuki",
  "Toyota",
  "Vauxhall",
  "Volkswagen",
  "Volvo",
  "Austin",
  "Renault",
  "Mg",
  "Fiat ",
  "Saab ",
  "Toyota ",
  "Audi ",
  "Honda ",
  "Bentley ",
];

export const options = {
  vus: 1000,
  duration: "100s",
};

function getRandomInt(minimum, maximum) {
  var randomnumber =
    Math.floor(Math.random() * (maximum - minimum + 1)) + minimum;

  return randomnumber;
}

function getMake() {
  const arrCount = makesArr.length;

  const index = getRandomInt(0, arrCount);

  return makesArr[index];
}

function getEndpoint() {
  const apiBaseUrl = "https://saxton.s2search.co.uk";
  const callingHost = "saxton.s2search.co.uk";
  let ApiBaseUrl = `${apiBaseUrl}/api/search?searchTerm=${getMake()}&filters=&orderBy=&pageNumber=${getRandomInt(
    0,
    5
  )}&pageSize=${getRandomInt(0, 24)}&numberOfExistingResults=${getRandomInt(
    0,
    5
  )}&callingHost=${callingHost}`;

  return ApiBaseUrl;
}

export default function () {
  let i = 0;

  while (i < 100) {
    http.get(`${getEndpoint()}`);
    sleep(getRandomInt(0, 20));
    i++;
  }

  /*   http.get(`${ApiBaseUrl}`);
  sleep(getRandomInt(0, 100)); */
}

// export function handleSummary(data) {
//   const filepath = `./k6-result.xml`;
//   return {
//     stdout: textSummary(data, { indent: " ", enableColors: true }),
//     "./loadtest-results.xml": jUnit(data),
//   };
// }
