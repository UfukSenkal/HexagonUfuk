using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Hexagon
{
    public class HexagonController : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _bombText;


        private int _x;
        private int _y;
        private Color _hexColor;
        private Vector2 hexPos;

        public Vector2 HexPos { get => hexPos; set => hexPos = value; }
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public Color HexColor { get => _hexColor; set => _hexColor = value; }

        private void Awake()
        {
            HexColor = GetComponent<SpriteRenderer>().color;
        }

        private void Update()
        {
            GetComponent<SpriteRenderer>().color = HexColor;
        }

        public void SetColor(Color _color)
        {
            HexColor = _color;
        }

        public void SetBombText(string text)
        {
            _bombText.text = text;
        }
    }

}
