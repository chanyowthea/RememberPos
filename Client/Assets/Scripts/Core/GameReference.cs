using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameReference : MonoBehaviour
{
	[SerializeField] public GameObject _mainCam;
	[SerializeField] public GameObject _sceneRoot;
	[SerializeField] public Transform _playerParent;
	[SerializeField] public Block _blockPrefab; 
	[SerializeField] public GameObject _rotateItem; 
	[SerializeField] public Transform _blockParent; 
	[SerializeField] public BlockLibrary _blockLib; 
	[SerializeField] public BoardLibrary _boardLib;
	[SerializeField] public BlockBoardLibrary _blockBoardLib; 
	[SerializeField] public Transform _boardPivot_Mine; 
	[SerializeField] public Transform _boardPivot_Opposite;
	[SerializeField] public Player _playerPrefab;
	[SerializeField] public PersonLibrary _personLib; 
	[SerializeField] public Transform _playerPos_Mine;
	[SerializeField] public Transform _playerPos_Opposite;
}
