using Marqdouj.DotNet.AzureMaps.Map;
using Marqdouj.DotNet.AzureMaps.Map.Controls;

namespace UnitTests
{
    [TestClass]
    public sealed class MapEnumExtensions
    {
        #region EnumToJson

        [TestMethod]
        public void MapEnumExt_EnumToJson()
        {
            var controlStyle = MapControlStyle.Light;
            const string result = "light";

            var value = controlStyle.EnumToJson();

            Assert.AreEqual(result, value);
        }

        [TestMethod]
        public void MapEnumExt_EnumToJson_Nullable_WithValue()
        {
            MapControlStyle? controlStyle = MapControlStyle.Light;
            const string result = "light";

            var v1 = controlStyle?.EnumToJson();

            Assert.AreEqual(result, v1);
        }

        [TestMethod]
        public void MapEnumExt_EnumToJson_Nullable_NoValue()
        {
            MapControlStyle? controlStyle = null;
            const string? result = null;

            var v1 = controlStyle?.EnumToJson();

            Assert.AreEqual(result, v1);
        }

        [TestMethod]
        public void MapEnumExt_EnumToJson_List_Nullable()
        {
            List<MapControlStyle>? items = [MapControlStyle.Light, MapControlStyle.Dark];

            var v1 = items?.EnumToJson();

            Assert.HasCount(2, v1!);
        }

        [TestMethod]
        public void MapEnumExt_EnumToJson_List_Nullable_Duplicates()
        {
            List<MapControlStyle>? items = [MapControlStyle.Light, MapControlStyle.Dark, MapControlStyle.Dark];

            var v1 = items?.EnumToJson();

            Assert.HasCount(2, v1!);
        }

        [TestMethod]
        public void MapEnumExt_EnumToJson_List_Nullable_Empty()
        {
            List<MapControlStyle>? items = [];

            var v1 = items?.EnumToJson();

            Assert.IsEmpty(v1!);
        }

        [TestMethod]
        public void MapEnumExt_EnumToJson_List_Nullable_Null()
        {
            List<MapControlStyle>? items = null;

            var v1 = items?.EnumToJson();

            Assert.IsNull(v1);
        }

        #endregion

        #region EnumToJsonN

        [TestMethod]
        public void MapEnumExt_EnumToJsonN_Nullable_WithValue()
        {
            MapControlStyle? controlStyle = MapControlStyle.Light;
            const string? result = "light";

            var v1 = controlStyle.EnumToJsonN();

            Assert.AreEqual(result, v1);
        }

        [TestMethod]
        public void MapEnumExt_EnumToJsonN_Nullable_NoValue()
        {
            MapControlStyle? controlStyle = null;
            const string? result = null;

            var v1 = controlStyle.EnumToJsonN();

            Assert.AreEqual(result, v1);
        }

        #endregion

        #region JsonToEnum

        [TestMethod]
        public void MapEnumExt_JsonToEnum_NullString()
        {
            string? controlStyle = null;

            var v1 = controlStyle.JsonToEnum<MapControlStyle>();
            var v2 = controlStyle?.JsonToEnum<MapControlStyle>();
            var v3 = controlStyle.JsonToEnum(MapControlStyle.Auto);
            var v4 = controlStyle?.JsonToEnum(MapControlStyle.Auto);

            Assert.AreEqual(MapControlStyle.Light, v1);
            Assert.IsNull(v2);
            Assert.AreEqual(MapControlStyle.Auto, v3);
            Assert.IsNull(v4);
        }

        [TestMethod]
        public void MapEnumExt_JsonToEnum_EmptyString()
        {
            string controlStyle = "";

            var v1 = controlStyle.JsonToEnum<MapControlStyle>();
            var v2 = controlStyle?.JsonToEnum<MapControlStyle>();
            var v3 = controlStyle.JsonToEnum(MapControlStyle.Auto);
            var v4 = controlStyle?.JsonToEnum(MapControlStyle.Auto);

            Assert.AreEqual(MapControlStyle.Light, v1);
            Assert.AreEqual(MapControlStyle.Light, v2);
            Assert.AreEqual(MapControlStyle.Auto, v3);
            Assert.AreEqual(MapControlStyle.Auto, v4);
        }

        [TestMethod]
        public void MapEnumExt_JsonToEnum_NullString_WithValue()
        {
            string? controlStyle = MapControlStyle.Light.EnumToJson();

            var v1 = controlStyle.JsonToEnum<MapControlStyle>();
            var v2 = controlStyle?.JsonToEnum<MapControlStyle>();
            var v3 = controlStyle.JsonToEnum(MapControlStyle.Auto);
            var v4 = controlStyle?.JsonToEnum(MapControlStyle.Auto);

            Assert.AreEqual(MapControlStyle.Light, v1);
            Assert.AreEqual(MapControlStyle.Light, v2);
            Assert.AreEqual(MapControlStyle.Light, v3);
            Assert.AreEqual(MapControlStyle.Light, v4);
        }

        [TestMethod]
        public void MapEnumExt_JsonToEnum_EmptyString_WithValue()
        {
            string controlStyle = MapControlStyle.Light.EnumToJson();

            var v1 = controlStyle.JsonToEnum<MapControlStyle>();
            var v2 = controlStyle?.JsonToEnum<MapControlStyle>();
            var v3 = controlStyle.JsonToEnum(MapControlStyle.Auto);
            var v4 = controlStyle?.JsonToEnum(MapControlStyle.Auto);

            Assert.AreEqual(MapControlStyle.Light, v1);
            Assert.AreEqual(MapControlStyle.Light, v2);
            Assert.AreEqual(MapControlStyle.Light, v3);
            Assert.AreEqual(MapControlStyle.Light, v4);
        }

        [TestMethod]
        public void MapEnumExt_JsonToEnum_List_Nullable()
        {
            List<string>? items = [MapControlStyle.Light.EnumToJson(), MapControlStyle.Dark.EnumToJson()];

            var v1 = items?.JsonToEnum<MapControlStyle>();

            Assert.HasCount(2, v1!);
        }

        [TestMethod]
        public void MapEnumExt_JsonToEnum_List_Nullable_Duplicates()
        {
            List<string>? items = [MapControlStyle.Light.EnumToJson(), MapControlStyle.Dark.EnumToJson(), MapControlStyle.Light.EnumToJson()];

            var v1 = items?.JsonToEnum<MapControlStyle>();

            Assert.HasCount(2, v1!);
        }

        [TestMethod]
        public void MapEnumExt_JsonToEnum_List_Nullable_Empty()
        {
            List<string>? items = [];

            var v1 = items?.JsonToEnum<MapControlStyle>();

            Assert.IsEmpty(v1!);
        }

        [TestMethod]
        public void MapEnumExt_JsonToEnum_List_Nullable_Null()
        {
            List<string>? items = null;

            var v1 = items.JsonToEnum<MapControlStyle>();
            var v2 = items?.JsonToEnum<MapControlStyle>();

            Assert.IsEmpty(v1);
            Assert.IsNull(v2);
        }


        #endregion

        #region JsonToEnumN

        [TestMethod]
        public void MapEnumExt_JsonToEnumN_NullString()
        {
            string? controlStyle = null;

            var v1 = controlStyle.JsonToEnumN<MapControlStyle>();
            var v2 = controlStyle?.JsonToEnumN<MapControlStyle>();

            Assert.IsNull(v1);
            Assert.IsNull(v2);
        }

        [TestMethod]
        public void MapEnumExt_JsonToEnumN_EmptyString()
        {
            string controlStyle = "";

            var v1 = controlStyle.JsonToEnumN<MapControlStyle>();
            var v2 = controlStyle?.JsonToEnumN<MapControlStyle>();

            Assert.IsNull(v1);
            Assert.IsNull(v2);
        }

        [TestMethod]
        public void MapEnumExt_JsonToEnumN_String_WithValue()
        {
            string controlStyle = MapControlStyle.Light.EnumToJson();

            var v1 = controlStyle.JsonToEnumN<MapControlStyle>();
            var v2 = controlStyle?.JsonToEnumN<MapControlStyle>();

            Assert.AreEqual(MapControlStyle.Light, v1);
            Assert.AreEqual(MapControlStyle.Light, v2);
        }

        [TestMethod]
        public void MapEnumExt_JsonToEnumN_NullString_WithValue()
        {
            string? controlStyle = MapControlStyle.Light.EnumToJson();

            var v1 = controlStyle.JsonToEnumN<MapControlStyle>();
            var v2 = controlStyle?.JsonToEnumN<MapControlStyle>();

            Assert.AreEqual(MapControlStyle.Light, v1);
            Assert.AreEqual(MapControlStyle.Light, v2);
        }

        #endregion
    }
}
