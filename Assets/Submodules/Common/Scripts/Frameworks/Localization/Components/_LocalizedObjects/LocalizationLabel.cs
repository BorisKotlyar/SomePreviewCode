using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Zenject;

namespace Localization
{
	public class LocalizationLabel : AbstractLocalizedObjects
	{
		[Inject] private readonly ILocalizationController _localizationController;
		
		[SerializeField] protected List<TextMeshPro> _labels = new List<TextMeshPro>();
		[SerializeField] protected string _key;
		[SerializeField] protected List<CustomTextMeshProParams> _customParams = new List<CustomTextMeshProParams>();
		[SerializeField] protected bool _isArabicAlligment;

		protected List<TextAlignmentOptions> _cachedAlligments = new List<TextAlignmentOptions>();
		protected CustomTextMeshProParams _defaultParams;

	    public Func<string, string> FormatFunc = s => s;

		protected virtual void Awake()
		{
			_defaultParams =
				new CustomTextMeshProParams("default", !_labels.IsNullOrEmpty() ? _labels[0].fontSizeMax : 0.0f);
		}

		protected override void Refresh(string languageCode)
		{
			var text = _localizationController.GetText(_key);
		    text = FormatFunc(text);

			if (languageCode == "ar")
			{
			    // TODO:[BORIS] add arabic support
				//_labels.ForEach(a => a.isRightToLeftText = true);
				//var txt = ArabicSupport.ArabicFixer.Fix(text, false, false);
				//_labels.ForEach(a => a.text = txt.ArabicReverseText());

				//if (_isArabicAlligment)
				//{
				//	_labels.ForEach(a => a.alignment = (TextAlignmentOptions) Enum.Parse(typeof(TextAlignmentOptions),
				//		a.alignment.ToString().Replace("Left", "Right")));
				//}
			}
			else
			{
				_labels.ForEach(a => a.isRightToLeftText = false);
				_labels.ForEach(a => a.text = text);

				if (_isArabicAlligment)
				{
					_labels.ForEach(a => a.alignment = (TextAlignmentOptions) Enum.Parse(typeof(TextAlignmentOptions),
						a.alignment.ToString().Replace("Right", "Left")));
				}
			}

			ParamsApplying(_customParams.FirstOrDefault(a => a.LocalizationCode == languageCode) ?? _defaultParams);
		}

		protected void ParamsApplying(CustomTextMeshProParams customParams)
		{
			_labels.ForEach(a => a.fontSizeMax = customParams.FontAutoSizeMax);
		}

		[ContextMenu("add all child text mesh")]
		private void AddAllChildTextMesh()
		{
			_labels = GetComponentsInChildren<TextMeshPro>().ToList();
		}
	}
}