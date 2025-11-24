using Marqdouj.DotNet.AzureMaps.Map.GeoJson;

namespace UnitTests
{
    [TestClass]
    public sealed class PositionTests
    {
        [TestMethod]
        public void Position_Constructor_Default()
        {
            //Arrange
            //NOTE: For Test Only - JS Interop Constructor
            var psn = new Position();

            //Act

            //Assert
            Assert.IsEmpty(psn);
            Assert.AreEqual(0, psn.Longitude);
            Assert.AreEqual(0, psn.Latitude);
            Assert.IsFalse(psn.HasElevation);
            Assert.IsNull(psn.Elevation);
            Assert.HasCount(2, psn);
        }

        [TestMethod]
        public void Position_Constructor_LonLat()
        {
            //Arrange
            var psn = new Position(1.1, 2.1);

            //Act

            //Assert
            Assert.IsFalse(psn.HasElevation);
            Assert.IsNull(psn.Elevation);
            Assert.HasCount(2, psn);
            Assert.AreEqual(1.1, psn.Longitude);
            Assert.AreEqual(2.1, psn.Latitude);
        }

        [TestMethod]
        public void Position_Constructor_LonLatElv()
        {
            //Arrange
            var psn = new Position(1.1, 2.1, 3.1);

            //Act

            //Assert
            Assert.IsTrue(psn.HasElevation);
            Assert.IsNotNull(psn.Elevation);
            Assert.HasCount(3, psn);
            Assert.AreEqual(1.1, psn.Longitude);
            Assert.AreEqual(2.1, psn.Latitude);
            Assert.AreEqual(3.1, psn.Elevation);
        }

        [TestMethod]
        public void Position_Constructor_List_LonLat()
        {
            //Arrange
            var psn = new Position([1.1, 2.1]);

            //Act

            //Assert
            Assert.IsFalse(psn.HasElevation);
            Assert.IsNull(psn.Elevation);
            Assert.HasCount(2, psn);
            Assert.AreEqual(1.1, psn.Longitude);
            Assert.AreEqual(2.1, psn.Latitude);
        }

        [TestMethod]
        public void Position_Constructor_List_LonLatElv()
        {
            //Arrange
            var psn = new Position([1.1, 2.1, 3.1]);

            //Act

            //Assert
            Assert.IsTrue(psn.HasElevation);
            Assert.IsNotNull(psn.Elevation);
            Assert.HasCount(3, psn);
            Assert.AreEqual(1.1, psn.Longitude);
            Assert.AreEqual(2.1, psn.Latitude);
            Assert.AreEqual(3.1, psn.Elevation);
        }

        [TestMethod]
        public void Position_Constructor_List_Empty()
        {
            //Arrange
            var psn = new Position([]);

            //Act
            
            //Assert
            Assert.IsFalse(psn.HasElevation);
            Assert.IsNull(psn.Elevation);
            Assert.IsEmpty(psn);
            Assert.AreEqual(0, psn.Longitude);
            Assert.AreEqual(0, psn.Latitude);
            Assert.HasCount(2, psn);
        }

        [TestMethod]
        public void Position_Constructor_List_Lon()
        {
            //Arrange
            var psn = new Position([1.1]);

            //Act

            //Assert
            Assert.IsFalse(psn.HasElevation);
            Assert.IsNull(psn.Elevation);
            Assert.HasCount(1, psn);
            Assert.AreEqual(1.1, psn.Longitude);
            Assert.AreEqual(0, psn.Latitude);
            Assert.HasCount(2, psn);
        }

        [TestMethod]
        public void Position_Constructor_List_LonLatElv_Extra()
        {
            //Arrange
            var psn = new Position([1.1, 2.1, 3.1, 4.1, 5.1]);

            //Act

            //Assert
            Assert.IsTrue(psn.HasElevation);
            Assert.IsNotNull(psn.Elevation);
            Assert.HasCount(3, psn);
            Assert.AreEqual(1.1, psn.Longitude);
            Assert.AreEqual(2.1, psn.Latitude);
            Assert.AreEqual(3.1, psn.Elevation);
        }
    }
}
