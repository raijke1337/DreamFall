using Game.Extras;
using UnityEngine;

namespace Game
{
    public class SectorComponent : MonoBehaviour
    {

        public BaseItem[] Items;
        [SerializeField] private Collider _end;

        [SerializeField] SpawnRange _spawnRange;

        public float length; // hardcode but w/e

        // colliders are destroyed to ensure proper controls
        public void InitSector()
        {
            var direction = Random.onUnitSphere;
            var d = direction * Random.Range(_spawnRange.MinRange, _spawnRange.MaxRange);

            var spawn = new Vector3(d.x, d.y, transform.position.z);

            var i = Instantiate(Items[Random.Range(0,Items.Length)], spawn, Random.rotation);

            i.transform.parent = transform;
        
        }
        public void CloseSector()
        {
            _end.enabled = false;
        }
    }


}