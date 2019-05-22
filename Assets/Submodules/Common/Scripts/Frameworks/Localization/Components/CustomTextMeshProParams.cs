using System;

namespace Localization
{
    [Serializable]
    public class CustomTextMeshProParams
    {
        public string LocalizationCode;
        public float FontAutoSizeMax;

        public CustomTextMeshProParams(string localizationCode, float fontAutoSizeMax)
        {
            LocalizationCode = localizationCode;
            FontAutoSizeMax = fontAutoSizeMax;
        }
    }
}