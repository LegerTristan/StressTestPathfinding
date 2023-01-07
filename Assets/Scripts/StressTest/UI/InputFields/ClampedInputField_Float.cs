using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Input field that handle float values.
/// Value is clamped between a min and max and have a default value.
/// </summary>
public class ClampedInputField_Float : MonoBehaviour
{
    #region F/P
    public event Action<float> OnInputValueChanged = null;

    [SerializeField, Header("UI")]
    TMP_InputField input = null;

    [SerializeField, Header("Clamp"), Range(0.1f, 100f)]
    float minValue = 1f;

    [SerializeField, Range(0.1f, 100f)]
    float defaultValue = 50f, maxValue = 100f;

    /// <summary>
    /// Return input field text parsed as float.
    /// </summary>
    public float InputValue => float.Parse(input.text);
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
    /// Check text in input field, if text can be parsed as float, then value is edited.
    /// Else, set default value instead.
    /// </summary>
    /// <param name="_text">Input field text</param>
    void SetInput(string _text)
    {
        float _result;
        bool _isValid = float.TryParse(_text, out _result);

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
