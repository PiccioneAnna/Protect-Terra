using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.RuleTile.TilingRuleOutput;
using Player;
public enum ResourceType
{
    Undefined,
    Tree,
    Mineral,
    Grass,
    Bush
}

namespace ToolActions
{

    // Base class for any tools that 'hit'
    public class ToolHit : MonoBehaviour
    {
        public virtual void Hit() { }

        public virtual bool CanBeHit(List<ResourceType> canBeHit)
        {
            return true;
        }
    }

    [CreateAssetMenu(menuName = "Data/Tool Action/Enviro Hit")]
    public class EnviroHit : Base
    {
        [SerializeField] float sizeOfInteractableArea = 2;
        [SerializeField] float grassCap = 5;
        [SerializeField] List<ResourceType> canHitNodesOfType;

        private Vector3 centerPoint;
        private Controller player;

        readonly int layerMaskInt = 1 << 0; // Default layer?

        public override bool OnApply(Vector2 worldPoint)
        {
            player = GameManager.Instance.player;
            centerPoint = player.weaponPoint.worldPos;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(centerPoint, sizeOfInteractableArea, layerMaskInt);

            //sort colliders by distance - closest should be checked first
            colliders = colliders.OrderBy((d) => (d.bounds.center - centerPoint).sqrMagnitude).ToArray();

            if (player.character.isExhausted) { return false; }

            int index = 0;

            foreach (Collider2D c in colliders)
            {     
                if(c.gameObject.name == "Player") { continue; }

                // check prevents disabled objects from being interacted with
                if (c.gameObject.layer == LayerMask.NameToLayer("Tilemap")) { continue; }
                if (c.gameObject.layer == LayerMask.NameToLayer("Border")) { break; }

                Debug.Log($"Collider found...{c.gameObject.name}");

                var hit = c.gameObject.GetComponent<Resource>() != null ? c.gameObject.GetComponent<Resource>() : c.gameObject.GetComponentInParent<Resource>();

                Debug.Log(hit);

                if (hit)
                {
                    Debug.Log("Resource Found: " + hit.nodeType);

                    if (canHitNodesOfType.Contains(hit.nodeType))
                    {
                        Debug.Log("Enviro Hit");
                        hit.Hit();

                        if(hit.nodeType == ResourceType.Grass)
                        {
                            index++;
                            if(index >= grassCap)
                            {
                                player.canDoAction = false;
                                return true;
                            }
                        }
                        else
                        {
                            player.canDoAction = false;
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}


