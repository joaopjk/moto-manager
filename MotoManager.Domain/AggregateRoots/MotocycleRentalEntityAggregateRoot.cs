namespace MotoManager.Domain.AggregateRoots
{
    public static class MotocycleRentalEntityAggregateRoot
    {

        public static MotocycleRentalEntity CreateMotocycleRentalEntity(
            DateTime startDate,
            DateTime endDate,
            DateTime expectedEndDate,
            string idDeliveryPartner,
            string idMotocycle,
            int plan,
            decimal dailyRate,
            decimal totalRentValue)
        {
            return new MotocycleRentalEntity
            {
                Identifier = $"locacao{GenerateUniqueFourDigitNumber()}",
                StartDate = startDate,
                EndDate = endDate,
                ExpectedEndDate = expectedEndDate,
                IdDeliveryPartner = idDeliveryPartner,
                IdMotocycle = idMotocycle,
                Plan = plan,
                DailyRent = dailyRate,
                TotalRentValue = (decimal)totalRentValue
            };
        }

        /// <summary>
        /// Generates a unique four-digit number for rental identification purposes.
        /// </summary>
        /// <returns>A four-digit integer between 1000 and 9999.</returns>
        public static int GenerateUniqueFourDigitNumber()
        {
            var bytes = new byte[4];
            int value;
            do
            {
                RandomNumberGenerator.Fill(bytes);
                value = BitConverter.ToInt32(bytes, 0);
                value = Math.Abs(value % 9000) + 1000;
            } while (value is < 1000 or > 9999);
            return value;
        }
    }
}
