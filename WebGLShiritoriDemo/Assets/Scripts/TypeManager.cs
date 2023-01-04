using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TypeManager : MonoBehaviour
{
    #region CardPanel
    [SerializeField] GameObject _charTextPanel;     // 一文字あたりのパネル
    [SerializeField] GameObject _parentTextPanel;   // 文字列を表示するパネル
    [SerializeField] GameObject _canvas;            // Canvas自体
    private int _lineCount = 1;                     // 現在の行番号
    private int _currentCardNumPerLine;             // 現在行におけるカード個数
    [SerializeField] int _maxCardCountPerLine = 7;  // 1行で表示可能なカードの個数
    [SerializeField] float _offsetX = 500.0f;       // カードオフセット値（X）
    [SerializeField] float _offsetY = 100.0f;       // カードオフセット値（Y）
    #endregion

    #region TextField
    string concatinatedString = ""; // 結合した文字列
    [SerializeField] Text text;
    #endregion

    void Start()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void Update()
    {
        // Enterキー押下
        if (Input.GetKeyUp(KeyCode.Return))
        {
            // 文字列を結合し、テキストボックスに表示する。
            text.text = concatinatedString;
            // リセットする。
            Reset();
        }
    }

    #region Events
    /// <summary>
    /// 任意のキーが押された時に呼ばれる。
    /// </summary>
    /// <param name="c"></param>
    void OnTextInput(char c)
    {
        // Enterキー押下時はカード生成しない
        if (Keyboard.current.enterKey.isPressed)
        {
            return;
        }

        // 必要に応じて改行
        if (_currentCardNumPerLine % _maxCardCountPerLine == 0)
        {
            _lineCount++;
            _currentCardNumPerLine = 0;
        }
        _currentCardNumPerLine++;

        // カード生成
        GenerateCard(c);
        // 文字列結合
        concatinatedString += c;
    }

    /// <summary>
    /// カードを生成する。
    /// </summary>
    /// <param name="c"></param>
    void GenerateCard(char c)
    {
        // 文字カードを生成する位置を決定
        var posX = GetPanelWidth() * 1.1f * _currentCardNumPerLine + _offsetX;// 1.1にすることで少し隙間を作る
        var posY = _offsetY - (_lineCount * GetPanelHeight());

        // カードを生成
        var obj = Instantiate(_charTextPanel, _parentTextPanel.transform);
        obj.transform.position = (new Vector2(posX, posY));
        obj.GetComponentInChildren<Text>().text = c.ToString();
    }
    #endregion


    /// <summary>
    /// リセット処理を行う。
    /// </summary>
    private void Reset()
    {
        concatinatedString = "";
        _currentCardNumPerLine = 0;
        _lineCount = 1;
        DestroyCards(_parentTextPanel);
    }

    /// <summary>
    /// 生成したカードを全削除する。
    /// </summary>
    /// <param name="parent"></param>
    private void DestroyCards(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnDestroy()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }


    #region Util
    /// <summary>
    /// パネルの幅を取得する。
    /// </summary>
    /// <returns></returns>
    float GetPanelWidth()
    {
        return _charTextPanel.GetComponent<RectTransform>().rect.width;
    }
    /// <summary>
    /// パネルの高さを取得する。
    /// </summary>
    /// <returns></returns>
    float GetPanelHeight()
    {
        return _charTextPanel.GetComponent<RectTransform>().rect.height;
    }
    #endregion
}