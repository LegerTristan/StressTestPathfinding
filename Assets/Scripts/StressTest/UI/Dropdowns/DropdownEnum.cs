using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Base class of dropdown widget with options based on a enum.
/// </summary>
/// <typeparam name="TEnum">Enum type contained in the dropdown</typeparam>
public class DropdownEnum<TEnum> : MonoBehaviour where TEnum : Enum
{
    #region F/P
    public event Action<TEnum> OnEnumValueChanged = null;

    [SerializeField]
    TMP_Dropdown dropdown = null;

    /// <summary>
    /// Contains string option in key and binded enum in value.
    /// </summary>
    [SerializeField]
    protected Dictionary<string, TEnum> enumValues = null;

    /// <summary>
    /// Current value selected in the dropdown
    /// </summary>
    public TEnum Value => enumValues[dropdown.options[dropdown.value].text];

    public void SetValue(int _option) => dropdown.SetValueWithoutNotify(_option);

    public void SetInteractable(bool _interactable) => dropdown.interactable = _interactable;
    #endregion

    #region UnityEvents
    void Start() => Init();
    #endregion

    #region CustomMethods
    protected virtual void Init()
    {
        if (!dropdown || enumValues == null)
            return;

        AddEnumOptions();
        dropdown.onValueChanged.AddListener(SetDropdownValue);
    }

    /// <summary>
    /// Fill dropdown options with dictionary enumValues
    /// </summary>
    void AddEnumOptions()
    {
        List<TMP_Dropdown.OptionData> _options = new List<TMP_Dropdown.OptionData>();

        foreach (KeyValuePair<string, TEnum> _algoType in enumValues)
        {
            _options.Add(new TMP_Dropdown.OptionData(_algoType.Key));
        }

        dropdown.AddOptions(_options);
    }

    void SetDropdownValue(int _value)
    {
        string _result = dropdown.options[_value].text;
        OnEnumValueChanged?.Invoke(enumValues[_result]);
    }

    public void SetDropdownVisible(bool _visibility)
    {
        if(dropdown != null)
            dropdown.gameObject.SetActive(_visibility);
    }
    #endregion
}
