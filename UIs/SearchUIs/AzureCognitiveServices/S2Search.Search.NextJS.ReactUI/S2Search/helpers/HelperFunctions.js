const RemoveDuplicates = existingVehicles => {
  existingVehicles = existingVehicles.filter(
    (vehicle, index, self) =>
      index === self.findIndex(v => v.vehicleID === vehicle.vehicleID)
  );

  return existingVehicles;
};

export default { RemoveDuplicates };
