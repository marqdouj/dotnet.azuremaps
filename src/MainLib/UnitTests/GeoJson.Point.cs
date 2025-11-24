using Marqdouj.DotNet.AzureMaps.Map.GeoJson;

namespace UnitTests
{
    [TestClass]
    public sealed class PointTests
    {
        [TestMethod]
        public void Point_Constructor_Default()
        {
            //Arrange
            var psn = new Point();

            //Act

            //Assert
            Assert.IsFalse(psn.Coordinates.HasElevation);
            Assert.IsNull(psn.Coordinates.Elevation);
            Assert.HasCount(2, psn.Coordinates);
            Assert.AreEqual(0, psn.Coordinates.Longitude);
            Assert.AreEqual(0, psn.Coordinates.Latitude);
        }

        [TestMethod]
        public void Point_Constructor_Position()
        {
            //Arrange
            var psn = new Point([]);

            //Act

            //Assert
            Assert.IsFalse(psn.Coordinates.HasElevation);
            Assert.IsNull(psn.Coordinates.Elevation);
            Assert.HasCount(2, psn.Coordinates);
            Assert.AreEqual(0, psn.Coordinates.Longitude);
            Assert.AreEqual(0, psn.Coordinates.Latitude);
        }

        [TestMethod]
        public void Point_Coordinates()
        {
            //Arrange
            var psn = new Point
            {
                //Act
                Coordinates = []
            };

            //Assert
            Assert.IsFalse(psn.Coordinates.HasElevation);
            Assert.IsNull(psn.Coordinates.Elevation);
            Assert.HasCount(2, psn.Coordinates);
            Assert.AreEqual(0, psn.Coordinates.Longitude);
            Assert.AreEqual(0, psn.Coordinates.Latitude);
        }

        [TestMethod]
        public void Point_Coordinates_Null()
        {
            //Arrange
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var psn = new Point
            {
                //Act
                Coordinates = null //Disabled warning; need to test this scenario.
            };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            //Assert
            Assert.IsFalse(psn?.Coordinates?.HasElevation);
            Assert.IsNull(psn?.Coordinates?.Elevation);
            Assert.HasCount(2, psn!.Coordinates!);
            Assert.AreEqual(0, psn!.Coordinates!.Longitude);
            Assert.AreEqual(0, psn!.Coordinates.Latitude);
        }
    }
}
