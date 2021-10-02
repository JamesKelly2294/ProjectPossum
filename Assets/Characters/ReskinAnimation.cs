using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReskinAnimation : MonoBehaviour
{
    public string spriteSheetName;

    private SpriteRenderer _sr;
    private string _spriteSheetName;
    private Sprite[] _subsprites;

    private void Start()
    {

        _sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (_spriteSheetName != spriteSheetName)
        {
            _subsprites = Resources.LoadAll<Sprite>(spriteSheetName);
            _spriteSheetName = spriteSheetName;
        }

        if (_sr)
        {
            var spriteFrameName = _sr.sprite.name;
            int pos = spriteFrameName.LastIndexOf("_") + 1;
            string suffix = spriteFrameName.Substring(pos, spriteFrameName.Length - pos);

            try
            {
                int result = int.Parse(suffix);
                if (result >= 0 && result < _subsprites.Length)
                {
                    _sr.sprite = _subsprites[result];
                }
            }
            catch (FormatException)
            {
                Debug.LogError("Unable to parse sprite number");
            }
        }
    }
}
