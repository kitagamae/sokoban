//--------------------------------------------------
// Keycontroller.cs
// 作成日12/01
// 作成者 北構天哉
using UnityEngine;
using UnityEngine.SceneManagement;
internal class Keycontroller : MonoBehaviour
{   

    private Vector2Int _vectorPlayer = new Vector2Int(-3, 0);
    //playerを生成
    Vector2Int _player = new Vector2Int(3, 4);
    //プレハブからオブジェクトを取得
    private GameObject _Resource;
    //無
    [SerializeField] GameObject _objectNoblock = default;
    //壁
    [SerializeField] GameObject _objectstatic = default;
    //荷物
    [SerializeField] GameObject _objectmove = default;
    //プレイヤー
    [SerializeField] GameObject _objectplay = default;
    //格納場所
    [SerializeField] GameObject _objecttarget = default;
　　//プレイヤーの初期位置
    private Vector2Int _playMovement = new Vector2Int(5, 5);
　　//プレイヤーの情報
    private GameObject _playerObject = default;
    //無のオブジェクト
    [SerializeField] Sprite _noBlockSprite = default;
    //壁のオブジェクト
    [SerializeField] Sprite _staticSprite = default;
    //荷物のオブジェクト
    [SerializeField] Sprite _moveBlockSprite = default;

    //情報を格納する変数
    private SpriteRenderer[,] _stageSprite = new SpriteRenderer[8,8];

    private string _horizontal = "Horizontal";
    private string _vertical = "Vertical";

    enum Way : int
    {
        //無
        noblock = 0,
        //動かないブロック
        staticblock = 1,
        //動かすブロック
        moveblock = 2,

    }
    //プレイヤー座標、(2,4)

    private int[,] _2array =
    {
        {1, 1, 1, 1, 1, 1, 1, 1,},
        {1, 0, 0, 0, 0, 0, 0, 1,},
        {1, 0, 0, 2, 0, 0, 0, 1,},
        {1, 0, 0, 0, 0, 2, 0, 1,},
        {1, 0, 0, 0, 0, 0, 0, 1,},
        {1, 0, 2, 0, 2, 0, 0, 1,},
        {1, 0, 0, 0, 2, 0, 0, 1,},
        {1, 1, 1, 1, 1, 1, 1, 1,},
    };
    private bool _isMove = false;

    //格納場所 //挿入位置
    private Vector2Int[] _targetBlock = new Vector2Int[]
    {
        new Vector2Int(1,4),
        new Vector2Int(2,1),
        new Vector2Int(4,4),
        new Vector2Int(6,6),
    };

    //ゲームの操作
    //上下処理
    //左右処理
    //生成
    //位置更新
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        GameObject inStage = default;

        for (int i = 0; i < _2array.GetLength(0); i++)
        {
            for (int r = 0; r < _2array.GetLength(1); r++)
            {
                switch (_2array[i, r])
                {
                    case (int)Way.noblock://無ブロックを取得
                        inStage = Instantiate
                            (_objectNoblock, new Vector3(i, r, 0), Quaternion.identity);
                        _stageSprite[i, r] = inStage.GetComponent<SpriteRenderer>();
                        break;
                    case (int)Way.staticblock://壁ブロックを取得
                        inStage = Instantiate
                            (_objectstatic, new Vector3(i, r, 0), Quaternion.identity);
                        _stageSprite[i, r] = inStage.GetComponent<SpriteRenderer>();
                        break;
                    case (int)Way.moveblock://荷物ブロックを取得
                        inStage = Instantiate
                            (_objectmove, new Vector3(i, r, 0), Quaternion.identity);
                        _stageSprite[i, r] = inStage.GetComponent<SpriteRenderer>();
                        break;
                }
            }
        }
        //格納場所を生成
        for (int i = 0; i < _targetBlock.GetLength(0); i++)
        {
            Vector2 clearJudge = _targetBlock[i];
            Instantiate(_objecttarget, clearJudge, Quaternion.identity);
        }
        //プレイヤーを生成
        Vector2 insMove = _playMovement;
         _playerObject = Instantiate(_objectplay, insMove, Quaternion.identity);
        
    }
    private void Update()
    {
        //プレイヤーの入力処理（縦）
        float x = Input.GetAxisRaw(_horizontal);
        //プレイヤーの入力処理（横）
        float y = Input.GetAxisRaw(_vertical);
        //外側の条件　入力処理
        //内側の条件　移動処理
        if (x < 0 && !_isMove)
        {
            if (_moveJudge(Vector2Int.left))
            {
                _playerObject.transform.position += Vector3.left;
                _playMovement += Vector2Int.left;
            }
            _isMove = true;
        }
       else if (x > 0 && !_isMove)
        {
            if (_moveJudge(Vector2Int.right))
            {
                _playerObject.transform.position += Vector3.right;
                _playMovement += Vector2Int.right;
            }
            _isMove = true;
        }
        if (y > 0 && !_isMove)
        {
            if (_moveJudge(Vector2Int.up))
            {
                _playerObject.transform.position += Vector3.up;
                _playMovement += Vector2Int.up;
            }
            _isMove = true;
        }
       else if (y < 0 && !_isMove)
        {
            if (_moveJudge(Vector2Int.down))
            {
                _playerObject.transform.position += Vector3.down;
                _playMovement += Vector2Int.down;
            }
            _isMove = true;
        }
        if (x == 0 && y == 0)
        {    
            _isMove = false;
        }
        //ステージを生成
        for (int i = 0; i < _2array.GetLength(0); i++)
        {
            for (int r = 0; r < _2array.GetLength(1); r++)
            {
                switch (_2array[i, r])
                {
                    case (int)Way.noblock://無ブロックを生成
                        _stageSprite[i, r].sprite = _noBlockSprite;

                        break;

                    case (int)Way.staticblock://壁ブロックを生成
                        _stageSprite[i, r].sprite = _staticSprite;
                        break;

                    case (int)Way.moveblock://荷物ブロックを生成
                        _stageSprite[i, r].sprite = _moveBlockSprite;
                        break;
                }
            }
        }
        //詰んだ時に初期にリセットする処理
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("Main");
        }
        Clear();
    }

    /// <summary>
    /// プレイヤーの判定
    /// </summary>
    /// <param name="vectorJudge"></param>
    /// <returns></returns>
    private bool _moveJudge(Vector2Int vectorJudge)
    {
        //ブロックを動かしてる時の移動判定　//動かすブロックの2個先が何もなかった場合
        if (_2array[_playMovement.x + vectorJudge.x,
            _playMovement.y + vectorJudge.y] ==
        (int)Way.moveblock && _2array[_playMovement.x + (vectorJudge.x * 2),
        _playMovement.y + (vectorJudge.y * 2)] == (int)Way.noblock)
        {
            _2array[_playMovement.x + (vectorJudge.x * 2),
                _playMovement.y + (vectorJudge.y * 2)] = (int)Way.moveblock;
            _2array[_playMovement.x + vectorJudge.x,
                _playMovement.y + vectorJudge.y] = (int)Way.noblock;
            return true;
        }
        //動かすブロックの２個先が何かあった場合
        if (_2array[_playMovement.x + vectorJudge.x,
            _playMovement.y + vectorJudge.y] ==
        (int)Way.moveblock && _2array[_playMovement.x + vectorJudge.x * 2,
        _playMovement.y + vectorJudge.y * 2] != (int)Way.noblock)
        {
            return false;
        }
        //プレイヤーの１個先が壁だった場合
        if (_2array[_playMovement.x + vectorJudge.x, 
            _playMovement.y + vectorJudge.y] == (int)Way.staticblock)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// ゲームクリアの判定
    /// </summary>
    /// <returns>ゲームクリアの有無</returns>
    private void Clear()
    {
        //クリア条件のループ文
        for (int i = 0; i < _targetBlock.Length; i++)
        {
            //全て入ってなかった時の条件
            if ((int)Way.moveblock!=
                _2array [_targetBlock[i].x,_targetBlock[i].y])
            {
                return;
            }
        }
        //クリアシーンへ遷移
        SceneManager.LoadScene("Clear");
    } 
}