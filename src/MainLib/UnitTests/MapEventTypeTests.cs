using Marqdouj.DotNet.AzureMaps.Map.Events;

namespace UnitTests
{
    [TestClass]
    public sealed class MapEventTypeTests
    {
        [TestMethod]
        public void MapEventType_MapEventType_Map() => TestValues(Enum.GetValues<MapEventTypeMap>());

        [TestMethod]
        public void MapEventType_MapEventType_DataSource() => TestValues(Enum.GetValues<MapEventTypeDataSource>());

        [TestMethod]
        public void MapEventType_MapEventType_Layer() => TestValues(Enum.GetValues<MapEventTypeLayer>());

        [TestMethod]
        public void MapEventType_MapEventType_HtmlMarker() => TestValues(Enum.GetValues<MapEventTypeHtmlMarker>());

        [TestMethod]
        public void MapEventType_MapEventType_Popup() => TestValues(Enum.GetValues<MapEventTypePopup>());

        [TestMethod]
        public void MapEventType_MapEventType_Shape() => TestValues(Enum.GetValues<MapEventTypeShape>());

        [TestMethod]
        public void MapEventType_MapEventType_StyleControl() => TestValues(Enum.GetValues<MapEventTypeStyleControl>());

        [TestMethod]
        public void MapEventType_MapEventType_Map_GetMapEventTypes() => TestEventTypeConversion<MapEventTypeMap>([.. MapEventTarget.Map.GetMapEventTypes()]);

        [TestMethod]
        public void MapEventType_MapEventType_DataSource_GetMapEventTypes() => TestEventTypeConversion<MapEventTypeDataSource>([.. MapEventTarget.DataSource.GetMapEventTypes()]);

        [TestMethod]
        public void MapEventType_MapEventType_Layer_GetMapEventTypes() => TestEventTypeConversion<MapEventTypeLayer>([.. MapEventTarget.Layer.GetMapEventTypes()]);

        [TestMethod]
        public void MapEventType_MapEventType_HtmlMarker_GetMapEventTypes() => TestEventTypeConversion<MapEventTypeHtmlMarker>([.. MapEventTarget.HtmlMarker.GetMapEventTypes()]);

        [TestMethod]
        public void MapEventType_MapEventType_Popup_GetMapEventTypes() => TestEventTypeConversion<MapEventTypePopup>([.. MapEventTarget.Popup.GetMapEventTypes()]);

        [TestMethod]
        public void MapEventType_MapEventType_Shape_GetMapEventTypes() => TestEventTypeConversion<MapEventTypeShape>([.. MapEventTarget.Shape.GetMapEventTypes()]);

        [TestMethod]
        public void MapEventType_MapEventType_StyleControl_GetMapEventTypes() => TestEventTypeConversion<MapEventTypeStyleControl>([.. MapEventTarget.StyleControl.GetMapEventTypes()]);

        private static void TestValues<T>(IEnumerable<T> values) where T : Enum
        {
            foreach (var value in values)
            {
                var cast = Enum.Parse<MapEventType>(value.ToString());

                Assert.IsTrue(Enum.IsDefined(typeof(MapEventType), cast.ToString()));
                Assert.IsTrue(Enum.IsDefined(typeof(T), cast.ToString()));
                Assert.AreEqual(value.ToString(), cast.ToString());
                Assert.AreEqual(Convert.ToInt32(value), (int)cast);
            }
        }

        private static void TestEventTypeConversion<T>(List<MapEventType> eventTypes) where T: Enum
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            var eventStrings = eventTypes.Select(e  => e.ToString()).ToList();

            Assert.HasCount(values.Count, eventTypes);

            foreach (var value in values)
            {
                Assert.Contains(value.ToString(), eventStrings);
            }

            TestValues(values);
        }
    }
}
