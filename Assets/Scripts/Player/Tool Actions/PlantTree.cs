using Data;
using TilemapScripts;
using ToolActions;
using UnityEngine;

namespace ToolAction
{
    [CreateAssetMenu(menuName = "Data/Tool Action/Plant Tree")]
    public class PlantTree : Base
    {
        GameObject spawnedTree;

        public override bool OnApplyToTileMap(Vector3Int gridPosition, Reader tilemapReadController, Item item)
        {
            if (!item.treeN) { return false; }

            spawnedTree = GameManager.Instance.enviroManager.enviroSpawner.NewTreeObject(item.treeN, gridPosition);

            if (spawnedTree == null) {  return false; }

            GameManager.Instance.inventory.RemoveItem(item);

            return true;
        }
    }
}


