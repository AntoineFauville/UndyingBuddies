using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColor : MonoBehaviour
{
    [SerializeField] private Color _colorInitial;
    [SerializeField] private Color _colorHighlighted;

    [SerializeField] private Image _image;

    private bool _changeColor;

    void Start()
    {
        _image.color = _colorInitial;
    }
    
    public void SetColorToInitial()
    {
        _changeColor = false;
        _image.color = _colorInitial;
    }

    public void SetColorHighlighted()
    {
        _changeColor = true;
        _image.color = _colorHighlighted;
    }

    void Update()
    {
        if (!_changeColor)
        {
            _image.color = _colorInitial;
        }
    }
}
