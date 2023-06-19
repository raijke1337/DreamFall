using UnityEngine;
namespace Game
{
    public class TunnelController : MonoBehaviour
    {
        [SerializeField] private SectorComponent _sectorPrefab;
        private int totalPlaced = 0;

        public void Generate(int initialSectors = 10)
        {
            for (int i = 0; i < initialSectors; i++)
            {
                var sec = Instantiate(_sectorPrefab);
                sec.transform.position = Vector3.zero+(Vector3.forward * sec.length * totalPlaced);
                sec.InitSector();
                totalPlaced++;
                sec.transform.parent = transform;
            }
        }

        public void OnSectorCleared(SectorComponent sect)
        {
            Generate(1);
            sect.CloseSector();
        }


    }
}