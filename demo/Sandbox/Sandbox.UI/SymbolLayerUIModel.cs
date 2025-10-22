using Marqdouj.DotNet.AzureMaps.Map.Interop.Layers;
using Marqdouj.DotNet.Web.Components.UI;

namespace Sandbox.UI
{
    public class SymbolLayerUIModel : LayerUIModel<SymbolLayerDef>, ICloneable
    {
        private readonly SymbolLayerOptionsUIModel options;

        public SymbolLayerUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            options = new(xmlService);
            Source = new();
        }

        public override SymbolLayerDef? Source
        {
            get => base.Source;
            set
            {
                base.Source = value;
                value?.Options ??= new();
                options.Source = value?.Options;
            }
        }

        public override List<IUIModelValue> ToUIList()
        {
            var items = base.ToUIList();
            items.RemoveAll(e => e.Name == nameof(SymbolLayerDef.Options));
            items.AddRange(options.ToUIList());

            return [.. items.OrderBy(e => e.SortOrder).ThenBy(e => e.NameDisplay)];
        }

        public override UIViewStyle ViewStyle 
        { 
            get => base.ViewStyle; 
            set
            {
                base.ViewStyle = value;
                options.IconOptionsUI.ViewStyle = value;
                options.TextOptionsUI.ViewStyle = value;
            }
        }

        public object Clone()
        {
            var clone = (SymbolLayerUIModel)this.MemberwiseClone();
            clone.Source = Source?.Clone() as SymbolLayerDef;

            return clone;
        }

        public SymbolLayerOptionsUIModel Options => options;
    }

    public class SymbolLayerOptionsUIModel : SourceLayerOptionsUIModel<SymbolLayerOptions>
    {
        private readonly IconOptionsUIModel iconOptionsUI;
        private readonly TextOptionsUIModel textOptionsUI;

        internal SymbolLayerOptionsUIModel(IAzureMapsXmlService? xmlService) : base(xmlService)
        {
            iconOptionsUI = new(xmlService);
            textOptionsUI = new(xmlService);
            Source = new();
        }

        public override SymbolLayerOptions? Source
        {
            get => base.Source;
            set
            {
                base.Source = value;
                value?.IconOptions ??= new IconOptions();
                value?.TextOptions ??= new TextOptions();
                iconOptionsUI.Source = value?.IconOptions;
                textOptionsUI.Source = value?.TextOptions;
            }
        }

        public override List<IUIModelValue> ToUIList()
        {
            var items = base.ToUIList();
            items.RemoveAll(e => e.Name == nameof(SymbolLayerOptions.IconOptions) || e.Name == nameof(SymbolLayerOptions.TextOptions));
            return items;
        }

        public IUIModelValue IconOptions => GetItem(nameof(SymbolLayerOptions.IconOptions))!;
        public IconOptionsUIModel IconOptionsUI => iconOptionsUI;
        public IUIModelValue LineSpacing => GetItem(nameof(SymbolLayerOptions.LineSpacing))!;
        public IUIModelValue TextOptions => GetItem(nameof(SymbolLayerOptions.TextOptions))!;
        public TextOptionsUIModel TextOptionsUI => textOptionsUI;
        public IUIModelValue Placement => GetItem(nameof(SymbolLayerOptions.Placement))!;
        public IUIModelValue SortKey => GetItem(nameof(SymbolLayerOptions.SortKey))!;
        public IUIModelValue ZOrder => GetItem(nameof(SymbolLayerOptions.ZOrder))!;
    }

    public class IconOptionsUIModel(IAzureMapsXmlService? xmlService) : XmlUIModel<IconOptions>(xmlService)
    {
        private readonly PixelUIModel offsetUI = new(xmlService);

        public UIViewStyle ViewStyle { get; set; }

        public IUIModelValue? AllowOverlap => GetItem(nameof(IconOptions.AllowOverlap));
        public IUIModelValue? Anchor => GetItem(nameof(IconOptions.Anchor));
        public IUIModelValue? IgnorePlacement => GetItem(nameof(IconOptions.IgnorePlacement));
        public IUIModelValue? Image => GetItem(nameof(IconOptions.Image));
        public IUIModelValue? Offset => GetItem(nameof(IconOptions.Offset));
        public PixelUIModel OffsetUI => offsetUI;
        public IUIModelValue? Opacity => GetItem(nameof(IconOptions.Opacity));
        public IUIModelValue? Optional => GetItem(nameof(IconOptions.Optional));
        public IUIModelValue? Padding => GetItem(nameof(IconOptions.Padding));
        public IUIModelValue? PitchAlignment => GetItem(nameof(IconOptions.PitchAlignment));
        public IUIModelValue? Rotation => GetItem(nameof(IconOptions.Rotation));
        public IUIModelValue? RotationAlignment => GetItem(nameof(IconOptions.RotationAlignment));
        public IUIModelValue? Size => GetItem(nameof(IconOptions.Size));
    }

    public class TextOptionsUIModel(IAzureMapsXmlService? xmlService) : XmlUIModel<TextOptions>(xmlService)
    {
        private readonly PixelUIModel offsetUI = new(xmlService);

        public UIViewStyle ViewStyle { get; set; }

        public IUIModelValue? AllowOverlap => GetItem(nameof(TextOptions.AllowOverlap));
        public IUIModelValue? Anchor => GetItem(nameof(TextOptions.Anchor));
        public IUIModelValue? Color => GetItem(nameof(TextOptions.Color));
        public IUIModelValue? Font => GetItem(nameof(TextOptions.Font));
        public IUIModelValue? HaloBlur => GetItem(nameof(TextOptions.HaloBlur));
        public IUIModelValue? HaloColor => GetItem(nameof(TextOptions.HaloColor));
        public IUIModelValue? HaloWidth => GetItem(nameof(TextOptions.HaloWidth));
        public IUIModelValue? IgnorePlacement => GetItem(nameof(TextOptions.IgnorePlacement));
        public IUIModelValue? Justify => GetItem(nameof(TextOptions.Justify));
        public IUIModelValue? Offset => GetItem(nameof(TextOptions.Offset));
        public PixelUIModel OffsetUI => offsetUI;
        public IUIModelValue? Opacity => GetItem(nameof(TextOptions.Opacity));
        public IUIModelValue? Optional => GetItem(nameof(TextOptions.Optional));
        public IUIModelValue? Padding => GetItem(nameof(TextOptions.Padding));
        public IUIModelValue? PitchAlignment => GetItem(nameof(TextOptions.PitchAlignment));
        public IUIModelValue? RadialOffset => GetItem(nameof(TextOptions.RadialOffset));
        public IUIModelValue? Rotation => GetItem(nameof(TextOptions.Rotation));
        public IUIModelValue? RotationAlignment => GetItem(nameof(TextOptions.RotationAlignment));
        public IUIModelValue? Size => GetItem(nameof(TextOptions.Size));
        public IUIModelValue? TextField => GetItem(nameof(TextOptions.TextField));
        public IUIModelValue? VariableAnchor => GetItem(nameof(TextOptions.VariableAnchor));
    }
}
