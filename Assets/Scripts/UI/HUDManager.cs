using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUDManager : MonoBehaviour
    {
        [Header("Santé")]
        [SerializeField] private Image heartIcon;
        [SerializeField] private TMP_Text heartText;

        [Header("Missiles")]
        [SerializeField] private Image missileIcon;
        [SerializeField] private TMP_Text missileText;

        [Header("Triple Shot")]
        [SerializeField] private Image tripleShotIcon;
        [SerializeField] private TMP_Text tripleShotText;

        [Header("États de Fin de Partie")]
        [SerializeField] private TMP_Text gameOverText;
        [SerializeField] private TMP_Text winText;

        private void Start()
        {
            if (gameOverText != null) gameOverText.gameObject.SetActive(false);
            if (winText != null) winText.gameObject.SetActive(false);

            // Affiche 0.0s au démarrage
            UpdateTripleShotTimer(0f);
        }

        #region Santé & Munitions
        public void UpdateHealth(int currentHealth, int maxHealth)
        {
            if (heartText != null)
            {
                heartText.text = $"{currentHealth} / {maxHealth}";
            }
        }

        public void UpdateAmmo(int currentAmmo, int maxAmmo)
        {
            if (missileText != null)
            {
                missileText.text = $"{currentAmmo} / {maxAmmo}";
            }
        }
        #endregion

        #region Triple Shot Booster
        public void UpdateTripleShotTimer(float remainingSeconds)
        {
            if (tripleShotText != null)
            {
                tripleShotText.text = $"{Mathf.Max(0f, remainingSeconds):F1}s";
            }
        }
        #endregion

        #region Écrans de Fin
        public void ShowGameOver()
        {
            if (gameOverText != null) gameOverText.gameObject.SetActive(true);
        }

        public void ShowWin()
        {
            if (winText != null) winText.gameObject.SetActive(true);
        }
        #endregion
    }
}