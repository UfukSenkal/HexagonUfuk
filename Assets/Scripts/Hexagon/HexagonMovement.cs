using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hexagon
{
    public class HexagonMovement : MonoBehaviour
    {

        public float speed = 2f;
        public Vector3 lerpPos;


     

        void Update()
        {
            if (IsMoving())
            {
                MoveToPos();
            }
        }


        public void ChangePos(Vector3 newPos)
        {
            lerpPos = newPos;
        }

        public bool IsMoving()
        {
            if (transform.position != lerpPos)
            {
                return true;
            }

            return false;
        }

        public void MoveToPos()
        {
            transform.position = Vector3.Lerp(transform.position, lerpPos, Time.deltaTime * speed);
            
        }
    }

}

