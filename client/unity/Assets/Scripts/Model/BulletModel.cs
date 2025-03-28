using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCity
{
    public class BulletModel
    {
        public int Id { get; set; }

        public Position BulletPosition{ get; set; }

        public GameObject BulletObject { get; set; }

        public BulletModel(int id, Position bulletPosition, int speed, int damage, bool isMissile = false, bool isAntiArmor = false, float traveledDistance = 0)
        {
            GameObject wallController = GameObject.Find("WallController");
            Id = id;
            GameObject prefab;

            //TODO: different speed and damage
            if (isMissile)
            {
                prefab = Resources.Load<GameObject>($"Model/Bullet/Bullet");
            }
            else
            {
                prefab = Resources.Load<GameObject>($"Model/Bullet/Bullet");
            }
            if(isAntiArmor)
            {
                //TODO
            }
            

            if (prefab != null)
            {
                Vector3 position = new Vector3((float)bulletPosition.X, (float)(bulletPosition.Y + 0.2), (float)bulletPosition.Z);

                Quaternion rotation = Quaternion.Euler(0, (float)bulletPosition.Angle, 0); // ���� Y ����ת

                BulletObject = Object.Instantiate(prefab, wallController.transform);
                BulletObject.transform.localPosition = position;
                BulletObject.transform.localRotation = rotation;
            }
            else
            {
                Debug.LogError($"Bullet model not found in Resources/Model/Bullet");
            }
        }

        public void UpdateBulletPosition(Position bulletPosition)
        {
            BulletPosition = bulletPosition;

            if (BulletObject != null)
            {
                Vector3 targetPosition = new Vector3((float)bulletPosition.X, (float)bulletPosition.Y, (float)bulletPosition.Z);
                BulletObject.transform.localPosition = Vector3.Lerp(BulletObject.transform.localPosition, targetPosition, 10 * Time.deltaTime);

                Quaternion targetRotation = Quaternion.Euler(0, (float)bulletPosition.Angle, 0); // ���� Y ����ת
                BulletObject.transform.localRotation = Quaternion.RotateTowards(BulletObject.transform.localRotation, targetRotation, 10 * Time.deltaTime);
            }
            else
            {
                Debug.LogWarning($"BulletObject is null for bullet with id {Id}. Cannot update position.");
            }
        }

        public void UpdateBulletPosition(float x, float y, float angle)
        {
            Position position = new Position(x, y, angle);
            UpdateBulletPosition(position);
        }

        public void SelfDestruct()
        {
            if (BulletObject != null)
            {
                Object.Destroy(BulletObject);
                BulletObject = null;
            }
        }
    }
}

