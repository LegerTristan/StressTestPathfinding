using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Input field that handle int values.
/// Value is clamped between a min and max and have a default value.
/// </summary>
public class ClampedInputField_Int : MonoBehaviour
{
    #region F/P
    public event Action<int> OnInputValueChanged = null;

    [SerializeField, Header("UI")]
    TMP_InputField input = null;

    [SerializeField, Header("Clamp"), Range(1, 100)]
    int minValue = 1;

    [SerializeField, Range(1, 100)]
    int defaultValue = 50, maxValue = 100;

    /// <summary>
    /// Return input field text parsed as int.
    /// </summary>
    public int InputValue => int.Parse(input.text);

    /// <summary>
    /// Set input field text to min value.
    /// </summary>
    public void SetToMinValue() => input.text = minValue.ToString();

    public void SetMaxValue(int _maxValue) => maxValue = _maxValue;

    public void SetMinValue(int _minValue) => minValue = _minValue;

    public void SetInteractable(bool _interactable) => input.interactable = _interactable;
    #endregion

    #region UnityEvents
    void Start() => Init();
    #endregion

    #region CustomMethods
    public void Init()
    {
        if (!input)
            return;

        input.text = defaultValue.ToString();
        input.onEndEdit.AddListener(SetInput);
    }

    /// <summary>
    /// Check text in input field, if text can be parsed as int, then value is edited.
    /// Else, set default value instead.
    /// </summary>
    /// <param name="_text">Input field text</param>
    void SetInput(string _text)
    {
        int _result;
        bool _isValid = int.TryParse(_text, out _result);

        if (_isValid)
        {
            _result = _result < minValue ? minValue : _result;
            _result = _result > maxValue ? maxValue : _result;
            input.text = _result.ToString();
            OnInputValueChanged?.Invoke(_result);
        }
        else
            input.text = defaultValue.ToString();
    }
    #endregion
}
