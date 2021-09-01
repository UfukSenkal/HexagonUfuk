using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Hexagon
{
    public class Neighbour : MonoBehaviour
    {

        public float _distance = .3f;

      


        List<GameObject> neighbourUpRight;
        List<GameObject> neighbourUpLeft;
        List<GameObject> neighbourRight;
        List<GameObject> neighbourLeft;
        List<GameObject> neighbourDownRight;
        List<GameObject> neighbourDownLeft;

        List<List<GameObject>> neighbourList;


        public List<List<GameObject>> NeighbourList { get => neighbourList; }

        private void Start()
        {
            neighbourList = new List<List<GameObject>>();
            


            neighbourUpRight = new List<GameObject>();
            neighbourUpLeft = new List<GameObject>();
            neighbourRight = new List<GameObject>();
            neighbourLeft = new List<GameObject>();
            neighbourDownRight = new List<GameObject>();
            neighbourDownLeft = new List<GameObject>();
        }

   


        public void FindNeighbours()
        {
            Physics2D.queriesStartInColliders = false;

            neighbourUpRight.Clear();
            neighbourUpLeft.Clear();
            neighbourRight.Clear();
            neighbourLeft.Clear();
            neighbourDownRight.Clear();
            neighbourDownLeft.Clear();

            neighbourList.Clear();

            RaycastHit2D hitUp = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), _distance);

            RaycastHit2D hitUpRight = Physics2D.Raycast(transform.position, Vector2.up + Vector2.right, _distance);

            RaycastHit2D hitUpLeft = Physics2D.Raycast(transform.position, Vector2.up + Vector2.left, _distance);

            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), _distance);

            RaycastHit2D hitDownRight = Physics2D.Raycast(transform.position, Vector2.down + Vector2.right, _distance);

            RaycastHit2D hitDownLeft = Physics2D.Raycast(transform.position, Vector2.down + Vector2.left, _distance);


            if (hitUp && hitUpRight)
            {
                neighbourUpRight.Add(hitUp.transform.gameObject);
                neighbourUpRight.Add(hitUpRight.transform.gameObject);
                neighbourUpRight.Add(this.gameObject);

                neighbourList.Add(neighbourUpRight);
            }
            if (hitUp && hitUpLeft)
            {
                neighbourUpLeft.Add(hitUp.transform.gameObject);
                neighbourUpLeft.Add(hitUpLeft.transform.gameObject);
                neighbourUpLeft.Add(this.gameObject);

                neighbourList.Add(neighbourUpLeft);
            }
            if (hitUpRight && hitDownRight)
            {
                neighbourRight.Add(hitUpRight.transform.gameObject);
                neighbourRight.Add(hitDownRight.transform.gameObject);
                neighbourRight.Add(this.gameObject);

                neighbourList.Add(neighbourRight);
            }
            if (hitUpLeft && hitDownLeft)
            {
                neighbourLeft.Add(hitDownLeft.transform.gameObject);
                neighbourLeft.Add(hitUpLeft.transform.gameObject);
                neighbourLeft.Add(this.gameObject);

                neighbourList.Add(neighbourLeft);
            }
            if (hitDown && hitDownRight)
            {
                neighbourDownRight.Add(hitDown.transform.gameObject);
                neighbourDownRight.Add(hitDownRight.transform.gameObject);
                neighbourDownRight.Add(this.gameObject);

                neighbourList.Add(neighbourDownRight);
            }
            if (hitDown && hitDownLeft)
            {
                neighbourDownLeft.Add(hitDown.transform.gameObject);
                neighbourDownLeft.Add(hitDownLeft.transform.gameObject);
                neighbourDownLeft.Add(this.gameObject);

                neighbourList.Add(neighbourDownLeft);
            }

            

          
            Physics2D.queriesStartInColliders = true;
            
        }


        public List<GameObject> GetClosestNeighbours(Vector2 mousePos)
        {

            FindNeighbours();

            float distance = 10f;
            Vector3 tempPos = Vector3.zero;

            List<GameObject> closestNeighbours = new List<GameObject>();

            foreach (var list in neighbourList)
            {
                foreach (var neighbour in list)
                {
                    tempPos += neighbour.transform.position;
                }
                tempPos /= 3;

                if (Vector3.Distance(mousePos, tempPos) < distance)
                {
                    distance = Vector2.Distance(mousePos, tempPos);
                    closestNeighbours = list;
                }
                tempPos = Vector3.zero;
            }
            

            return closestNeighbours;
        }

       

    }



}

