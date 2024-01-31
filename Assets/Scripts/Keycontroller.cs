//--------------------------------------------------
// Keycontroller.cs
// �쐬��12/01
// �쐬�� �k�\�V��
using UnityEngine;
using UnityEngine.SceneManagement;
internal class Keycontroller : MonoBehaviour
{   

    private Vector2Int _vectorPlayer = new Vector2Int(-3, 0);
    //player�𐶐�
    Vector2Int _player = new Vector2Int(3, 4);
    //�v���n�u����I�u�W�F�N�g���擾
    private GameObject _Resource;
    //��
    [SerializeField] GameObject _objectNoblock = default;
    //��
    [SerializeField] GameObject _objectstatic = default;
    //�ו�
    [SerializeField] GameObject _objectmove = default;
    //�v���C���[
    [SerializeField] GameObject _objectplay = default;
    //�i�[�ꏊ
    [SerializeField] GameObject _objecttarget = default;
�@�@//�v���C���[�̏����ʒu
    private Vector2Int _playMovement = new Vector2Int(5, 5);
�@�@//�v���C���[�̏��
    private GameObject _playerObject = default;
    //���̃I�u�W�F�N�g
    [SerializeField] Sprite _noBlockSprite = default;
    //�ǂ̃I�u�W�F�N�g
    [SerializeField] Sprite _staticSprite = default;
    //�ו��̃I�u�W�F�N�g
    [SerializeField] Sprite _moveBlockSprite = default;

    //�����i�[����ϐ�
    private SpriteRenderer[,] _stageSprite = new SpriteRenderer[8,8];

    private string _horizontal = "Horizontal";
    private string _vertical = "Vertical";

    enum Way : int
    {
        //��
        noblock = 0,
        //�����Ȃ��u���b�N
        staticblock = 1,
        //�������u���b�N
        moveblock = 2,

    }
    //�v���C���[���W�A(2,4)

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

    //�i�[�ꏊ //�}���ʒu
    private Vector2Int[] _targetBlock = new Vector2Int[]
    {
        new Vector2Int(1,4),
        new Vector2Int(2,1),
        new Vector2Int(4,4),
        new Vector2Int(6,6),
    };

    //�Q�[���̑���
    //�㉺����
    //���E����
    //����
    //�ʒu�X�V
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
                    case (int)Way.noblock://���u���b�N���擾
                        inStage = Instantiate
                            (_objectNoblock, new Vector3(i, r, 0), Quaternion.identity);
                        _stageSprite[i, r] = inStage.GetComponent<SpriteRenderer>();
                        break;
                    case (int)Way.staticblock://�ǃu���b�N���擾
                        inStage = Instantiate
                            (_objectstatic, new Vector3(i, r, 0), Quaternion.identity);
                        _stageSprite[i, r] = inStage.GetComponent<SpriteRenderer>();
                        break;
                    case (int)Way.moveblock://�ו��u���b�N���擾
                        inStage = Instantiate
                            (_objectmove, new Vector3(i, r, 0), Quaternion.identity);
                        _stageSprite[i, r] = inStage.GetComponent<SpriteRenderer>();
                        break;
                }
            }
        }
        //�i�[�ꏊ�𐶐�
        for (int i = 0; i < _targetBlock.GetLength(0); i++)
        {
            Vector2 clearJudge = _targetBlock[i];
            Instantiate(_objecttarget, clearJudge, Quaternion.identity);
        }
        //�v���C���[�𐶐�
        Vector2 insMove = _playMovement;
         _playerObject = Instantiate(_objectplay, insMove, Quaternion.identity);
        
    }
    private void Update()
    {
        //�v���C���[�̓��͏����i�c�j
        float x = Input.GetAxisRaw(_horizontal);
        //�v���C���[�̓��͏����i���j
        float y = Input.GetAxisRaw(_vertical);
        //�O���̏����@���͏���
        //�����̏����@�ړ�����
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
        //�X�e�[�W�𐶐�
        for (int i = 0; i < _2array.GetLength(0); i++)
        {
            for (int r = 0; r < _2array.GetLength(1); r++)
            {
                switch (_2array[i, r])
                {
                    case (int)Way.noblock://���u���b�N�𐶐�
                        _stageSprite[i, r].sprite = _noBlockSprite;

                        break;

                    case (int)Way.staticblock://�ǃu���b�N�𐶐�
                        _stageSprite[i, r].sprite = _staticSprite;
                        break;

                    case (int)Way.moveblock://�ו��u���b�N�𐶐�
                        _stageSprite[i, r].sprite = _moveBlockSprite;
                        break;
                }
            }
        }
        //�l�񂾎��ɏ����Ƀ��Z�b�g���鏈��
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("Main");
        }
        Clear();
    }

    /// <summary>
    /// �v���C���[�̔���
    /// </summary>
    /// <param name="vectorJudge"></param>
    /// <returns></returns>
    private bool _moveJudge(Vector2Int vectorJudge)
    {
        //�u���b�N�𓮂����Ă鎞�̈ړ�����@//�������u���b�N��2�悪�����Ȃ������ꍇ
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
        //�������u���b�N�̂Q�悪�����������ꍇ
        if (_2array[_playMovement.x + vectorJudge.x,
            _playMovement.y + vectorJudge.y] ==
        (int)Way.moveblock && _2array[_playMovement.x + vectorJudge.x * 2,
        _playMovement.y + vectorJudge.y * 2] != (int)Way.noblock)
        {
            return false;
        }
        //�v���C���[�̂P�悪�ǂ������ꍇ
        if (_2array[_playMovement.x + vectorJudge.x, 
            _playMovement.y + vectorJudge.y] == (int)Way.staticblock)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// �Q�[���N���A�̔���
    /// </summary>
    /// <returns>�Q�[���N���A�̗L��</returns>
    private void Clear()
    {
        //�N���A�����̃��[�v��
        for (int i = 0; i < _targetBlock.Length; i++)
        {
            //�S�ē����ĂȂ��������̏���
            if ((int)Way.moveblock!=
                _2array [_targetBlock[i].x,_targetBlock[i].y])
            {
                return;
            }
        }
        //�N���A�V�[���֑J��
        SceneManager.LoadScene("Clear");
    } 
}