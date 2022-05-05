using System;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    [Serializable]
    public class EditorColorPalette
    {
        #region fields

        // @formatter:place_accessorholder_attribute_on_same_line true
        // @formatter:keep_blank_lines_in_declarations 0
        [field: SerializeField] public Color AppToolbarBackground { get; private set; }
        [field: SerializeField] public Color AppToolbarButtonBackground { get; private set; }
        [field: SerializeField] public Color AppToolbarButtonBackgroundChecked { get; private set; }
        [field: SerializeField] public Color AppToolbarButtonBackgroundHover { get; private set; }
        [field: SerializeField] public Color AppToolbarButtonBackgroundPressed { get; private set; }
        [field: SerializeField] public Color AppToolbarButtonBorder { get; private set; }
        [field: SerializeField] public Color AppToolbarButtonBorderAccent { get; private set; }
        [field: SerializeField] public Color ButtonBackground { get; private set; }
        [field: SerializeField] public Color ButtonBackgroundFocus { get; private set; }
        [field: SerializeField] public Color ButtonBackgroundHover { get; private set; }
        [field: SerializeField] public Color ButtonBackgroundPressed { get; private set; }
        [field: SerializeField] public Color ButtonBorder { get; private set; }
        [field: SerializeField] public Color ButtonBorderAccent { get; private set; }
        [field: SerializeField] public Color ButtonBorderPressed { get; private set; }
        [field: SerializeField] public Color ButtonText { get; private set; }
        [field: SerializeField] public Color DropdownBackground { get; private set; }
        [field: SerializeField] public Color DropdownBackgroundHover { get; private set; }
        [field: SerializeField] public Color DropdownBorder { get; private set; }
        [field: SerializeField] public Color DropdownBorderAccent { get; private set; }
        [field: SerializeField] public Color DropdownText { get; private set; }
        [field: SerializeField] public Color HelpboxBackground { get; private set; }
        [field: SerializeField] public Color HelpboxBorder { get; private set; }
        [field: SerializeField] public Color InputFieldBackground { get; private set; }
        [field: SerializeField] public Color InputFieldBorder { get; private set; }
        [field: SerializeField] public Color InputFieldBorderAccent { get; private set; }
        [field: SerializeField] public Color InputFieldBorderFocus { get; private set; }
        [field: SerializeField] public Color InputFieldBorderHover { get; private set; }
        [field: SerializeField] public Color ObjectFieldBackground { get; private set; }
        [field: SerializeField] public Color ObjectFieldBorder { get; private set; }
        [field: SerializeField] public Color ObjectFieldBorderFocus { get; private set; }
        [field: SerializeField] public Color ObjectFieldBorderHover { get; private set; }
        [field: SerializeField] public Color ObjectFieldButtonBackground { get; private set; }
        [field: SerializeField] public Color ObjectFieldButtonBackgroundHover { get; private set; }
        [field: SerializeField] public Color ScrollbarButtonBackground { get; private set; }
        [field: SerializeField] public Color ScrollbarButtonBackgroundHover { get; private set; }
        [field: SerializeField] public Color ScrollbarGrooveBackground { get; private set; }
        [field: SerializeField] public Color ScrollbarGrooveBorder { get; private set; }
        [field: SerializeField] public Color ScrollbarThumbBackground { get; private set; }
        [field: SerializeField] public Color ScrollbarThumbBackgroundHover { get; private set; }
        [field: SerializeField] public Color ScrollbarThumbBorder { get; private set; }
        [field: SerializeField] public Color ScrollbarThumbBorderHover { get; private set; }
        [field: SerializeField] public Color SliderGrooveBackground { get; private set; }
        [field: SerializeField] public Color SliderGrooveBackgroundDisabled { get; private set; }
        [field: SerializeField] public Color SliderThumbBackground { get; private set; }
        [field: SerializeField] public Color SliderThumbBackgroundDisabled { get; private set; }
        [field: SerializeField] public Color SliderThumbBackgroundHover { get; private set; }
        [field: SerializeField] public Color SliderThumbBorder { get; private set; }
        [field: SerializeField] public Color SliderThumbBorderDisabled { get; private set; }
        [field: SerializeField] public Color SliderThumbHaloBackground { get; private set; }
        [field: SerializeField] public Color TabBackground { get; private set; }
        [field: SerializeField] public Color TabBackgroundChecked { get; private set; }
        [field: SerializeField] public Color TabBackgroundHover { get; private set; }
        [field: SerializeField] public Color TabHighlightBackground { get; private set; }
        [field: SerializeField] public Color InspectorTitlebarBackground { get; private set; }
        [field: SerializeField] public Color InspectorTitlebarBackgroundHover { get; private set; }
        [field: SerializeField] public Color InspectorToolbarBackground { get; private set; }
        [field: SerializeField] public Color InspectorWindowBackground { get; private set; }
        [field: SerializeField] public Color InspectorTitlebarBorder { get; private set; }
        [field: SerializeField] public Color InspectorTitlebarBorderAccent { get; private set; }
        [field: SerializeField] public Color WindowTabDefaultBackground { get; private set; }
        [field: SerializeField] public Color WindowHighlightBackgroundInactive { get; private set; }
        [field: SerializeField] public Color WindowHighlightBackground { get; private set; }
        [field: SerializeField] public Color WindowHighlightBackgroundHover { get; private set; }
        [field: SerializeField] public Color WindowHighlightBackgroundHoverLighter { get; private set; }
        [field: SerializeField] public Color WindowBackground { get; private set; }
        [field: SerializeField] public Color WindowAlternatedRowsBackground { get; private set; }
        [field: SerializeField] public Color DefaultBorder { get; private set; }
        [field: SerializeField] public Color WindowBorder { get; private set; }
        [field: SerializeField] public Color ToolbarBackground { get; private set; }
        [field: SerializeField] public Color ToolbarBorder { get; private set; }
        [field: SerializeField] public Color ToolbarButtonBackground { get; private set; }
        [field: SerializeField] public Color ToolbarButtonBackgroundChecked { get; private set; }
        [field: SerializeField] public Color ToolbarButtonBackgroundFocus { get; private set; }
        [field: SerializeField] public Color ToolbarButtonBackgroundHover { get; private set; }
        [field: SerializeField] public Color ToolbarButtonBorder { get; private set; }
        [field: SerializeField] public Color DefaultText { get; private set; }
        [field: SerializeField] public Color DefaultTextDisabled { get; private set; }
        [field: SerializeField] public Color DefaultTextHover { get; private set; }
        [field: SerializeField] public Color ErrorText { get; private set; }
        [field: SerializeField] public Color LinkText { get; private set; }
        [field: SerializeField] public Color VisitedLinkText { get; private set; }
        [field: SerializeField] public Color WarningText { get; private set; }
        [field: SerializeField] public Color HelpboxText { get; private set; }
        [field: SerializeField] public Color HighlightText { get; private set; }
        [field: SerializeField] public Color HighlightTextInactive { get; private set; }
        [field: SerializeField] public Color LabelText { get; private set; }
        [field: SerializeField] public Color LabelTextFocus { get; private set; }
        [field: SerializeField] public Color PreviewOverlayText { get; private set; }
        [field: SerializeField] public Color WindowText { get; private set; }
        [field: SerializeField] public Color TabText { get; private set; }
        [field: SerializeField] public Color ToolbarButtonText { get; private set; }
        [field: SerializeField] public Color ToolbarButtonTextChecked { get; private set; }
        [field: SerializeField] public Color ToolbarButtonTextHover { get; private set; }

        // @formatter:keep_blank_lines_in_declarations restore
        // @formatter:place_accessorholder_attribute_on_same_line restore

        public static readonly EditorColorPalette Light;
        public static readonly EditorColorPalette Dark;

        #endregion

        public static EditorColorPalette Current => EditorGUIUtility.isProSkin ? Dark : Light;

        static EditorColorPalette()
        {
            // @formatter:int_align_assignments true
            Light = new EditorColorPalette
            {
                AppToolbarBackground                  = new Color32(0x8A, 0x8A, 0x8A, 255),
                AppToolbarButtonBackground            = new Color32(0xC8, 0xC8, 0xC8, 255),
                AppToolbarButtonBackgroundChecked     = new Color32(0x65, 0x65, 0x65, 255),
                AppToolbarButtonBackgroundHover       = new Color32(0xBB, 0xBB, 0xBB, 255),
                AppToolbarButtonBackgroundPressed     = new Color32(0x65, 0x65, 0x65, 255),
                AppToolbarButtonBorder                = new Color32(0x6B, 0x6B, 0x6B, 255),
                AppToolbarButtonBorderAccent          = new Color32(0x6B, 0x6B, 0x6B, 255), //
                ButtonBackground                      = new Color32(0xE4, 0xE4, 0xE4, 255),
                ButtonBackgroundFocus                 = new Color32(0xBE, 0xBE, 0xBE, 255),
                ButtonBackgroundHover                 = new Color32(0xEC, 0xEC, 0xEC, 255),
                ButtonBackgroundPressed               = new Color32(0x96, 0xC3, 0xFB, 255),
                ButtonBorder                          = new Color32(0xB2, 0xB2, 0xB2, 255),
                ButtonBorderAccent                    = new Color32(0x93, 0x93, 0x93, 255),
                ButtonBorderPressed                   = new Color32(0x70, 0x70, 0x70, 255),
                ButtonText                            = new Color32(0x09, 0x09, 0x09, 255), //
                DropdownBackground                    = new Color32(0xDF, 0xDF, 0xDF, 255),
                DropdownBackgroundHover               = new Color32(0xE4, 0xE4, 0xE4, 255),
                DropdownBorder                        = new Color32(0xB2, 0xB2, 0xB2, 255),
                DropdownBorderAccent                  = new Color32(0x93, 0x93, 0x93, 255),
                DropdownText                          = new Color32(0x09, 0x09, 0x09, 255), //
                HelpboxBackground                     = new Color32(235, 235, 235, 52),
                HelpboxBorder                         = new Color32(0xA9, 0xA9, 0xA9, 255), //
                InputFieldBackground                  = new Color32(0xF0, 0xF0, 0xF0, 255),
                InputFieldBorder                      = new Color32(0xB7, 0xB7, 0xB7, 255),
                InputFieldBorderAccent                = new Color32(0xA0, 0xA0, 0xA0, 255),
                InputFieldBorderFocus                 = new Color32(0x1D, 0x50, 0x87, 255),
                InputFieldBorderHover                 = new Color32(0x6C, 0x6C, 0x6C, 255), //
                ObjectFieldBackground                 = new Color32(0xED, 0xED, 0xED, 255),
                ObjectFieldBorder                     = new Color32(0xB0, 0xB0, 0xB0, 255),
                ObjectFieldBorderFocus                = new Color32(0x1D, 0x50, 0x87, 255),
                ObjectFieldBorderHover                = new Color32(0x6C, 0x6C, 0x6C, 255),
                ObjectFieldButtonBackground           = new Color32(0xDE, 0xDE, 0xDE, 255),
                ObjectFieldButtonBackgroundHover      = new Color32(0xCC, 0xCC, 0xCC, 255), //
                ScrollbarButtonBackground             = new Color(0f, 0f, 0f, 0.05098039f),
                ScrollbarButtonBackgroundHover        = new Color32(0xA7, 0xA7, 0xA7, 255),
                ScrollbarGrooveBackground             = new Color(0f, 0f, 0f, 0.05098039f),
                ScrollbarGrooveBorder                 = new Color(0f, 0f, 0f, 0.1019608f),
                ScrollbarThumbBackground              = new Color32(0x9A, 0x9A, 0x9A, 255),
                ScrollbarThumbBackgroundHover         = new Color32(0x8E, 0x8E, 0x8E, 255),
                ScrollbarThumbBorder                  = new Color32(0xB9, 0xB9, 0xB9, 255),
                ScrollbarThumbBorderHover             = new Color32(0x8E, 0x8E, 0x8E, 255), //
                SliderGrooveBackground                = new Color32(0x8F, 0x8F, 0x8F, 255),
                SliderGrooveBackgroundDisabled        = new Color32(0xA4, 0xA4, 0xA4, 255),
                SliderThumbBackground                 = new Color32(0x61, 0x61, 0x61, 255),
                SliderThumbBackgroundDisabled         = new Color32(0x9B, 0x9B, 0x9B, 255),
                SliderThumbBackgroundHover            = new Color32(0x4F, 0x4F, 0x4F, 255),
                SliderThumbBorder                     = new Color32(0x61, 0x61, 0x61, 255),
                SliderThumbBorderDisabled             = new Color32(0x66, 0x66, 0x66, 255),
                SliderThumbHaloBackground             = new Color32(12, 108, 203, 113), //
                TabBackground                         = new Color32(0xB6, 0xB6, 0xB6, 255),
                TabBackgroundChecked                  = new Color32(0xCB, 0xCB, 0xCB, 255),
                TabBackgroundHover                    = new Color32(0xB0, 0xB0, 0xB0, 255),
                TabHighlightBackground                = new Color32(0x3A, 0x72, 0xB0, 255), //
                InspectorTitlebarBackground           = new Color32(0xCB, 0xCB, 0xCB, 255),
                InspectorTitlebarBackgroundHover      = new Color32(0xD6, 0xD6, 0xD6, 255),
                InspectorToolbarBackground            = new Color32(0xCB, 0xCB, 0xCB, 255),
                InspectorWindowBackground             = new Color32(0xC8, 0xC8, 0xC8, 255), //
                InspectorTitlebarBorder               = new Color32(0x7F, 0x7F, 0x7F, 255),
                InspectorTitlebarBorderAccent         = new Color32(0xBA, 0xBA, 0xBA, 255), //
                WindowTabDefaultBackground            = new Color32(0xA5, 0xA5, 0xA5, 255),
                WindowHighlightBackgroundInactive     = new Color32(0xAE, 0xAE, 0xAE, 255),
                WindowHighlightBackground             = new Color32(0x3A, 0x72, 0xB0, 255),
                WindowHighlightBackgroundHover        = new Color(0, 0, 0, 0.0627451f),
                WindowHighlightBackgroundHoverLighter = new Color32(0x9A, 0x9A, 0x9A, 255),
                WindowBackground                      = new Color32(0xC8, 0xC8, 0xC8, 255),
                WindowAlternatedRowsBackground        = new Color32(0xCA, 0xCA, 0xCA, 255), //
                DefaultBorder                         = new Color32(0x99, 0x99, 0x99, 255),
                WindowBorder                          = new Color32(0x93, 0x93, 0x93, 255), //
                ToolbarBackground                     = new Color32(0xCB, 0xCB, 0xCB, 255),
                ToolbarBorder                         = new Color32(0x99, 0x99, 0x99, 255),
                ToolbarButtonBackground               = new Color32(0xCB, 0xCB, 0xCB, 255),
                ToolbarButtonBackgroundChecked        = new Color32(0xEF, 0xEF, 0xEF, 255),
                ToolbarButtonBackgroundFocus          = new Color32(0xC1, 0xC1, 0xC1, 255),
                ToolbarButtonBackgroundHover          = new Color32(0xC1, 0xC1, 0xC1, 255),
                ToolbarButtonBorder                   = new Color32(0x99, 0x99, 0x99, 255), //
                DefaultText                           = new Color32(0x09, 0x09, 0x09, 255),
                DefaultTextDisabled                   = new Color32(0x72, 0x72, 0x72, 255),
                DefaultTextHover                      = new Color32(0x09, 0x09, 0x09, 255),
                ErrorText                             = new Color32(0x5A, 0x00, 0x00, 255),
                LinkText                              = new Color32(0x4C, 0x7E, 0xFF, 255),
                VisitedLinkText                       = new Color32(0xAA, 0x00, 0xAA, 255),
                WarningText                           = new Color32(0x33, 0x33, 0x08, 255), //
                HelpboxText                           = new Color32(0x16, 0x16, 0x16, 255),
                HighlightText                         = new Color32(0x00, 0x32, 0xE6, 255),
                HighlightTextInactive                 = new Color32(0xFF, 0xFF, 0xFF, 255),
                LabelText                             = new Color32(0x09, 0x09, 0x09, 255),
                LabelTextFocus                        = new Color32(0x00, 0x3C, 0x88, 255),
                PreviewOverlayText                    = new Color32(0xFF, 0xFF, 0xFF, 255),
                WindowText                            = new Color32(0x09, 0x09, 0x09, 255), //
                TabText                               = new Color32(0x09, 0x09, 0x09, 255),
                ToolbarButtonText                     = new Color32(0x09, 0x09, 0x09, 255),
                ToolbarButtonTextChecked              = new Color32(0x09, 0x09, 0x09, 255),
                ToolbarButtonTextHover                = new Color32(0x09, 0x09, 0x09, 255),
            };

            Dark = new EditorColorPalette
            {
                AppToolbarBackground                  = new Color32(0x19, 0x19, 0x19, 255),
                AppToolbarButtonBackground            = new Color32(0x38, 0x38, 0x38, 255),
                AppToolbarButtonBackgroundChecked     = new Color32(0x6A, 0x6A, 0x6A, 255),
                AppToolbarButtonBackgroundHover       = new Color32(0x42, 0x42, 0x42, 255),
                AppToolbarButtonBackgroundPressed     = new Color32(0x6A, 0x6A, 0x6A, 255),
                AppToolbarButtonBorder                = new Color32(0x19, 0x19, 0x19, 255),
                AppToolbarButtonBorderAccent          = new Color32(0x22, 0x22, 0x22, 255), //
                ButtonBackground                      = new Color32(0x58, 0x58, 0x58, 255),
                ButtonBackgroundFocus                 = new Color32(0x6E, 0x6E, 0x6E, 255),
                ButtonBackgroundHover                 = new Color32(0x67, 0x67, 0x67, 255),
                ButtonBackgroundPressed               = new Color32(0x46, 0x60, 0x7C, 255),
                ButtonBorder                          = new Color32(0x30, 0x30, 0x30, 255),
                ButtonBorderAccent                    = new Color32(0x24, 0x24, 0x24, 255),
                ButtonBorderPressed                   = new Color32(0x0D, 0x0D, 0x0D, 255),
                ButtonText                            = new Color32(0xEE, 0xEE, 0xEE, 255), //
                DropdownBackground                    = new Color32(0x51, 0x51, 0x51, 255),
                DropdownBackgroundHover               = new Color32(0x58, 0x58, 0x58, 255),
                DropdownBorder                        = new Color32(0x30, 0x30, 0x30, 255),
                DropdownBorderAccent                  = new Color32(0x24, 0x24, 0x24, 255),
                DropdownText                          = new Color32(0xE4, 0xE4, 0xE4, 255), //
                HelpboxBackground                     = new Color32(96, 96, 96, 52),
                HelpboxBorder                         = new Color32(0x23, 0x23, 0x23, 255), //
                InputFieldBackground                  = new Color32(0x2A, 0x2A, 0x2A, 255),
                InputFieldBorder                      = new Color32(0x21, 0x21, 0x21, 255),
                InputFieldBorderAccent                = new Color32(0x0D, 0x0D, 0x0D, 255),
                InputFieldBorderFocus                 = new Color32(0x3A, 0x79, 0xBB, 255),
                InputFieldBorderHover                 = new Color32(0x65, 0x65, 0x65, 255), //
                ObjectFieldBackground                 = new Color32(0x28, 0x28, 0x28, 255),
                ObjectFieldBorder                     = new Color32(0x20, 0x20, 0x20, 255),
                ObjectFieldBorderFocus                = new Color32(0x3A, 0x79, 0xBB, 255),
                ObjectFieldBorderHover                = new Color32(0x65, 0x65, 0x65, 255),
                ObjectFieldButtonBackground           = new Color32(0x37, 0x37, 0x37, 255),
                ObjectFieldButtonBackgroundHover      = new Color32(0x4C, 0x4C, 0x4C, 255), //
                ScrollbarButtonBackground             = new Color(0f, 0f, 0f, 05098039f),
                ScrollbarButtonBackgroundHover        = new Color32(0x49, 0x49, 0x49, 255),
                ScrollbarGrooveBackground             = new Color(0f, 0f, 0f, 05098039f),
                ScrollbarGrooveBorder                 = new Color(0f, 0f, 0f, 0.1019608f),
                ScrollbarThumbBackground              = new Color32(0x5F, 0x5F, 0x5F, 255),
                ScrollbarThumbBackgroundHover         = new Color32(0x68, 0x68, 0x68, 255),
                ScrollbarThumbBorder                  = new Color32(0x32, 0x32, 0x32, 255),
                ScrollbarThumbBorderHover             = new Color32(0x68, 0x68, 0x68, 255), //
                SliderGrooveBackground                = new Color32(0x5E, 0x5E, 0x5E, 255),
                SliderGrooveBackgroundDisabled        = new Color32(0x57, 0x57, 0x57, 255),
                SliderThumbBackground                 = new Color32(0x99, 0x99, 0x99, 255),
                SliderThumbBackgroundDisabled         = new Color32(0x66, 0x66, 0x66, 255),
                SliderThumbBackgroundHover            = new Color32(0xEA, 0xEA, 0xEA, 255),
                SliderThumbBorder                     = new Color32(0x99, 0x99, 0x99, 255),
                SliderThumbBorderDisabled             = new Color32(0x66, 0x66, 0x66, 255),
                SliderThumbHaloBackground             = new Color32(12, 108, 203, 113), //
                TabBackground                         = new Color32(0x35, 0x35, 0x35, 255),
                TabBackgroundChecked                  = new Color32(0x3C, 0x3C, 0x3C, 255),
                TabBackgroundHover                    = new Color32(0x30, 0x30, 0x30, 255),
                TabHighlightBackground                = new Color32(0x2C, 0x5D, 0x87, 255), //
                InspectorTitlebarBackground           = new Color32(0x3E, 0x3E, 0x3E, 255),
                InspectorTitlebarBackgroundHover      = new Color32(0x47, 0x47, 0x47, 255),
                InspectorToolbarBackground            = new Color32(0x3C, 0x3C, 0x3C, 255),
                InspectorWindowBackground             = new Color32(0x38, 0x38, 0x38, 255), //
                InspectorTitlebarBorder               = new Color32(0x1A, 0x1A, 0x1A, 255),
                InspectorTitlebarBorderAccent         = new Color32(0x30, 0x30, 0x30, 255), //
                WindowTabDefaultBackground            = new Color32(0x28, 0x28, 0x28, 255),
                WindowHighlightBackgroundInactive     = new Color32(0x4D, 0x4D, 0x4D, 255),
                WindowHighlightBackground             = new Color32(0x2C, 0x5D, 0x87, 255),
                WindowHighlightBackgroundHover        = new Color(0, 0, 0, 0627451f),
                WindowHighlightBackgroundHoverLighter = new Color32(0x5F, 0x5F, 0x5F, 255),
                WindowBackground                      = new Color32(0x38, 0x38, 0x38, 255),
                WindowAlternatedRowsBackground        = new Color32(0x3F, 0x3F, 0x3F, 255), //
                DefaultBorder                         = new Color32(0x23, 0x23, 0x23, 255),
                WindowBorder                          = new Color32(0x24, 0x24, 0x24, 255), //
                ToolbarBackground                     = new Color32(0x3C, 0x3C, 0x3C, 255),
                ToolbarBorder                         = new Color32(0x23, 0x23, 0x23, 255),
                ToolbarButtonBackground               = new Color32(0x3C, 0x3C, 0x3C, 255),
                ToolbarButtonBackgroundChecked        = new Color32(0x50, 0x50, 0x50, 255),
                ToolbarButtonBackgroundFocus          = new Color32(0x46, 0x46, 0x46, 255),
                ToolbarButtonBackgroundHover          = new Color32(0x46, 0x46, 0x46, 255),
                ToolbarButtonBorder                   = new Color32(0x23, 0x23, 0x23, 255), //
                DefaultText                           = new Color32(0xD2, 0xD2, 0xD2, 255),
                DefaultTextDisabled                   = new Color32(0x89, 0x89, 0x89, 255),
                DefaultTextHover                      = new Color32(0xBD, 0xBD, 0xBD, 255),
                ErrorText                             = new Color32(0xD3, 0x22, 0x22, 255),
                LinkText                              = new Color32(0x4C, 0x7E, 0xFF, 255),
                VisitedLinkText                       = new Color32(0xFF, 0x00, 0xFF, 255),
                WarningText                           = new Color32(0xF4, 0xBC, 0x02, 255), //
                HelpboxText                           = new Color32(0xBD, 0xBD, 0xBD, 255),
                HighlightText                         = new Color32(0x4C, 0x7E, 0xFF, 255),
                HighlightTextInactive                 = new Color32(0xFF, 0xFF, 0xFF, 255),
                LabelText                             = new Color32(0xC4, 0xC4, 0xC4, 255),
                LabelTextFocus                        = new Color32(0x81, 0xB4, 0xFF, 255),
                PreviewOverlayText                    = new Color32(0xDE, 0xDE, 0xDE, 255),
                WindowText                            = new Color32(0xBD, 0xBD, 0xBD, 255), //
                TabText                               = new Color32(0xBD, 0xBD, 0xBD, 255),
                ToolbarButtonText                     = new Color32(0xC4, 0xC4, 0xC4, 255),
                ToolbarButtonTextChecked              = new Color32(0xC4, 0xC4, 0xC4, 255),
                ToolbarButtonTextHover                = new Color32(0xBD, 0xBD, 0xBD, 255),
            };

            // @formatter:int_align_assignments restore
        }
    }
}