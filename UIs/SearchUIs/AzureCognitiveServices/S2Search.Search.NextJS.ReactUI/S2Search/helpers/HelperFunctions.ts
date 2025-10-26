interface Vehicle {
  vehicleID: string;
  [key: string]: any;
}

const RemoveDuplicates = (existingVehicles: Vehicle[]): Vehicle[] => {
  existingVehicles = existingVehicles.filter(
    (vehicle, index, self) =>
      index === self.findIndex(v => v.vehicleID === vehicle.vehicleID)
  );

  return existingVehicles;
};

const HelperFunctions = { RemoveDuplicates };

export default HelperFunctions;