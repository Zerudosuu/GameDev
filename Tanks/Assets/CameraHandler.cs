using UnityEngine;

namespace Complete
{
    public class CameraHandler : MonoBehaviour
    {
        [Header("Player One")]
        private GameObject Tank1;
        public Camera player1Camera;
        public TankMovement tankMovement1;
        public TankHealth tankHealth1;

        [Header("Player Two")]
        private GameObject Tank2;
        public Camera player2Camera;
        public TankMovement tankMovement2;
        public TankHealth tankHealth2;

        void Start()
        {
            Tank1 = GameObject.FindGameObjectWithTag("Player1");
            player1Camera = Tank1.GetComponentInChildren<Camera>();
            tankMovement1 = Tank1.GetComponentInParent<TankMovement>();
            tankHealth1 = Tank1.GetComponentInParent<TankHealth>();

            Tank2 = GameObject.FindGameObjectWithTag("Player2");
            player2Camera = Tank2.GetComponentInChildren<Camera>();
            tankMovement2 = Tank2.GetComponentInParent<TankMovement>();
            tankHealth2 = Tank2.GetComponentInParent<TankHealth>();
        }

        void Update()
        {
            if (tankMovement1.m_PlayerNumber == 1 && tankHealth1.m_CurrentHealth <= 0)
            {
                AdjustCamera(player1Camera, player2Camera);
            }

            if (tankMovement2.m_PlayerNumber == 2 && tankHealth2.m_CurrentHealth <= 0)
            {
                AdjustCamera(player2Camera, player1Camera);
            }
        }

        void AdjustCamera(Camera inactiveCamera, Camera activeCamera)
        {
            inactiveCamera.enabled = false;
            activeCamera.enabled = true;
            activeCamera.rect = new Rect(0f, 0f, 1f, 1f);
        }
    }
}
