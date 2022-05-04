using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    public class EditorColorPalette
    {
        public Color AppToolbarBackground { get; private set; }
        public Color AppToolbarButtonBackground { get; private set; }
        public Color AppToolbarButtonBackgroundChecked { get; private set; }
        public Color AppToolbarButtonBackgroundHover { get; private set; }
        public Color AppToolbarButtonBackgroundPressed { get; private set; }
        public Color AppToolbarButtonBorder { get; private set; }
        public Color AppToolbarButtonBorderAccent { get; private set; }

        public Color ButtonBackground { get; private set; }
        public Color ButtonBackgroundFocus { get; private set; }
        public Color ButtonBackgroundHover { get; private set; }
        public Color ButtonBackgroundPressed { get; private set; }
        public Color ButtonBorder { get; private set; }
        public Color ButtonBorderAccent { get; private set; }
        public Color ButtonBorderPressed { get; private set; }
        public Color ButtonText { get; private set; }

        public Color DropdownBackground { get; private set; }
        public Color DropdownBackgroundHover { get; private set; }
        public Color DropdownBorder { get; private set; }
        public Color DropdownBorderAccent { get; private set; }
        public Color DropdownText { get; private set; }

        public Color HelpboxBackground { get; private set; }
        public Color HelpboxBorder { get; private set; }

        public Color InputFieldBackground { get; private set; }
        public Color InputFieldBorder { get; private set; }
        public Color InputFieldBorderAccent { get; private set; }
        public Color InputFieldBorderFocus { get; private set; }
        public Color InputFieldBorderHover { get; private set; }

        public Color ObjectFieldBackground { get; private set; }
        public Color ObjectFieldBorder { get; private set; }
        public Color ObjectFieldBorderFocus { get; private set; }
        public Color ObjectFieldBorderHover { get; private set; }
        public Color ObjectFieldButtonBackground { get; private set; }
        public Color ObjectFieldButtonBackgroundHover { get; private set; }

        public Color ScrollbarButtonBackground { get; private set; }
        public Color ScrollbarButtonBackgroundHover { get; private set; }
        public Color ScrollbarGrooveBackground { get; private set; }
        public Color ScrollbarGrooveBorder { get; private set; }
        public Color ScrollbarThumbBackground { get; private set; }
        public Color ScrollbarThumbBackgroundHover { get; private set; }
        public Color ScrollbarThumbBorder { get; private set; }
        public Color ScrollbarThumbBorderHover { get; private set; }

        public Color SliderGrooveBackground { get; private set; }
        public Color SliderGrooveBackgroundDisabled { get; private set; }
        public Color SliderThumbBackground { get; private set; }
        public Color SliderThumbBackgroundDisabled { get; private set; }
        public Color SliderThumbBackgroundHover { get; private set; }
        public Color SliderThumbBorder { get; private set; }
        public Color SliderThumbBorderDisabled { get; private set; }
        public Color SliderThumbHaloBackground { get; private set; }

        public Color TabBackground { get; private set; }
        public Color TabBackgroundChecked { get; private set; }
        public Color TabBackgroundHover { get; private set; }
        public Color TabHighlightBackground { get; private set; }

        public Color InspectorTitlebarBackground { get; private set; }
        public Color InspectorTitlebarBackgroundHover { get; private set; }
        public Color InspectorToolbarBackground { get; private set; }
        public Color InspectorWindowBackground { get; private set; }
        public Color InspectorTitlebarBorder { get; private set; }
        public Color InspectorTitlebarBorderAccent { get; private set; }

        public Color WindowTabDefaultBackground { get; private set; }
        public Color WindowHighlightBackgroundInactive { get; private set; }
        public Color WindowHighlightBackground { get; private set; }
        public Color WindowHighlightBackgroundHover { get; private set; }
        public Color WindowHighlightBackgroundHoverLighter { get; private set; }
        public Color WindowBackground { get; private set; }
        public Color WindowAlternatedRowsBackground { get; private set; }

        public Color DefaultBorder { get; private set; }
        public Color WindowBorder { get; private set; }

        public Color ToolbarBackground { get; private set; }
        public Color ToolbarBorder { get; private set; }
        public Color ToolbarButtonBackground { get; private set; }
        public Color ToolbarButtonBackgroundChecked { get; private set; }
        public Color ToolbarButtonBackgroundFocus { get; private set; }
        public Color ToolbarButtonBackgroundHover { get; private set; }
        public Color ToolbarButtonBorder { get; private set; }

        public Color DefaultText { get; private set; }
        public Color DefaultTextDisabled { get; private set; }
        public Color DefaultTextHover { get; private set; }
        public Color ErrorText { get; private set; }
        public Color LinkText { get; private set; }
        public Color VisitedLinkText { get; private set; }
        public Color WarningText { get; private set; }

        public Color HelpboxText { get; private set; }
        public Color HighlightText { get; private set; }
        public Color HighlightTextInactive { get; private set; }
        public Color LabelText { get; private set; }
        public Color LabelTextFocus { get; private set; }
        public Color PreviewOverlayText { get; private set; }
        public Color WindowText { get; private set; }

        public Color TabText { get; private set; }
        public Color ToolbarButtonText { get; private set; }
        public Color ToolbarButtonTextChecked { get; private set; }
        public Color ToolbarButtonTextHover { get; private set; }

        public static readonly EditorColorPalette Light;
        public static readonly EditorColorPalette Dark;

        public static EditorColorPalette Current => EditorGUIUtility.isProSkin ? Dark : Light;

        static EditorColorPalette()
        {
            Light = new EditorColorPalette
            {
                AppToolbarBackground = FromHex("#8A8A8A"),
                AppToolbarButtonBackground = FromHex("#C8C8C8"),
                AppToolbarButtonBackgroundChecked = FromHex("#656565"),
                AppToolbarButtonBackgroundHover = FromHex("#BBBBBB"),
                AppToolbarButtonBackgroundPressed = FromHex("#656565"),
                AppToolbarButtonBorder = FromHex("#6B6B6B"),
                AppToolbarButtonBorderAccent = FromHex("#6B6B6B"),

                ButtonBackground = FromHex("#E4E4E4"),
                ButtonBackgroundFocus = FromHex("#BEBEBE"),
                ButtonBackgroundHover = FromHex("#ECECEC"),
                ButtonBackgroundPressed = FromHex("#96C3FB"),
                ButtonBorder = FromHex("#B2B2B2"),
                ButtonBorderAccent = FromHex("#939393"),
                ButtonBorderPressed = FromHex("#707070"),
                ButtonText = FromHex("#090909"),

                DropdownBackground = FromHex("#DFDFDF"),
                DropdownBackgroundHover = FromHex("#E4E4E4"),
                DropdownBorder = FromHex("#B2B2B2"),
                DropdownBorderAccent = FromHex("#939393"),
                DropdownText = FromHex("#090909"),

                HelpboxBackground = new Color32(235, 235, 235, 52),
                HelpboxBorder = FromHex("#A9A9A9"),

                InputFieldBackground = FromHex("#F0F0F0"),
                InputFieldBorder = FromHex("#B7B7B7"),
                InputFieldBorderAccent = FromHex("#A0A0A0"),
                InputFieldBorderFocus = FromHex("#1D5087"),
                InputFieldBorderHover = FromHex("#6C6C6C"),

                ObjectFieldBackground = FromHex("#EDEDED"),
                ObjectFieldBorder = FromHex("#B0B0B0"),
                ObjectFieldBorderFocus = FromHex("#1D5087"),
                ObjectFieldBorderHover = FromHex("#6C6C6C"),
                ObjectFieldButtonBackground = FromHex("#DEDEDE"),
                ObjectFieldButtonBackgroundHover = FromHex("#CCCCCC"),

                ScrollbarButtonBackground = new Color(0f, 0f, 0f, 0.05098039f),
                ScrollbarButtonBackgroundHover = FromHex("#A7A7A7"),
                ScrollbarGrooveBackground = new Color(0f, 0f, 0f, 0.05098039f),
                ScrollbarGrooveBorder = new Color(0f, 0f, 0f, 0.1019608f),
                ScrollbarThumbBackground = FromHex("#9A9A9A"),
                ScrollbarThumbBackgroundHover = FromHex("#8E8E8E"),
                ScrollbarThumbBorder = FromHex("#B9B9B9"),
                ScrollbarThumbBorderHover = FromHex("#8E8E8E"),

                SliderGrooveBackground = FromHex("#8F8F8F"),
                SliderGrooveBackgroundDisabled = FromHex("#A4A4A4"),
                SliderThumbBackground = FromHex("#616161"),
                SliderThumbBackgroundDisabled = FromHex("#9B9B9B"),
                SliderThumbBackgroundHover = FromHex("#4F4F4F"),
                SliderThumbBorder = FromHex("#616161"),
                SliderThumbBorderDisabled = FromHex("#666666"),
                SliderThumbHaloBackground = new Color32(12, 108, 203, 113),

                TabBackground = FromHex("#B6B6B6"),
                TabBackgroundChecked = FromHex("#CBCBCB"),
                TabBackgroundHover = FromHex("#B0B0B0"),
                TabHighlightBackground = FromHex("#3A72B0"),

                InspectorTitlebarBackground = FromHex("#CBCBCB"),
                InspectorTitlebarBackgroundHover = FromHex("#D6D6D6"),
                InspectorToolbarBackground = FromHex("#CBCBCB"),
                InspectorWindowBackground = FromHex("#C8C8C8"),

                InspectorTitlebarBorder = FromHex("#7F7F7F"),
                InspectorTitlebarBorderAccent = FromHex("#BABABA"),

                WindowTabDefaultBackground = FromHex("#A5A5A5"),
                WindowHighlightBackgroundInactive = FromHex("#AEAEAE"),
                WindowHighlightBackground = FromHex("#3A72B0"),
                WindowHighlightBackgroundHover = new Color(0, 0, 0, 0.0627451f),
                WindowHighlightBackgroundHoverLighter = FromHex("#9A9A9A"),
                WindowBackground = FromHex("#C8C8C8"),
                WindowAlternatedRowsBackground = FromHex("#CACACA"),

                DefaultBorder = FromHex("#999999"),
                WindowBorder = FromHex("#939393"),

                ToolbarBackground = FromHex("#CBCBCB"),
                ToolbarBorder = FromHex("#999999"),
                ToolbarButtonBackground = FromHex("#CBCBCB"),
                ToolbarButtonBackgroundChecked = FromHex("#EFEFEF"),
                ToolbarButtonBackgroundFocus = FromHex("#C1C1C1"),
                ToolbarButtonBackgroundHover = FromHex("#C1C1C1"),
                ToolbarButtonBorder = FromHex("#999999"),

                DefaultText = FromHex("#090909"),
                DefaultTextDisabled = FromHex("#727272"),
                DefaultTextHover = FromHex("#090909"),
                ErrorText = FromHex("#5A0000"),
                LinkText = FromHex("#4C7EFF"),
                VisitedLinkText = FromHex("#AA00AA"),
                WarningText = FromHex("#333308"),

                HelpboxText = FromHex("#161616"),
                HighlightText = FromHex("#0032E6"),
                HighlightTextInactive = FromHex("#FFFFFF"),
                LabelText = FromHex("#090909"),
                LabelTextFocus = FromHex("#003C88"),
                PreviewOverlayText = FromHex("#FFFFFF"),
                WindowText = FromHex("#090909"),

                TabText = FromHex("#090909"),
                ToolbarButtonText = FromHex("#090909"),
                ToolbarButtonTextChecked = FromHex("#090909"),
                ToolbarButtonTextHover = FromHex("#090909"),
            };

            Dark = new EditorColorPalette
            {
                AppToolbarBackground = FromHex("#191919"),
                AppToolbarButtonBackground = FromHex("#383838"),
                AppToolbarButtonBackgroundChecked = FromHex("#6A6A6A"),
                AppToolbarButtonBackgroundHover = FromHex("#424242"),
                AppToolbarButtonBackgroundPressed = FromHex("#6A6A6A"),
                AppToolbarButtonBorder = FromHex("#191919"),
                AppToolbarButtonBorderAccent = FromHex("#222222"),

                ButtonBackground = FromHex("#585858"),
                ButtonBackgroundFocus = FromHex("#6E6E6E"),
                ButtonBackgroundHover = FromHex("#676767"),
                ButtonBackgroundPressed = FromHex("#46607C"),
                ButtonBorder = FromHex("#303030"),
                ButtonBorderAccent = FromHex("#242424"),
                ButtonBorderPressed = FromHex("#0D0D0D"),
                ButtonText = FromHex("#EEEEEE"),

                DropdownBackground = FromHex("#515151"),
                DropdownBackgroundHover = FromHex("#585858"),
                DropdownBorder = FromHex("#303030"),
                DropdownBorderAccent = FromHex("#242424"),
                DropdownText = FromHex("#E4E4E4"),

                HelpboxBackground = new Color32(96, 96, 96, 52),
                HelpboxBorder = FromHex("#232323"),

                InputFieldBackground = FromHex("#2A2A2A"),
                InputFieldBorder = FromHex("#212121"),
                InputFieldBorderAccent = FromHex("#0D0D0D"),
                InputFieldBorderFocus = FromHex("#3A79BB"),
                InputFieldBorderHover = FromHex("#656565"),

                ObjectFieldBackground = FromHex("#282828"),
                ObjectFieldBorder = FromHex("#202020"),
                ObjectFieldBorderFocus = FromHex("#3A79BB"),
                ObjectFieldBorderHover = FromHex("#656565"),
                ObjectFieldButtonBackground = FromHex("#373737"),
                ObjectFieldButtonBackgroundHover = FromHex("#4C4C4C"),

                ScrollbarButtonBackground = new Color(0f, 0f, 0f, 05098039f),
                ScrollbarButtonBackgroundHover = FromHex("#494949"),
                ScrollbarGrooveBackground = new Color(0f, 0f, 0f, 05098039f),
                ScrollbarGrooveBorder = new Color(0f, 0f, 0f, 0.1019608f),
                ScrollbarThumbBackground = FromHex("#5F5F5F"),
                ScrollbarThumbBackgroundHover = FromHex("#686868"),
                ScrollbarThumbBorder = FromHex("#323232"),
                ScrollbarThumbBorderHover = FromHex("#686868"),

                SliderGrooveBackground = FromHex("#5E5E5E"),
                SliderGrooveBackgroundDisabled = FromHex("#575757"),
                SliderThumbBackground = FromHex("#999999"),
                SliderThumbBackgroundDisabled = FromHex("#666666"),
                SliderThumbBackgroundHover = FromHex("#EAEAEA"),
                SliderThumbBorder = FromHex("#999999"),
                SliderThumbBorderDisabled = FromHex("#666666"),
                SliderThumbHaloBackground = new Color32(12, 108, 203, 113),

                TabBackground = FromHex("#353535"),
                TabBackgroundChecked = FromHex("#3C3C3C"),
                TabBackgroundHover = FromHex("#303030"),
                TabHighlightBackground = FromHex("#2C5D87"),

                InspectorTitlebarBackground = FromHex("#3E3E3E"),
                InspectorTitlebarBackgroundHover = FromHex("#474747"),
                InspectorToolbarBackground = FromHex("#3C3C3C"),
                InspectorWindowBackground = FromHex("#383838"),

                InspectorTitlebarBorder = FromHex("#1A1A1A"),
                InspectorTitlebarBorderAccent = FromHex("#303030"),

                WindowTabDefaultBackground = FromHex("#282828"),
                WindowHighlightBackgroundInactive = FromHex("#4D4D4D"),
                WindowHighlightBackground = FromHex("#2C5D87"),
                WindowHighlightBackgroundHover = new Color(0, 0, 0, 0627451f),
                WindowHighlightBackgroundHoverLighter = FromHex("#5F5F5F"),
                WindowBackground = FromHex("#383838"),
                WindowAlternatedRowsBackground = FromHex("#3F3F3F"),

                DefaultBorder = FromHex("#232323"),
                WindowBorder = FromHex("#242424"),

                ToolbarBackground = FromHex("#3C3C3C"),
                ToolbarBorder = FromHex("#232323"),
                ToolbarButtonBackground = FromHex("#3C3C3C"),
                ToolbarButtonBackgroundChecked = FromHex("#505050"),
                ToolbarButtonBackgroundFocus = FromHex("#464646"),
                ToolbarButtonBackgroundHover = FromHex("#464646"),
                ToolbarButtonBorder = FromHex("#232323"),

                DefaultText = FromHex("#D2D2D2"),
                DefaultTextDisabled = FromHex("#898989"),
                DefaultTextHover = FromHex("#BDBDBD"),
                ErrorText = FromHex("#D32222"),
                LinkText = FromHex("#4C7EFF"),
                VisitedLinkText = FromHex("#FF00FF"),
                WarningText = FromHex("#F4BC02"),

                HelpboxText = FromHex("#BDBDBD"),
                HighlightText = FromHex("#4C7EFF"),
                HighlightTextInactive = FromHex("#FFFFFF"),
                LabelText = FromHex("#C4C4C4"),
                LabelTextFocus = FromHex("#81B4FF"),
                PreviewOverlayText = FromHex("#DEDEDE"),
                WindowText = FromHex("#BDBDBD"),

                TabText = FromHex("#BDBDBD"),
                ToolbarButtonText = FromHex("#C4C4C4"),
                ToolbarButtonTextChecked = FromHex("#C4C4C4"),
                ToolbarButtonTextHover = FromHex("#BDBDBD"),
            };
        }

        private static Color FromHex(string hexString)
        {
            ColorUtility.TryParseHtmlString(hexString, out var c);
            return c;
        }
    }
}