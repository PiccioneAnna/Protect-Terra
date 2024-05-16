using CreatureClasses;
using System;
using UnityEngine;

// Class handles movement, actions, and main references to other systems
namespace Player
{
    public class AnimManager : MonoBehaviour
    {
        #region Fields
        [HideInInspector] public Animator frontAnim;
        [HideInInspector] public Animator sideAnim;
        [HideInInspector] public Animator backAnim;

        private Animator activeAnim;

        public GameObject front;
        public GameObject back;
        public GameObject side;

        [HideInInspector] public bool flipX;
        [HideInInspector] public Direction direction;
        [HideInInspector] public Vector2 dirValue;

        #endregion


        // Start is called before the first frame update
        void Awake()
        {
            frontAnim = front.GetComponent<Animator>();
            sideAnim = side.GetComponent<Animator>();
            backAnim = back.GetComponent<Animator>();

            activeAnim = frontAnim;
        }

        // Update is called once per frame
        public void UpdateAnimation()
        {
            CalculateDirection();
            HandleAnimatorDirection();
            UpdateMotion();
        }

        #region Helper Methods
        private void HandleAnimatorDirection()
        {
            switch (direction)
            {
                case Direction.North:
                    front.SetActive(true);
                    back.SetActive(false);
                    side.SetActive(false);
                    activeAnim = frontAnim;
                    break;
                case Direction.South:
                    front.SetActive(false);
                    back.SetActive(true);
                    side.SetActive(false);
                    activeAnim = backAnim;
                    break;
                case Direction.East:
                    front.SetActive(false);
                    back.SetActive(false);
                    side.SetActive(true);
                    activeAnim = sideAnim;
                    break;
                case Direction.West:
                    front.SetActive(false);
                    back.SetActive(false);
                    side.SetActive(true);
                    activeAnim = sideAnim;
                    break;
                default:
                    break;
            }
        }

        private void CalculateDirection()
        {
            if (Math.Abs(dirValue.x) > Math.Abs(dirValue.y))
            {
                if (dirValue.x > 0)
                {
                    side.GetComponent<SpriteRenderer>().flipX = true;
                    direction = Direction.West;
                }
                else if (dirValue.x < 0)
                {
                    side.GetComponent<SpriteRenderer>().flipX = false;
                    direction = Direction.East;
                }
            }
            else
            {
                if (dirValue.y > 0)
                {
                    direction = Direction.South;
                }
                else if (dirValue.y < 0)
                {
                    direction = Direction.North;
                }
            }
        }

        private void UpdateMotion()
        {
            if (Math.Abs(dirValue.x) > 0 || Math.Abs(dirValue.y) > 0) { HandleWalking(activeAnim, true); }
            else { HandleWalking(activeAnim, false); }
        }

        private void HandleWalking(Animator anim, bool val)
        {
            anim.SetBool("IsWalking", val);
        }
        #endregion
    }
}